using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAINum_IQ1_M : OzAINum_IQ
    {
        public byte[] Value;

        public override bool FromBytes(byte[] res, out string error)
        {
            error = $"{GetTypeName()}.ToBytes not implemented yet";
            return false;
        }

        public override bool ToBytes(out byte[] res, out string error)
        {
            res = null;
            error = $"{GetTypeName()}.ToBytes not implemented yet";
            return false;
        }

        public override bool FromFloats(float[] res, out string error)
        {
            error = $"{GetTypeName()}.FromFloats not implemented yet";
            return false;
        }

        public override bool ToFloats(out float[] res, out string error)
        {
            res = null;
            error = $"{GetTypeName()}.ToFloats not implemented yet";
            return false;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override ulong GetNumsPerBlock()
        {
            return 256;
        }

        protected override string GetTypeName()
        {
            return "iq1_m";
        }

        protected override ulong GetBytesPerBlock()
        {
            return 56;
        }

        protected override bool GetIsQuantized()
        {
            return true;
        }
    }
}
