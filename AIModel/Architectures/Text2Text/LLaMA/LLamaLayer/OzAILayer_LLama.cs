using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    /// <summary>
    /// This contains all the components of an LLama3.2 layer <br/>
    /// VectorWeights:<br/>
    /// - AttnNorm<br/>
    /// - RoPE Frequencies<br/>
    /// - FFNNorm<br/>
    /// - FFNBiases (Optional)<br/>
    /// MatrixWeights:<br/>
    /// - Attn<br/>
    /// - FFN<br/>
    /// </summary>
    public partial class OzAILayer_LLama : OzAIArchComp
    {
        public OzAIMemNode Acc;
        public OzAIMemNode AttnRes;

        public OzAIRMSNorm AttnNorm;
        public OzAIMultiHeadAttn Attn;
        public OzAIRMSNorm GLUNorm;
        public OzAIGLU GLU;

        public override string Name => "OzAILayer_LLama";

        protected override bool InitInner(out string error)
        {
            Acc = new OzAIMemNode();
            AttnRes = new OzAIMemNode();

            if (!createAttnNorm(out error)) return false;
            if (!createAttn(out error)) return false;
            if (!createGLUNorm(out error)) return false;
            if (!createGLU(out error)) return false;

            return true;
        }

        bool createAttnNorm(out string error)
        {
            AttnNorm = new OzAIRMSNorm();

            var attnNormMem = new OzAICompIOMem_Unary()
            {
                Inputs = Mem.Inputs,
                Outputs = Acc,
            };
            var attnNormParams = new OzAICompParams()
            {
                Mem = attnNormMem,
                IParams = IParams.AttnNorm,
                HParams = HParams.Norm
            };

            if (!AttnNorm.Init(attnNormParams, out error))
                return false;

            error = null;
            return true;
        }

        bool createAttn(out string error)
        {
            Attn = new OzAIMultiHeadAttn();

            var attnMem = new OzAICompIOMem_Unary()
            {
                Inputs = Acc,
                Outputs = Acc,
            };
            var attnParams = new OzAICompParams()
            {
                Mem = attnMem,
                IParams = IParams.Attn,
                HParams = HParams.Attn
            };

            if (!Attn.Init(attnParams, out error))
                return false;

            error = null;
            return true;
        }

        bool createGLUNorm(out string error)
        {
            GLUNorm = new OzAIRMSNorm();

            var GLUNormMem = new OzAICompIOMem_Unary()
            {
                Inputs = AttnRes,
                Outputs = Acc,
            };
            var GLUNormParams = new OzAICompParams()
            {
                Mem = GLUNormMem,
                IParams = IParams.GLUNorm,
                HParams = HParams.Norm
            };

            if (!GLUNorm.Init(GLUNormParams, out error))
                return false;

            error = null;
            return true;
        }

        bool createGLU(out string error)
        {
            GLU = new OzAIGLU();

            var GLUMem = new OzAICompIOMem_Unary()
            {
                Inputs = Acc,
                Outputs = Acc,
            };
            var GLUParams = new OzAICompParams()
            {
                Mem = GLUMem,
                IParams = IParams.GLU,
                HParams = HParams.GLU
            };

            if (!GLU.Init(GLUParams, out error))
                return false;

            error = null;
            return true;
        }

        public override bool Forward(out string error)
        {
            if (!initMem(out error)) return false;
            if (!attnBlock(out error)) return false;
            if (!gluBlock(out error)) return false;

            return true;
        }

        bool initMem(out string error)
        {
            if (!Acc.CreateDestOf(Mem.Inputs, out error))
                return false;
            if (!AttnRes.CreateDestOf(Mem.Inputs, out error))
                return false;
            return true;
        }

        bool attnBlock(out string error)
        {
            if (!AttnNorm.Forward(out error))
                return false;
            if (!Attn.Forward(out error))
                return false;

            var exec = IParams.ExecManager;
            var ins = Mem.Inputs.GetArray();
            var acc = Acc.GetArray();
            var attnRes = AttnRes.GetArray();

            if (!exec.Add(ins, acc, attnRes, out error))
                return false;

            return true;
        }

        bool gluBlock(out string error)
        {
            if (!GLUNorm.Forward(out error))
                return false;
            if (!GLU.Forward(out error))
                return false;

            var exec = IParams.ExecManager;
            var acc = Acc.GetArray();
            var attnRes = AttnRes.GetArray();
            var outs = Mem.Outputs.GetArray();

            if (!exec.Add(attnRes, acc, outs, out error))
                return false;

            return true;
        }
    }
}
