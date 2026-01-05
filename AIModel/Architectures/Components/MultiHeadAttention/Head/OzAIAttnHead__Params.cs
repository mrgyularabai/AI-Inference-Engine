using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIAttnHead
    {

        public CompMem Mem;
        public class CompMem : OzAICompIOMem
        {
            public OzAIMemNode Inputs;
            public OzAIMemNode Values;
            public OzAIMemNode Keys;
            public OzAIMemNode Outputs;

            public override bool IsPossible(out string error)
            {
                List<object> objs = [Inputs, Values, Keys, Outputs];
                List<string> names = ["Inputs", "Values", "Keys", "Outputs"];
                return CheckIfNull(objs, names, out error);
            }
        }

        public CompIParams IParams;
        public class CompIParams : OzAICompIParams
        {
            public OzAIMatrix QueryMat;

            public override bool IsPossible(out string error)
            {
                List<object> objs = [QueryMat, ExecManager];
                List<string> names = ["QueryMat", "ExecManager"];
                return CheckIfNull(objs, names, out error);
            }
        }

        public CompHParams HParams;
        public class CompHParams : OzAICompHParams
        {

            public ulong ValLen = 64;
            public ulong KeyLen = 64;
            public OzAIScalar Scale;
            public OzAIRoPE.CompHParams RoPEParams;

            public override bool SetDefaults(OzAIProcMode mode, out string error)
            {
                if (!mode.GetCPUSettings(out var cpu, out error))
                    return false;
                if (!OzAIScalar.CreateHalf((Half)1 / Half.Sqrt((Half)KeyLen), mode, out Scale, out error))
                    return false;
                return true;
            }

            public override bool IsPossible(out string error)
            {
                if (!CheckIfNull([RoPEParams, Scale], ["RoPEParams", "Scale"], out error))
                    return false;
                return RoPEParams.IsPossible(out error);
            }
        }

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not CompMem)
            {
                error = "IO memory required to be in a format for a unary op OzAIAttnHead.CompMem.";
                return false;
            }
            if (Params.IParams is not CompIParams)
            {
                error = "Instance parameters required to be in a format for a OzAIAttnHead.CompIParams.";
                return false;
            }
            if (Params.HParams is not CompHParams)
            {
                error = "Hyperparameters required to be in a format for a OzAIAttnHead.CompHParams.";
                return false;
            }

            Mem = Params.Mem as CompMem;
            IParams = Params.IParams as CompIParams;
            HParams = Params.HParams as CompHParams;

            error = null;
            return true;
        }
    }
}
