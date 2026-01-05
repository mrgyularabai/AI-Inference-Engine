using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIFloatMat_CSharp
    {
        public override bool ToBytes(out byte[] res, out string error)
        {
            if (Values == null)
            {
                res = null;
                error = "Could not convert OzAIFloatMat_CSharp's values to bytes, because it is not initialized.";
                return false;
            }
            var byteCount = (ulong)Values.LongLength * 4;
            res = new byte[byteCount];
            if (!CheckBlockCopy(Values, "Values", 0, 0, byteCount, out var needsULong, out error))
            {
                res = null;
                error = "Could not convert OzAIFloatMat_CSharp's values to bytes, because copying data to byte array failed: " + error;
                return false;
            }
            Buffer.BlockCopy(Values, 0, res, 0, (int)byteCount);
            error = null;
            return true;
        }

        public override bool ToFloats(out float[] res, out string error)
        {
            if (Values == null)
            {
                res = null;
                error = "Could not convert OzAIFloatMat_CSharp's values to floats, because it is not initialized.";
                return false;
            }
            res = new float[Values.LongLength];
            var byteCount = (ulong)Values.LongLength * 4;
            if (!CheckBlockCopy(Values, "Values", 0, 0, byteCount, out var needsULong, out error))
            {
                res = null;
                error = "Could not convert OzAIFloatMat_CSharp's values to floats, because copying data to float array failed: " + error;
                return false;
            }
            Buffer.BlockCopy(Values, 0, res, 0, (int)byteCount);
            error = null;
            return true;
        }
    }
}
