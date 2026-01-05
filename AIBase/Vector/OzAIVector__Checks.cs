using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIVector
    {
        public static bool CheckBlockCopy(Array values, string valuesName, ulong srcOffset, ulong dstOffset, ulong byteCount, out bool needsULong, out string error)
        {
            if (values == null)
            {
                error = $"Could not copy, because no data to be copied ({values}) was provided.";
                needsULong = false;
                return false;
            }
            if (srcOffset + byteCount > (ulong)values.LongLength)
            {
                error = $"{values} could not be copied, because the data range to be copied was out of bounds: off: {srcOffset}, len: {byteCount}, but max len {values.LongLength}.";
                needsULong = false;
                return false;
            }
            if (srcOffset + byteCount > (ulong)int.MaxValue)
            {
                error = $"{values} could not be copied, because the source values given had an index greater than what an int32 could hold: off: {srcOffset}, len: {byteCount}, but max len {values.LongLength}.";
                needsULong = true;
                return false;
            }
            if (dstOffset + byteCount > (ulong)int.MaxValue)
            {
                error = $"{values} could not be copied, because the destination values given had an index greater than what an int32 could hold: off: {dstOffset}, len: {byteCount}, but max len {values.LongLength}.";
                needsULong = true;
                return false;
            }
            error = null;
            needsULong = false;
            return true;
        }
    }
}
