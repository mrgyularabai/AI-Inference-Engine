using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract partial class OzAIVector
    {
        public abstract OzAINumType GetNumType();
        public abstract bool GetProcMode(out OzAIProcMode res, out string error);

        public static bool Create(OzAIProcMode mode, out OzAIVector res, out string error)
        {
            res = null;
            if (mode == null)
            {
                error = "Could not create an OzAIVector, because no processing mode provided.";
                return false;
            }
            if (!mode.GetCPUSettings(out var cpu, out error))
            {
                error = "Could not create an OzAIVector: " + error;
                return false;
            }
            var type = cpu.DefaultProcType;
            if (!type.CreateVec(mode, out res, out error))
            {
                error = "Could not create an OzAIVector: " + error;
                return false;
            }
            return true;
        }

        public virtual bool ToDType(OzAINumType type, out OzAIVector res, out string error)
        {
            res = null;
            if (type == OzAINumType.None)
            {
                error = "Could not cast OzAIVector to specified Data Type, because no conversion to 'None' exists.";
                return false;
            }
            if (!GetProcMode(out var mode, out error))
            {
                error = "Could not cast OzAIVector to specified Data Type: " + error;
                return false;
            }
            if (!type.CreateVec(mode, out res, out error))
            {
                error = "Could not cast OzAIVector to specified Data Type: " + error;
                return false;
            }
            if (!ToFloat(out var resFloats, out error))
            {
                error = "Could not cast OzAIVector to specified Data Type: " + error;
                return false;
            }
            if (!res.Init(resFloats, 0, (ulong)resFloats.LongLength, out error))
            {
                error = "Could not cast OzAIVector to specified Data Type: " + error;
                return false;
            }
            return true;
        }

        // Discription
        public abstract bool GetSize(out ulong size, out string error);
        public abstract bool GetNumCount(out ulong size, out string error);
        public abstract bool GetBlockCount(out ulong size, out string error);
        public abstract bool GetBytesPerBlock(out ulong size, out string error);
        public abstract bool GetNumsPerBlock(out ulong size, out string error);

        // Initialization
        public abstract bool Init(ulong length, out string error);
        public abstract bool Init(byte[] data, ulong byteOffset, ulong byteCount, out string error);
        public abstract bool Init(float[] data, ulong offset, ulong length, out string error);
        public abstract bool ToBytes(out byte[] res, out string error);
        public abstract bool ToFloat(out float[] res, out string error);

        // Operations
        public abstract bool GetNth(ulong index, out float res, out string error);
        public abstract bool Clone(out OzAIVector res, out string error);
    }
}
