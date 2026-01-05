using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAINum_Int8 : OzAINum_Int
    {

        public sbyte[] Value;

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
            Value[index] = (sbyte)val;
        }

        public override bool FromBytes(byte[] bytes, out string error)
        {
            Value = new sbyte[bytes.LongLength];

            for (long i = 0; i < bytes.LongLength; i++)
            {
                Value[i] = (sbyte)bytes[i];
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
            error = "Int8.FromFloats not implemented yet";
            return false;
        }

        public override bool ToFloats(out float[] res, out string error)
        {
            res = null;
            error = "Int8.ToFloats not implemented yet";
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
            return "i8";
        }

        protected override ulong GetBytesPerBlock()
        {
            return 1;
        }

        protected override bool GetIsQuantized()
        {
            return false;
        }
    }
}
