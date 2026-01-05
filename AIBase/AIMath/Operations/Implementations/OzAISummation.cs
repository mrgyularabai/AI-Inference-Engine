using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAISummation : OzAIOperation
    {
        public override OzAIOperationType Type => OzAIOperationType.Sum;
        public OzAIVectorRange[] Source;
        public OzAIVectorRange Destination;

        public override bool IsPossible(out string error)
        {
            if ((ulong)Source.LongLength != Destination.Length)
            {
                error = $"{Type} is not possible, becuase different number of source and destination vectors given.";
                return false;
            }

            if (!CheckAreRangesValid([Source, [Destination]], ["Source", "Destination (only 1 range)"], out error))
                return false;

            error = null;
            return true;
        }
    }
}
