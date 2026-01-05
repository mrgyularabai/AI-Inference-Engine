using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIHalfMat_CSharp
    {
        public override bool ToBytes(out byte[] res, out string error)
        {
            if (Values == null)
            {
                res = null;
                error = "Could not convert OzAIHalfMat_CSharp's values to bytes, because it is not initialized.";
                return false;
            }
            res = new byte[Values.LongLength];
            if (!CheckBlockCopy(Values, "Values", 0, 0, (ulong)Values.LongLength, out var needsULong, out error))
            {
                res = null;
                error = "Could not convert OzAIHalfMat_CSharp's values to bytes, because copying data to byte array failed: " + error;
                return false;
            }
            Buffer.BlockCopy(Values, 0, res, 0, Values.Length);
            error = null;
            return true;
        }

        public override bool ToFloats(out float[] res, out string error)
        {
            if (Values == null)
            {
                res = null;
                error = "Could not convert OzAIHalfMat_CSharp's values to floats, because it is not initialized.";
                return false;
            }
            res = new float[_count];
            if (_count > (ulong)int.MaxValue)
            {
                error = $"Could not convert OzAIHalfMat_CSharp's values to floats, because there where more values than what an int32 could hold: {_count}.";
                return false;
            }
            for (ulong i = 0; i < _count; i++)
            {
                var idx = i * 2;
                var val = BitConverter.ToHalf(Values, (int)idx);
                res[i] = (float)val;
            }
            error = null;
            return true;
        }
    }
}
