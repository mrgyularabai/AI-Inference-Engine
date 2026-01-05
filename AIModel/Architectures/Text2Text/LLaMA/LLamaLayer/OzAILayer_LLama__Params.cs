using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAILayer_LLama
    {
        public OzAICompIOMem_Unary Mem;
        public CompIParams IParams;
        public class CompIParams : OzAICompIParams
        {
            public OzAIRMSNorm.CompIParams AttnNorm;
            public OzAIMultiHeadAttn.CompIParams Attn;
            public OzAIRMSNorm.CompIParams GLUNorm;
            public OzAIGLU.CompIParams GLU;

            public override bool IsPossible(out string error)
            {
                List<object> objs = [AttnNorm, Attn, GLUNorm, GLU];
                List<string> names = ["AttnNorm", "Attn", "GLUNorm", "GLU"];
                if (!CheckIfNull(objs, names, out error))
                    return false;

                if (!AttnNorm.IsPossible(out error)) return false;
                if (!Attn.IsPossible(out error)) return false;
                if (!GLUNorm.IsPossible(out error)) return false;
                if (!GLU.IsPossible(out error)) return false;

                return true;
            }
        }

        public CompHParams HParams;
        public class CompHParams : OzAICompHParams
        {
            public OzAIRMSNorm.CompHParams Norm;
            public OzAIMultiHeadAttn.CompHParams Attn;
            public OzAIGLU.CompHParams GLU;

            public override bool IsPossible(out string error)
            {
                List<object> objs = [Attn, Norm, GLU];
                List<string> names = ["Attn", "Norm", "GLU"];
                if (!CheckIfNull(objs, names, out error))
                    return false;

                if (!Norm.IsPossible(out error)) return false;
                if (!Attn.IsPossible(out error)) return false;
                if (!GLU.IsPossible(out error)) return false;

                return true;
            }
        }

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not OzAICompIOMem_Unary)
            {
                error = "IO memory required to be in a format for a unary op OzAICompIOMem_Unary.";
                return false;
            }
            if (Params.IParams is not CompIParams)
            {
                error = "Instance parameters required to be in a format for a OzAILayer_LLama.CompIParams.";
                return false;
            }
            if (Params.HParams is not CompHParams)
            {
                error = "Hyperparameters required to be in a format for a OzAILayer_LLama.CompHParams.";
                return false;
            }

            Mem = Params.Mem as OzAICompIOMem_Unary;
            IParams = Params.IParams as CompIParams;
            HParams = Params.HParams as CompHParams;

            error = null;
            return true;
        }
    }
}
