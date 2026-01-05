using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIHalfMat_CSharpAVX : OzAIHalfMat
    {
        public override bool GetBlockCount(out ulong size, out string error)
        {
            size = ulong.MaxValue;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool GetBytesPerBlock(out ulong size, out string error)
        {
            size = ulong.MaxValue;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool GetNumsPerBlock(out ulong size, out string error)
        {
            error = "AVX not implemented yet";
            size = ulong.MaxValue;
            return false;
        }

        public override bool GetNumCount(out ulong size, out string error)
        {
            size = ulong.MaxValue;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool GetHeight(out ulong size, out string error)
        {
            size = ulong.MaxValue;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool GetRow(ulong y, out OzAIVector res, out string error)
        {
            res = null;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool GetRows(out OzAIVector[] res, out string error)
        {
            res = null;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool GetSize(out ulong size, out string error)
        {
            size = ulong.MaxValue;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool GetWidth(out ulong size, out string error)
        {
            size = ulong.MaxValue;
            error = "AVX not implemented yet";
            return false;
        }

        public override bool Init(ulong count, ulong width, out string error)
        {
            error = "AVX not implemented yet";
            return false;
        }

        public override bool Init(float[] values, ulong width, out string error)
        {
            error = "AVX not implemented yet";
            return false;
        }

        public override bool Init(OzAIVector[] values, out string error)
        {
            error = "AVX not implemented yet";
            return false;
        }

        public override bool Init(byte[] values, ulong offset, ulong length, ulong width, out string error)
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

        public override bool ToFloats(out float[] res, out string error)
        {
            res = null;
            error = "AVX not implemented yet";
            return false;
        }
    }
}
