using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAINum
    {
        /// <summary>
        /// Initializes the OzAINum instance with a (bytes / BytesPerBlock) * BlockSize amount of floats, using the raw data provided.
        /// </summary>
        /// <param name="bytes"> The raw data. </param>
        /// <param name="error"> The error message if the function returns false. </param>
        /// <returns> True if the function succeeded. </returns>
        public abstract bool FromBytes(byte[] bytes, out string error);

        /// <summary>
        /// Converts the format used to store the floats in OzAINum into a byte array.
        /// </summary>
        /// <param name="res"> The output. </param>
        /// <param name="error"> The error message if the function returns false. </param>
        /// <returns> True if the function succeeded. </returns>
        public abstract bool ToBytes(out byte[] res, out string error);


        /// <summary>
        /// Initializes the OzAINum instance with a the floats provided, casting/quantizing if necessary.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="error"> The error message if the function returns false. </param>
        /// <returns> True if the function succeeded. </returns>
        public abstract bool FromFloats(float[] res, out string error);

        /// <summary>
        /// Returns all the float stored in the OzAINum instance, casting/dequantizing if necessary.
        /// </summary>
        /// <param name="res"> The output. </param>
        /// <param name="error"> The error message if the function returns false. </param>
        /// <returns> True if the function succeeded. </returns>
        public abstract bool ToFloats(out float[] res, out string error);

        public ulong CalcSize(ulong count)
        {
            return (count / NumsPerBlock) * BytesPerBlock;
        }

        protected abstract float GetNumber(ulong index);
        protected abstract void SetNumber(ulong index, float val);


        protected abstract ulong GetCount();

        protected abstract ulong GetBlockCount();

        /// <summary>
        /// The GGUF Name of this OzAINum's type.
        /// </summary>
        public string TypeName
        {
            get
            {
                return GetTypeName();
            }
        }
        protected abstract string GetTypeName();

        /// <summary>
        /// The number of floats that can be extracted from a single block in this type of OzAINum.
        /// </summary>
        public ulong NumsPerBlock
        {
            get
            {
                return GetNumsPerBlock();
            }
        }
        protected abstract ulong GetNumsPerBlock();

        /// <summary>
        /// The number of bytes required to represent a single block in this type of OzAINum.
        /// </summary>
        public ulong BytesPerBlock
        {
            get
            {
                return GetBytesPerBlock();
            }
        }
        protected abstract ulong GetBytesPerBlock();

        /// <summary>
        /// Whether a quantization algorithm is applied in this type of OzAINum to store the floats.
        /// </summary>
        public bool IsQuantized
        {
            get
            {
                return GetIsQuantized();
            }
        }
        protected abstract bool GetIsQuantized();
    }
}