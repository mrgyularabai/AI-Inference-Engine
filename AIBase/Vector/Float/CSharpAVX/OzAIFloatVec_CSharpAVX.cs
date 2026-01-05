using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIFloatVec_CSharpAVX : OzAIFloatVec
    {
        public override bool GetBlockCount(out ulong size, out string error)
        {
            error = "AVX not implemented yet";
            size = ulong.MaxValue;
            return false;
        }

        public override bool GetBytesPerBlock(out ulong size, out string error)
        {
            error = "AVX not implemented yet";
            size = ulong.MaxValue;
            return false;
        }

        public override bool GetNumCount(out ulong size, out string error)
        {
            error = "AVX not implemented yet";
            size = ulong.MaxValue;
            return false;
        }

        public override bool GetNth(ulong index, out float res, out string error)
        {
            error = "AVX not implemented yet";
            res = float.NaN;
            return false;
        }

        public override bool GetNumsPerBlock(out ulong size, out string error)
        {
            error = "AVX not implemented yet";
            size = ulong.MaxValue;
            return false;
        }

        public override bool GetSize(out ulong size, out string error)
        {
            error = "AVX not implemented yet";
            size = ulong.MaxValue;
            return false;
        }

        public override bool Init(ulong length, out string error)
        {
            error = "AVX not implemented yet";
            return false;
        }

        public override bool Init(byte[] data, ulong offset, ulong length, out string error)
        {
            error = "AVX not implemented yet";
            return false;
        }

        public override bool Init(float[] data, ulong offset, ulong length, out string error)
        {
            error = "AVX not implemented yet";
            return false;
        }

        public override bool ToBytes(out byte[] res, out string error)
        {
            res = null;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool ToFloat(out float[] res, out string error)
        {
            res = null;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool Clone(out OzAIVector res, out string error)
        {
            throw new NotImplementedException();
        }
    }
}
