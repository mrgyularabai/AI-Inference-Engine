using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIOpMax : OzAIOperation
    {
        public override OzAIOperationType Type => OzAIOperationType.Max;
        public OzAIVectorRange Source;
        public OzAIScalar Destination;

        public override bool IsPossible(out string error)
        {
            if (!CheckAreRangesValid([[Source], [Destination]], ["Inputs (Only one range)", "Output (Only one range)"], out error))
                return false;
            error = null;
            return true;
        }
    }
}
