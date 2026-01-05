using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAISwish1 : OzAIActivation
    {
        public override string Name => "OzAISwish1";

        public OzAICompIOMem_Unary Mem;

        public override bool IsPossible(out string error)
        {
            if (Params.Mem is not OzAICompIOMem_Unary)
            {
                error = "IO memory required to be in a format for a unary op OzAICompIOMem_Unary";
                return false;
            }
            error = null;
            return true;
        }

        protected override bool InitInner(out string error)
        {
            Mem = Params.Mem as OzAICompIOMem_Unary;
            error = null;
            return true;
        }

        public override bool Forward(out string error)
        {
            var inps = Mem.Inputs.GetArray();
            var outs = Mem.Outputs.GetArray();
            var exec = Params.IParams.ExecManager;
            if (!exec.Swish1(inps, outs, out error))
                return false;
            error = null;
            return true;
        }
    }
}
