using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAINum_Float64 : OzAINum_Float
    {

        public double[] Value;

        protected override ulong GetCount()
        {
            return (ulong)Value.LongLength;
        }

        protected override float GetNumber(ulong index)
        {
            return (float)Value[index];
        }

        protected override void SetNumber(ulong index, float val)
        {
            Value[index] = val;
        }

        public override bool FromBytes(byte[] bytes, out string error)
        {
            float fcount = bytes.LongLength / (float)BytesPerBlock;
            if (fcount % 1 != 0)
            {
                error = "Could not convert bytes to OzAINum_Float64, because invalid number of bytes given";
                return false;
            }

            ulong count = (ulong)bytes.LongLength / BytesPerBlock;
            Value = new double[count];

            var byteCount = 0ul;
            for (ulong i = 0; i < count; i++)
            {
                Value[i] = BitConverter.ToDouble(bytes, (int)byteCount);
                byteCount += BytesPerBlock;
            }
            error = null;
            return true;
        }

        public override bool ToBytes(out byte[] res, out string error)
        {
            error = null;
            res = null;
            return true;
        }


        public override bool FromFloats(float[] res, out string error)
        {
            res = null;
            error = "Float64.FromFloats not implemented yet";
            return false;
        }

        public override bool ToFloats(out float[] res, out string error)
        {
            res = null;
            error = "Float64.ToFloats not implemented yet";
            return false;
        }


        public override string ToString()
        {
            return Value.ToString();
        }

        protected override ulong GetNumsPerBlock()
        {
            return 1;
        }

        protected override string GetTypeName()
        {
            return "f64";
        }

        protected override ulong GetBytesPerBlock()
        {
            return 8;
        }

        protected override bool GetIsQuantized()
        {
            return false;
        }
    }
}
