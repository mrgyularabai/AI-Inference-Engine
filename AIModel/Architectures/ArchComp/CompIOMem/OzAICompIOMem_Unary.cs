using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAICompIOMem_Unary : OzAICompIOMem
    {
        public OzAIMemNode Inputs;
        public OzAIMemNode Outputs;

        public override bool IsPossible(out string error)
        {
            return CheckIfNull([Inputs, Outputs], ["Inputs", "Outputs"], out error);
        }
    }
}
