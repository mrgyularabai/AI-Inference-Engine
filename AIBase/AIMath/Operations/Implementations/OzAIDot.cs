using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIDot : OzAIOperation
    {
        public override OzAIOperationType Type => OzAIOperationType.Dot;
        public OzAIVector[] Source1;
        public OzAIVector[] Source2;
        public OzAIVectorRange Destination;

        public override bool IsPossible(out string error)
        {
            if (Source1.LongLength != Source2.LongLength)
            {
                error = $"{Type} is not possible, becuase different number of vectors given for two sources.";
                return false;
            }

            if ((ulong)Source1.LongLength != Destination.Length)
            {
                error = $"{Type} is not possible, becuase different number of source and destination vectors given.";
                return false;
            }

            if (!CheckAreRangesValid([[Destination]], ["Destination (only 1 range)"], out error))
                return false;

            for (int i = 0; i < Source1.LongLength; i++)
            {
                var vec1 = Source1[i];
                if (!vec1.GetNumCount(out var len1, out error))
                {
                    error = $"Could not check whether dot product would be possible: " + error;
                    return false;
                }
                var vec2 = Source2[i];
                if (!vec2.GetNumCount(out var len2, out error))
                {
                    error = $"Could not check whether dot product would be possible: " + error;
                    return false;
                }
                if (len1 != len2)
                {
                    error = $"{Type} is not possible, becuase sources contain vectors with incompatible lengths for vectors number {i}.";
                    return false;
                }
            }

            error = null;
            return true;
        }
    }
}
