using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIMatMul : OzAIOperation
    {
        public override OzAIOperationType Type => OzAIOperationType.MatMul;
        public OzAIMatrixRange Matrix;
        public OzAIVectorRange[] Source, Destination;

        public override bool IsPossible(out string error)
        {
            if (Source.LongLength != Destination.LongLength)
            {
                error = $"{Type} is not possible, becuase different number of source and destination vectors given.";
                return false;
            }

            if (!CheckAreRangesValid([Destination, [Matrix]], ["Destination", "Matrix (Only 1 matrix range)"], out error))
                return false;

            for (int i = 0; i < Source.LongLength; i++)
            {
                var len1 = Source[i].Length;
                var len2 = Matrix.Counts.Item1;
                if (len1 != len2)
                {
                    error = $"{Type} is not possible, becuase the source's vector {i} did not have a length that equals the width of the matrix range specified: len: {len1}, width: {len2}";
                    return false;
                }
            }

            for (int i = 0; i < Destination.LongLength; i++)
            {
                var len1 = Destination[i].Length;
                var len2 = Matrix.Counts.Item2;
                if (len1 != len2)
                {
                    error = $"{Type} is not possible, becuase the destination's vector {i} did not have a length that equals the height of the matrix range specified: len: {len1}, height: {len2}";
                    return false;
                }
            }

            error = null;
            return true;
        }
    }
}
