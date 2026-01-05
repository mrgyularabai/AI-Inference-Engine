using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIBinaryOperation : OzAIOperation
    {
        public OzAIVectorRange[] Source1, Source2, Destination;
        public override bool IsPossible(out string error)
        {
            if (Source1.LongLength != Source2.LongLength)
            {
                error = $"{Type} is not possible, becuase different number of vectors given for two sources.";
                return false;
            }

            if (Source1.LongLength != Destination.LongLength)
            {
                error = $"{Type} is not possible, becuase different number of source and destination vectors given.";
                return false;
            }

            if (!CheckAreRangesValid([Source1, Source2, Destination], ["Source 1", "Source 2", "Destination"], out error))
                return false;

            for (int i = 0; i < Source1.LongLength; i++)
            {
                var len1 = Source1[i].Length;
                var len2 = Source2[i].Length;
                if (len1 != len2)
                {
                    error = $"{Type} is not possible, becuase sources contain ranges with incompatible lengths for vector ranges number {i}.";
                    return false;
                }
            }

            for (int i = 0; i < Source1.LongLength; i++)
            {
                var len1 = Source1[i].Length;
                var len2 = Destination[i].Length;
                if (len1 != len2)
                {
                    error = $"{Type} is not possible, becuase the sources vs the destination contain ranges with incompatible lengths for vector ranges number {i}.";
                    return false;
                }
            }

            error = null;
            return true;
        }
    }
}
