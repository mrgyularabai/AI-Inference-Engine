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
    partial class OzAIHalfVec_CSharp
    {
        public override bool Init(ulong length, out string error)
        {
            Values = new Half[length];
            error = null;
            return true;
        }

        public override bool Init(byte[] data, ulong byteOffset, ulong byteCount, out string error)
        {
            if (data == null)
            {
                error = $"Could not initialize OzAIHalfVec_CSharp, because no byte values provided.";
                return false;
            }

            if (byteCount % 2 != 0)
            {
                error = $"Could not initialize OzAIHalfVec_CSharp, because invalid number of bytes given.";
                return false;
            }

            if (byteOffset + byteCount > int.MaxValue)
            {
                error = $"Could not initialize OzAIHalfVec_CSharp, because the range of data provided contains indicies that exceed the maximum index that can be stored in an int: { byteOffset + byteCount}.";
                return false;
            }

            var halfCount = byteCount / 2;
            Values = new Half[halfCount];
            for (ulong i = 0; i < halfCount; i++)
            {
                Values[i] = BitConverter.ToHalf(data, (int)byteOffset);
                byteOffset += 2;
            }
            error = null;
            return true;
        }

        public override bool Init(float[] data, ulong offset, ulong length, out string error)
        {
            if (data == null)
            {
                error = $"Could not initialize OzAIHalfVec_CSharp, because no float values provided.";
                return false;
            }
            Values = new Half[length];
            for (ulong i = 0; i < length; i++)
            {
                Values[i] = (Half)data[offset++];
            }
            error = null;
            return true;
        }
    }
}
