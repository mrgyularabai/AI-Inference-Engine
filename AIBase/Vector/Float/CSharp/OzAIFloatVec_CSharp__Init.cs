using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIFloatVec_CSharp
    {
        public override bool Init(ulong length, out string error)
        {
            Values = new float[length];
            error = null;
            return true;
        }

        public override bool Init(byte[] data, ulong offset, ulong length, out string error)
        {
            if (data == null)
            {
                error = "OzAIFloatVec_CSharp could not be initialized, because the byte[] 'data' provided was null.";
                return false;
            }
            try
            {
                Values = new float[length];
                var byteCount = length * 4;
                var byteOffset = offset * 4;
                if (!CheckBlockCopy(data, "Data", byteOffset, 0, byteCount, out var needsULong, out error))
                {
                    error = "Could not init OzAIFloatVec_CSharp: " + error;
                    return false;
                }
                Buffer.BlockCopy(data, (int)byteOffset, Values, 0, (int)byteCount);
            }
            catch (Exception ex)
            {
                error = "Could not init OzAIFloatVec_CSharp:" + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override bool Init(float[] data, ulong offset, ulong length, out string error)
        {
            if (data == null)
            {
                error = "OzAIFloatVec_CSharp could not be initialized, because the float[] 'data' provided was null.";
                return false;
            }
            try
            {
                Values = new float[length];
                var byteCount = length * 4;
                var byteOffset = offset * 4;
                if (!CheckBlockCopy(data, "Data", offset, 0, length, out var needsULong, out error))
                {
                    error = "Could not init OzAIFloatVec_CSharp: " + error;
                    return false;
                }
                Buffer.BlockCopy(data, (int)byteOffset, Values, 0, (int)byteCount);
            }
            catch (Exception ex)
            {
                error = "Could not init OzAIFloatVec_CSharp:" + ex.Message;
                return false;
            }
            error = null;
            return true;
        }
    }
}
