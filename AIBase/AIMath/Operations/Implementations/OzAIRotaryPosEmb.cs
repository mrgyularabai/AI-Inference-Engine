using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIRotaryPosEmb : OzAIOperation
    {
        public override OzAIOperationType Type => OzAIOperationType.RoPE;
        public ulong[] SourcePositions;
        public OzAIVectorRange[] Source;
        public OzAIScalar ThetaBase;
        public OzAIVectorRange[] Destination;

        public override bool IsPossible(out string error)
        {
            List<object> objs = [Source, SourcePositions, ThetaBase, Destination];
            List<string> names = ["Source", "SourcePositions", "ThetaBase", "Destination"];

            if (!CheckIfNull(objs, names, out error))
                return false;

            if (Source.LongLength != Destination.LongLength)
            {
                error = $"{Type} is not possible, becuase different number of source and destination vectors given.";
                return false;
            }

            if (SourcePositions.LongLength != Destination.LongLength)
            {
                error = $"{Type} is not possible, becuase different number of source positions and destination vectors given.";
                return false;
            }

            if (!CheckAreRangesValid([Source, [ThetaBase], Destination], ["Source", "ThetaBase (only 1 range)", "Destination"], out error))
                return false;

            for (int i = 0; i < Source.LongLength; i++)
            {
                var len1 = Source[i].Length;
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
