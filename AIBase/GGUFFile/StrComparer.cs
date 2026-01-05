using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzMDStrComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] left, byte[] right)
        {
            if (left == null || right == null)
            {
                return true;
            }
            if (left.Length != right.Length)
            {
                return false;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(byte[] key)
        {
            var res = new byte[4];
            int len = Math.Min(key.Length, 4);
            int offset = Math.Max(key.Length - 5, 0);
            Buffer.BlockCopy(key, offset, res, 0, len);
            return BitConverter.ToInt32(res, 0);
        }
    }
}
