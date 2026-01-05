using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIArch_LLaMA
    {
        bool createLayerParams(OzGGUFFile file, int i, OzAIMemNode modelMem, OzAILayer_LLama.CompHParams hParams, out OzAICompParams res, out string error)
        {
            res = null;

            var layerMem = createLayerMem(modelMem);

            if (!createLayerIParams(file, i, hParams, out var layerIParams, out error))
                return false;

            res = new OzAICompParams()
            {
                Mem = layerMem,
                IParams = layerIParams,
                HParams = hParams,
            };

            return true;
        }

        OzAICompIOMem_Unary createLayerMem(OzAIMemNode modelMem)
        {
            return new OzAICompIOMem_Unary()
            {
                Inputs = modelMem,
                Outputs = modelMem
            };
        }

        bool createLayerIParams(OzGGUFFile file, int i, OzAILayer_LLama.CompHParams hParams, out OzAILayer_LLama.CompIParams res, out string error)
        {
            res = new OzAILayer_LLama.CompIParams();

            res.ExecManager = ExecManager;

            // RMS
            if (!file.GetVector($"blk.{i}.attn_norm.weight", Mode, out var attnNorm, out error, false))
                return false;
            res.AttnNorm = createRMSIParams(Mode, attnNorm);

            // Attn
            if (!createAttnIParams(file, Mode, i, hParams.Attn, out res.Attn, out error))
                return false;

            // RMS
            if (!file.GetVector($"blk.{i}.ffn_norm.weight", Mode, out var gluNorm, out error, false))
                return false;
            res.GLUNorm = createRMSIParams(Mode, gluNorm);

            // GLU
            if (!createGLUIParams(file, i, out res.GLU, out error))
                return false;

            return true;
        }

        bool createGLUIParams(OzGGUFFile file, int i, out OzAIGLU.CompIParams res, out string error)
        {
            res = null;

            if (!file.GetMatrix($"blk.{i}.ffn_up.weight", Mode, out var top, out error))
                return false;
            if (!file.GetMatrix($"blk.{i}.ffn_gate.weight", Mode, out var gate, out error))
                return false;
            if (!file.GetMatrix($"blk.{i}.ffn_down.weight", Mode, out var bottom, out error))
                return false;

            var activation = new OzAISwish1();
            var actParams = new OzAICompIParams_ExecOnly() { ExecManager = ExecManager };
            res = new OzAIGLU.CompIParams()
            {
                ExecManager = ExecManager,
                Activation = activation,
                ActivationParams = actParams,
                WeightsTop = top,
                WeightsBottom = bottom,
                WeightsGate = gate
            };

            error = null;
            return true;
        }

        bool createAttnIParams(OzGGUFFile file, OzAIProcMode mode, int i, OzAIMultiHeadAttn.CompHParams hparams, out OzAIMultiHeadAttn.CompIParams res, out string error)
        {
            res = null;
            if (!file.GetMatrix($"blk.{i}.attn_output.weight", Mode, out var output, out error))
                return false;

            if (!getAttnGroupsIParams(file, i, hparams, out var groups, out error))
                return false;

            res = new OzAIMultiHeadAttn.CompIParams()
            {
                ExecManager = ExecManager,
                GroupParams = groups,
                OutputWeights = output
            };

            if (!res.SetDefaults(mode, out error))
                return false;

            error = null;
            return true;
        }

        bool getAttnGroupsIParams(OzGGUFFile file, int layerIdx, OzAIMultiHeadAttn.CompHParams hparams, out OzAIAttnGroup.CompIParams[] res, out string error)
        {
            res = null;
            if (!file.GetMatricies($"blk.{layerIdx}.attn_k.weight", Mode, out var keys, (int)hparams.GroupCount, out error))
                return false;
            if (!file.GetMatricies($"blk.{layerIdx}.attn_v.weight", Mode, out var values, (int)hparams.GroupCount, out error))
                return false;

            res = new OzAIAttnGroup.CompIParams[hparams.GroupCount];
            for (int i = 0; i < hparams.GroupCount; i++)
            {
                if (!getAttnHeadIParams(file, layerIdx, i, hparams, out var headParams, out error))
                    return false;

                res[i] = new OzAIAttnGroup.CompIParams()
                {
                    ExecManager = ExecManager,
                    KeyWeights = keys[i],
                    ValueWeights = values[i],
                    HeadParams = headParams,
                };
            }

            return true;
        }

        bool getAttnHeadIParams(OzGGUFFile file, int layerIdx, int groupIdx, OzAIMultiHeadAttn.CompHParams hparams, out OzAIAttnHead.CompIParams[] res, out string error)
        {
            res = null;
            var totalHeadCount = hparams.GroupCount * hparams.GroupParams.HeadCount;
            if (!file.GetMatricies($"blk.{layerIdx}.attn_q.weight", Mode, out var querys, (int)totalHeadCount, out error))
                return false;

            var queryOffset = groupIdx * (int)hparams.GroupParams.HeadCount;
            res = new OzAIAttnHead.CompIParams[hparams.GroupParams.HeadCount];
            for (int i = 0; i < hparams.GroupParams.HeadCount; i++)
            {
                res[i] = new OzAIAttnHead.CompIParams()
                {
                    ExecManager = ExecManager,
                    QueryMat = querys[queryOffset + i],
                };
            }

            return true;
        }

        OzAIRMSNorm.CompIParams createRMSIParams(OzAIProcMode mode, OzAIVector gain)
        {
            if (!OzAIScalar.CreateFloat(1f, mode, out var part, out string error))
                return null;
            return new OzAIRMSNorm.CompIParams()
            {
                ExecManager = ExecManager,
                Part = part,
                Gain = gain
            };
        }

        bool createLayer(OzAICompParams layerParams, out string error)
        {
            var layer = new OzAILayer_LLama();
            if (!layer.Init(layerParams, out error))
                return false;
            Layers.Add(layer);
            return true;
        }
    }
}
