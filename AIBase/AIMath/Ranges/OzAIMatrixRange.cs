using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIMatrixRange : OzAITensRange
    {
        public OzAIMatrix Matrix;
        public Tuple<ulong, ulong> StartCoords;
        public Tuple<ulong, ulong> Counts;

        public static bool ToFull(OzAIMatrix matrix, out OzAIMatrixRange res, out string error)
        {
            res = null;
            if (!matrix.GetHeight(out var height, out error))
                return false;
            if (!matrix.GetWidth(out var width, out error))
                return false;
            res =  new OzAIMatrixRange()
            {
                Matrix = matrix,
                StartCoords = new Tuple<ulong, ulong>(0, 0),
                Counts = new Tuple<ulong, ulong>(width, height)
            };
            return true;
        }

        public override bool IsValid(out string error)
        {
            if (Matrix == null)
            {
                error = $"Matrix range is not valid for any operation, since it contains no reference to a matrix.";
                return false;
            }
            if (!Matrix.GetWidth(out var width, out error))
            {
                error = $"Could not check whether matrix range would be valid for any operation: " + error;
                return false;
            }
            if (StartCoords.Item1 + Counts.Item1 > width)
            {
                error = $"Matrix range is not valid for any operation, since it is out of bounds for its matrix's columns: off:{StartCoords.Item1}, len:{Counts.Item1}, orignial mats width: {width}";
                return false;
            }
            if (Counts.Item1 == 0)
            {
                error = $"Matrix range is not valid for any operation, since it contains no columns.";
                return false;
            }
            if (!Matrix.GetHeight(out var height, out error))
            {
                error = $"Could not check whether matrix range would be valid for any operation: " + error;
                return false;
            }
            if (StartCoords.Item2 + Counts.Item2 > height)
            {
                error = $"Matrix range is not valid for any operation, since it is out of bounds for its matrix's rows: off:{StartCoords.Item2}, len:{Counts.Item2}, orignial mats height: {height}";
                return false;
            }
            if (Counts.Item2 == 0)
            {
                error = $"Matrix range is not valid for any operation, since it contains no rows.";
                return false;
            }
            error = null;
            return true;
        }
    }
}
