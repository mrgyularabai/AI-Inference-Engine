using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAIMatrix
    {
        public abstract OzAINumType GetNumType();
        public abstract bool GetProcMode(out OzAIProcMode res, out string error);

        public static bool Create(OzAIProcMode mode, out OzAIMatrix res, out string error)
        {
            res = null;
            if (mode == null)
            {
                error = "Could not create an OzAIMatrix, because no processing mode provided.";
                return false;
            }
            if (!mode.GetCPUSettings(out var cpu, out error))
            {
                error = "Could not create an OzAIMatrix: " + error;
                return false;
            }
            var type = cpu.DefaultProcType;
            if (!type.CreateMat(mode, out res, out error))
            {
                error = "Could not create an OzAIMatrix: " + error;
                return false;
            }
            return true;
        }

        public virtual bool ToDType(OzAINumType type, out OzAIMatrix res, out string error)
        {
            res = null;
            if (type == OzAINumType.None)
            {
                error = "Could not cast OzAIMatrix to specified Data Type, because no conversion to 'None' exists.";
                return false;
            }
            if (!GetProcMode(out var mode, out error))
            {
                error = "Could not cast OzAIMatrix to specified Data Type: " + error;
                return false;
            }
            if (!type.CreateMat(mode, out res, out error))
            {
                error = "Could not cast OzAIMatrix to specified Data Type: " + error;
                return false;
            }
            if (!ToFloats(out var resFloats, out error))
            {
                error = "Could not cast OzAIMatrix to specified Data Type: " + error;
                return false;
            }
            if (!GetWidth(out var width, out error))
            {
                error = "Could not cast OzAIMatrix to specified Data Type: " + error;
                return false;
            }
            if (!res.Init(resFloats, width, out error))
            {
                error = "Could not cast OzAIMatrix to specified Data Type: " + error;
                return false;
            }
            return true;
        }

        public static bool CheckBlockCopy(Array values, string valuesName, ulong srcByteOffset, ulong dstByteOffset, ulong byteCount, out bool needsULong, out string error)
        {
            if (values == null)
            {
                error = $"Could not copy, because no data to be copied ({values}) was provided.";
                needsULong = false;
                return false;
            }
            if (srcByteOffset + byteCount > (ulong)values.LongLength)
            {
                error = $"{values} could not be copied, because the data range to be copied was out of bounds: off: {srcByteOffset}, len: {byteCount}.";
                needsULong = false;
                return false;
            }
            if (srcByteOffset + byteCount > (ulong)int.MaxValue)
            {
                error = $"{values} could not be copied, because the source values given had an index greater than what an int32 could hold: off: {srcByteOffset}, len: {byteCount}.";
                needsULong = true;
                return false;
            }
            if (dstByteOffset + byteCount > (ulong)int.MaxValue)
            {
                error = $"{values} could not be copied, because the destination values given had an index greater than what an int32 could hold: off: {dstByteOffset}, len: {byteCount}.";
                needsULong = true;
                return false;
            }
            error = null;
            needsULong = false;
            return true;
        }

        public abstract bool GetRow(ulong y, out OzAIVector res, out string error);
        public abstract bool GetRows(out OzAIVector[] res, out string error);

        public abstract bool GetSize(out ulong size, out string error);
        public abstract bool GetNumCount(out ulong count, out string error);
        public abstract bool GetBlockCount(out ulong count, out string error);
        public abstract bool GetBytesPerBlock(out ulong count, out string error);
        public abstract bool GetNumsPerBlock(out ulong size, out string error);
        public abstract bool GetWidth(out ulong count, out string error);
        public abstract bool GetHeight(out ulong count, out string error);

        public abstract bool Init(ulong width, ulong height, out string error);
        public abstract bool Init(byte[] values, ulong offset, ulong length, ulong width, out string error);
        public abstract bool Init(float[] values, ulong width, out string error);
        public abstract bool Init(OzAIVector[] rows, out string error);

        public abstract bool ToBytes(out byte[] res, out string error);
        public abstract bool ToFloats(out float[] res, out string error);
    }
}
