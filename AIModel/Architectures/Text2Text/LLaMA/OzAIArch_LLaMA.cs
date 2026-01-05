using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public partial class OzAIArch_LLaMA : OzAIArch_Text2Text
    {
        // Tensor Architecture
        public uint LayerCount; // block_count
        public List<OzAILayer_LLama> Layers;
        public OzAIRMSNorm OutNorm;


        public override bool Forward(out string error)
        {
            Embedding.Mem.Input = IN as OzAIIntVec;
            if (!Embedding.Forward(out error))
                return false;
            for (int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                if (!layer.Forward(out error))
                    return false;
            }
            if (!OutNorm.Forward(out error))
                return false;
            if (!Unembedding.Forward(out error))
                return false;
            OUT = Unembedding.Mem.Output;
            return true;
        }

        protected override bool ArchInitialize(OzGGUFFile file, out string error)
        {
            if (!initExecManager(out error)) return false;

            if (!GetRMSHParams(Mode, file, out var rmsHParams, out error)) return false;
            if (!GetAttnHParams(Mode, file, out var attnHParams, out error)) return false;
            if (!GetGLUHParams(Mode, file, out var gluHParams, out error)) return false;
            var layerHParams = createLayerHParams(rmsHParams, attnHParams, gluHParams);

            if (!GetModelSize(file, attnHParams, out error)) return false;

            var modelMem = new OzAIMemNode();
            if (!GetEmbeddings(file, modelMem, out error)) return false;

            Layers = new List<OzAILayer_LLama>();
            for (int i = 0; i < LayerCount; i++)
            {
                if (!createLayerParams(file, i, modelMem, layerHParams, out var layerParams, out error)) return false;
                if (!createLayer(layerParams, out error)) return false;
            }

            if (!createOutNorm(file, modelMem, rmsHParams, out error))
                return false;
            if (!createUnembedding(file, modelMem, out error))
                return false;

            return true;
        }

        bool initExecManager(out string error)
        {
            if (!OzAIProcMode.GetDefaults(out Mode, out error))
                return false;

            ExecManager = new OzAIExecManager();
            if (!ExecManager.Init(Mode, out error)) return false;

            return true;
        }

        OzAILayer_LLama.CompHParams createLayerHParams(OzAIRMSNorm.CompHParams rms, OzAIMultiHeadAttn.CompHParams attn, OzAIGLU.CompHParams glu)
        {
            return new OzAILayer_LLama.CompHParams()
            {
                Norm = rms,
                Attn = attn,
                GLU = glu,
            };
        }

        bool GetRMSHParams(OzAIProcMode mode, OzGGUFFile file, out OzAIRMSNorm.CompHParams res, out string error)
        {
            res = null;
            if (!file.GetMDFloat32($"{Name}.attention.layer_norm_rms_epsilon", out var epsilonF, out error))
                return false;
            if (!OzAIScalar.CreateFloat(epsilonF, mode, out var epsilon, out error))
                return false;
            res = new OzAIRMSNorm.CompHParams()
            {
                Epsilon = epsilon
            };
            return true;
        }

        bool GetAttnHParams(OzAIProcMode mode, OzGGUFFile file, out OzAIMultiHeadAttn.CompHParams hparams, out string error)
        {
            hparams = null;

            var ropeParams = new OzAIRoPE.CompHParams();
            if (!ropeParams.InitFromFile(mode, file, out error)) return false;

            var headParams = new OzAIAttnHead.CompHParams();
            headParams.RoPEParams = ropeParams;
            if (!headParams.SetDefaults(mode, out error)) return false;

            var groupParams = new OzAIAttnGroup.CompHParams();
            groupParams.HeadParams = headParams;
            if (!file.GetMDUInt32($"{Name}.attention.head_count", out var totalHeadCount, out error))
                return false;
            if (!file.GetMDUInt32($"{Name}.attention.head_count_kv", out var groupCount, out error, false, totalHeadCount) && error != null)
                return false;
            groupParams.HeadCount = totalHeadCount / groupCount;

            hparams = new OzAIMultiHeadAttn.CompHParams();
            hparams.GroupParams = groupParams;
            hparams.GroupCount = groupCount;

            return true;
        }

        bool GetGLUHParams(OzAIProcMode mode, OzGGUFFile file, out OzAIGLU.CompHParams res, out string error)
        {
            res = new OzAIGLU.CompHParams()
            {
                ActivationParams = null,
                FFNLength = 8192
            };
            error = null;
            return true;
        }

        bool GetModelSize(OzGGUFFile file, OzAIMultiHeadAttn.CompHParams hparams, out string error)
        {
            uint groupCount = hparams.GroupCount;
            uint headCount = hparams.GroupParams.HeadCount * hparams.GroupCount;

            if (!file.GetMDUInt32($"{Name}.block_count", out LayerCount, out error))
                return false;

            switch (LayerCount)
            {
                case 16: Size = OzAIModelSize.PCount1B; break; // Llama 3.2 1B
                case 22: Size = OzAIModelSize.PCount1B; break;
                case 26: Size = OzAIModelSize.PCount3B; break;
                case 28: Size = OzAIModelSize.PCount3B; break; // Llama 3.2 3B
                //llama.cpp: granite uses a vocab with len 49152
                case 32:
                    uint vocabSize;
                    if (!file.GetMDUInt32($"{Name}.vocab_size", out vocabSize, out error, false))
                    {
                        if (error != null)
                            return false;

                        if (!file.GetMDUInt32($"tokenizer.ggml.tokens", out vocabSize, out error))
                            return false;
                    }
                    Size = vocabSize == 49152 ? OzAIModelSize.PCount3B : (vocabSize < 40000 ? OzAIModelSize.PCount7B : OzAIModelSize.PCount8B); break;
                case 36: Size = OzAIModelSize.PCount8B; break; //llama.cpp: granite
                case 40: Size = OzAIModelSize.PCount13B; break;
                case 48: Size = OzAIModelSize.PCount34B; break;
                case 60: Size = OzAIModelSize.PCount30B; break;
                case 80: Size = headCount == groupCount ? OzAIModelSize.PCount65B : OzAIModelSize.PCount70B; break;
                default: Size = OzAIModelSize.UNKNOWN; break;
            }

            return true;
        }

        bool GetEmbeddings(OzGGUFFile file, OzAIMemNode output, out string error)
        {
            if (!file.GetVectors("token_embd.weight", Mode, out var embeddings, out error))
                return false;

            Embedding = new OzAIEmbedding();
            var embedMem = new OzAIEmbedding.CompMem()
            {
                Outputs = output
            };
            var embedIParams = new OzAIEmbedding.CompIParams()
            {
                ExecManager = ExecManager,
                Embeddings = embeddings.ToArray()
            };
            var embedParams = new OzAICompParams()
            {
                Mem = embedMem,
                IParams = embedIParams,
                HParams = null
            };
            if (!Embedding.Init(embedParams, out error))
                return false;

            return true;
        }

        bool createOutNorm(OzGGUFFile file, OzAIMemNode modelMem, OzAIRMSNorm.CompHParams hparams, out string error)
        {
            OutNorm = new OzAIRMSNorm();
            var outNormMem = new OzAICompIOMem_Unary()
            {
                Inputs = modelMem,
                Outputs = modelMem
            };

            if (!file.GetVector("output_norm.weight", Mode, out var outNorm, out error, false))
                return false;
            var outNormIParams = createRMSIParams(Mode, outNorm);

            var rmsParams = new OzAICompParams()
            {
                Mem = outNormMem,
                IParams = outNormIParams,
                HParams = hparams
            };

            if (!OutNorm.Init(rmsParams, out error))
                return false;

            return true;
        }

        bool createUnembedding(OzGGUFFile file, OzAIMemNode modelMem, out string error)
        {
            var unembedMem = new OzAIUnembedding.CompMem()
            {
                Inputs = modelMem
            };
            if (!OzAIIntVec.Create(Mode, out unembedMem.Output, out error))
                return false;

            var unembedIParams = new OzAIUnembedding.CompIParams()
            {
                ExecManager = ExecManager,
                Embeddings = Embedding.IParams.Embeddings
            };

            var unembedHParams = new OzAIUnembedding.CompHParams()
            {
                VocabSize = (ulong)Embedding.IParams.Embeddings.LongLength
            };

            var unembedParams = new OzAICompParams()
            {
                Mem = unembedMem,
                IParams = unembedIParams,
                HParams = unembedHParams
            };

            Unembedding = new OzAIUnembedding();
            if (!Unembedding.Init(unembedParams, out error))
                return false;

            return true;
        }


        protected override string GetName()
        {
            return "llama";
        }
    }
}
