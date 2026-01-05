using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAINum_BFloat16 : OzAINum_Float
    {
        public byte[] Value;

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
            Value[index] = byte.MaxValue;
        }

        public override bool FromBytes(byte[] bytes, out string error)
        {
            Value = bytes;
            error = null;
            return true;
        }

        public override bool ToBytes(out byte[] res, out string error)
        {
            res = Value;
            error = null;
            return true;
        }

        public override bool FromFloats(float[] res, out string error)
        {
            res = null;
            error = "BFloat16.FromFloats not implemented yet";
            return false;
        }

        public override bool ToFloats(out float[] res, out string error)
        {
            res = null;
            error = "BFloat16.ToFloats not implemented yet";
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
            return "bf16";
        }

        protected override ulong GetBytesPerBlock()
        {
            return 2;
        }

        protected override bool GetIsQuantized()
        {
            return false;
        }
    }
}
