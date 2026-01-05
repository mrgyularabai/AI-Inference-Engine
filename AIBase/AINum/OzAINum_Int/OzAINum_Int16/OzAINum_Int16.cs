using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAINum_Int16 : OzAINum_Int
    {
        public short[] Value;

        protected override ulong GetCount()
        {
            return (ulong)Value.LongLength;
        }

        protected override float GetNumber(ulong index)
        {
            return Value[index];
        }

        protected override void SetNumber(ulong index, float val)
        {
            Value[index] = (short)val;
        }

        public override bool FromBytes(byte[] bytes, out string error)
        {
            float fcount = bytes.LongLength / (float)BytesPerBlock;
            if (fcount % 1 != 0)
            {
                error = "Could not convert bytes to OzAINum_Int16, because invalid number of bytes given";
                return false;
            }

            ulong count = (ulong)bytes.LongLength / BytesPerBlock;
            Value = new short[count];

            var byteCount = 0ul;
            for (ulong i = 0; i < count; i++)
            {
                Value[i] = BitConverter.ToInt16(bytes, (int)byteCount);
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
            error = "Int16.FromFloats not implemented yet";
            return false;
        }

        public override bool ToFloats(out float[] res, out string error)
        {
            res = null;
            error = "Int16.ToFloats not implemented yet";
            return false;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override string GetTypeName()
        {
            return "I16";
        }

        protected override ulong GetBytesPerBlock()
        {
            return 2;
        }

    }
}
