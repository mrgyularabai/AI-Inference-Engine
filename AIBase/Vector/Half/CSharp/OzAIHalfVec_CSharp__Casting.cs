using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    partial class OzAIHalfVec_CSharp
    {
        public override bool ToBytes(out byte[] res, out string error)
        {
            if (Values == null)
            {
                error = $"Could not convert OzAIHalfVec_CSharp to bytes, because it is not initialized.";
                res = null;
                return false;
            }

            var halfCount = (ulong)Values.LongLength;
            var byteCount = halfCount * 2;
            res = new byte[byteCount];
            var byteOffset = 0;
            for (ulong i = 0; i < halfCount; i++)
            {
                var val = Values[i];
                var bytes = BitConverter.GetBytes(val);
                res[byteOffset++] = bytes[0];
                res[byteOffset++] = bytes[1];
            }
            error = null;
            return true;
        }

        public override bool ToFloat(out float[] res, out string error)
        {
            if (Values == null)
            {
                error = $"Could not convert OzAIHalfVec_CSharp to bytes, because it is not initialized.";
                res = null;
                return false;
            }

            var numCount = (ulong)Values.LongLength;
            res = new float[numCount];
            for (ulong i = 0; i < numCount; i++)
            {
                var val = Values[i];
                res[i] = (float)val;
            }
            error = null;
            return true;
        }
    }
}
