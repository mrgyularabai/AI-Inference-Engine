using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAISoftMax : OzAIOperation
    {
        public override OzAIOperationType Type => OzAIOperationType.SoftMax;
        public OzAIVector[] Source;
        public OzAIVectorRange[] Destination;

        public override bool IsPossible(out string error)
        {
            if (Source.LongLength != Destination.LongLength)
            {
                error = $"{Type} is not possible, becuase different number of source and destination vectors given.";
                return false;
            }

            if (!CheckAreRangesValid([Destination], ["Destination"], out error))
                return false;

            for (int i = 0; i < Source.LongLength; i++)
            {
                var vec = Source[i];
                if (!vec.GetNumCount(out var len1, out error))
                {
                    error = $"Could not check whether dot product would be possible: " + error;
                    return false;
                }
                var len2 = Destination[i].Length;
                if (len1 != len2)
                {
                    error = $"{Type} is not possible, becuase the source vs the destination contain ranges with incompatible lengths for range/vector number {i}.";
                    return false;
                }
            }

            error = null;
            return true;
        }
    }
}
