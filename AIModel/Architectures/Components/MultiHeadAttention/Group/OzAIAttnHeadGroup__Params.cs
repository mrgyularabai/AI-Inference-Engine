using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIAttnGroup
    {
        public CompMem Mem;
        public class CompMem : OzAICompIOMem
        {
            public OzAIMemNode Inputs;
            public OzAIMemNode[] Outputs;

            public override bool IsPossible(out string error)
            {
                List<object> objs = [Inputs, Outputs];
                List<string> names = ["Inputs", "Outputs"];
                return CheckIfNull(objs, names, out error);
            }
        }

        public CompIParams IParams;
        public class CompIParams : OzAICompIParams
        {
            public OzAIMatrix ValueWeights;
            public OzAIMatrix KeyWeights;
            public OzAIAttnHead.CompIParams[] HeadParams;

            public override bool IsPossible(out string error)
            {
                List<object> objs = [ExecManager, ValueWeights, KeyWeights, HeadParams];
                List<string> names = ["ExecManager","ValueWeights", "KeyWeights", "AttnParams"];
                if (!CheckIfNull(objs, names, out error))
                    return false;

                for (int i = 0; i < HeadParams.Length; i++)
                {
                    var head = HeadParams[i];
                    if (head == null)
                    {
                        error = $"No attention parameters provided for head number {i}."; ;
                        return false;
                    }
                    if (!HeadParams[i].IsPossible(out error))
                    {
                        error = $"No attention parameters for head number {i} not possible: " + error; ;
                        return false;
                    }
                }

                error = null;
                return true;
            }
        }

        public CompHParams HParams;
        public class CompHParams : OzAICompHParams
        {
            public uint HeadCount = 4;
            public OzAIAttnHead.CompHParams HeadParams;

            public override bool IsPossible(out string error)
            {
                if (!CheckIfNull(HeadParams, "AttnParams", out error))
                    return false;
                return HeadParams.IsPossible(out error);
            }
        }

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not CompMem)
            {
                error = "IO memory required to be in a format for a OzAIAttnGroup.CompMem.";
                return false;
            }
            if (Params.IParams is not CompIParams)
            {
                error = "Instance parameters required to be in a format for a OzAIAttnGroup.CompIParams.";
                return false;
            }
            if (Params.HParams is not CompHParams)
            {
                error = "Hyperparameters required to be in a format for a OzAIAttnGroup.CompHParams.";
                return false;
            }

            Mem = Params.Mem as CompMem;
            IParams = Params.IParams as CompIParams;
            HParams = Params.HParams as CompHParams;

            if (Mem.Outputs.Length != HParams.HeadCount)
            {
                error = "Number of outputs provided does not match the number of heads";
                return false;
            }

            if (IParams.HeadParams.Length != HParams.HeadCount)
            {
                error = "Number of heads provided does not match the head cout hparam.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
