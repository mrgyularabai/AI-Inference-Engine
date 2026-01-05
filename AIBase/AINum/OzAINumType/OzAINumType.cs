using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Ozeki.OzGGUF_NumType;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public enum OzAINumType : uint
    {
        None = 0,
        // Ints
        /// <summary>
        /// Same as 'sbyte'. An 8-bit integer.
        /// </summary>
        Int8 = 1,
        /// <summary>
        /// Same as 'short'. A 16-bit integer.
        /// </summary>
        Int16 = 2,
        /// <summary>
        /// Same as 'int'. A 32-bit integer.
        /// </summary>
        Int32 = 3,
        /// <summary>
        /// Same as 'long'. A 64-bit integer.
        /// </summary>
        Int64 = 4,

        // Floats
        /// <summary>
        /// No C# equivalen. Use ushort or byte[2n] to store.
        /// A 16-bit floating point number developped for easy conversion with Float32. 
        /// <para> (1 bit sign, 8 bit exponent, 7 bit significand).
        /// Formula: <br />
        /// If exponent equals all 0s,  -1^sign  *  2^(-126)            *  0.(significand bits) <br />
        /// If exponent equals all 1s,  +/- infinity or NaN if significand is non-zero <br />
        /// Otherwise,                  -1^sign  *  2^(exponent - 127)  *  1.(significand bits) </para>
        /// </summary>
        BrainFloat16 = 5,
        /// <summary>
        /// Same as 'Half'. At the time of writing this can be used with the HalfConverter and is not yet a primitive, but all Maths operations are supported.
        /// A 16-bit floating point number according to the IEE 754 standard's binary16 'half precision' number. 
        /// <para> (1 bit sign, 5 bit exponent, 10 bit significand). 
        /// Formula: <br />
        /// If exponent equals all 0s,  -1^sign  *  2^(-14)            *  0.(significand bits) <br />
        /// If exponent equals all 1s,  +/- infinity or NaN if significand is non-zero <br />
        /// Otherwise,                  -1^sign  *  2^(exponent - 15)  *  1.(significand bits) </para>
        /// </summary>
        Float16 = 6,
        /// <summary>
        /// Same as 'float'.
        /// A 32-bit floating point number according to the IEE 754 standard's binary32 'single precision' number. 
        /// <para> (1 bit sign, 8 bit exponent, 23 bit significand). 
        /// Formula: <br />
        /// If exponent equals all 0s,  -1^sign  *  2^(-126)            *  0.(significand bits) <br />
        /// If exponent equals all 1s,  +/- infinity or NaN if significand is non-zero <br />
        /// Otherwise,                  -1^sign  *  2^(exponent - 127)  *  1.(significand bits) </para>
        /// </summary>
        Float32 = 7,
        /// <summary>
        /// Same as 'double'.
        /// A 64-bit floating point number according to the IEE 754 standard's binary64 'double precision' number. 
        /// <para> (1 bit sign, 11 bit exponent, 52 bit significand). <br />
        /// Formula: <br />
        /// If exponent equals all 0s,  -1^sign  *  2^(-1022)            *  0.(significand bits) <br />
        /// If exponent equals all 1s,  +/- infinity or NaN if significand is non-zero <br />
        /// Otherwise,                  -1^sign  *  2^(exponent - 1023)  *  1.(significand bits)</para>
        /// </summary>
        Float64 = 8,

        // Round-to-nearest Quantization
        Q4_0 = 9,

        // Super Block Round-to-nearest Quantization
        Q4_0_4_4 = 10,
        Q4_0_4_8 = 11,
        Q4_0_8_8 = 12,

        Q4_1 = 13,
        Q5_0 = 14,
        Q5_1 = 15,
        Q8_0 = 16,
        Q8_1 = 17,

        // K-quants
        Q2_K = 18,
        Q3_K = 19,
        Q4_K = 20,
        Q5_K = 21,
        Q6_K = 22,
        Q8_K = 23,

        // Importance Matrix Quantization
        IQ1_S = 24,
        IQ1_M = 25,
        IQ2_XXS = 26,
        IQ2_XS = 27,
        IQ2_S = 28,
        IQ3_XXS = 29,
        IQ3_S = 30,
        IQ4_XS = 31,
        IQ4_NL = 32
    }

    public static partial class NumTypeExtender
    {
        public static OzAINum Create(this OzAINumType self)
        {
            switch (self)
            {
                case OzAINumType.Int8:
                    return new OzAINum_Int8();
                case OzAINumType.Int16:
                    return new OzAINum_Int16();
                case OzAINumType.Int32:
                    return new OzAINum_Int32();
                case OzAINumType.Int64:
                    return new OzAINum_Int64();
                case OzAINumType.BrainFloat16:
                    return new OzAINum_BFloat16();
                case OzAINumType.Float16:
                    return new OzAINum_Float16();
                case OzAINumType.Float32:
                    return new OzAINum_Float32();
                case OzAINumType.Float64:
                    return new OzAINum_Float64();
                case OzAINumType.Q4_0:
                    break;
                case OzAINumType.Q4_0_4_4:
                    break;
                case OzAINumType.Q4_0_4_8:
                    break;
                case OzAINumType.Q4_0_8_8:
                    break;
                case OzAINumType.Q4_1:
                    break;
                case OzAINumType.Q5_0:
                    break;
                case OzAINumType.Q5_1:
                    break;
                case OzAINumType.Q8_0:
                    break;
                case OzAINumType.Q8_1:
                    break;
                case OzAINumType.Q2_K:
                    break;
                case OzAINumType.Q3_K:
                    break;
                case OzAINumType.Q4_K:
                    break;
                case OzAINumType.Q5_K:
                    break;
                case OzAINumType.Q6_K:
                    break;
                case OzAINumType.Q8_K:
                    break;
                case OzAINumType.IQ1_S:
                    break;
                case OzAINumType.IQ1_M:
                    break;
                case OzAINumType.IQ2_XXS:
                    break;
                case OzAINumType.IQ2_XS:
                    break;
                case OzAINumType.IQ2_S:
                    break;
                case OzAINumType.IQ3_XXS:
                    break;
                case OzAINumType.IQ3_S:
                    break;
                case OzAINumType.IQ4_XS:
                    break;
                case OzAINumType.IQ4_NL:
                    break;
                default:
                    break;
            }

            return null;
        }
    }
}
