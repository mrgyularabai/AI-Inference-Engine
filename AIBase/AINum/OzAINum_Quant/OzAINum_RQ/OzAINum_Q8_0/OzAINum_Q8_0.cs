using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Resources;

namespace Ozeki
{
    public class OzAINum_Q8_0 : OzAINum_KQ
    {
        

        public OzAINum_Int8[] Values;
        public OzAINum_Float16 Delta;
        public override bool FromBytes(byte[] res, out string error)
        {
            Values = new OzAINum_Int8[NumsPerBlock];
            for (ulong i = 0; i < NumsPerBlock; i++)
            {
                Values[i] = new OzAINum_Int8();
                if (!Values[i].FromBytes([res[i]], out error)) 
                    return false;
            }

            var deltaBytes = new byte[2];
            Buffer.BlockCopy(res, (int)NumsPerBlock, deltaBytes, 0, 2);
            Delta = new OzAINum_Float16();
            if (!Delta.FromBytes(deltaBytes, out error))
                return false;

            error = null;
            return true;
        }

        public override bool ToBytes(out byte[] res, out string error)
        {
            res = new byte[BytesPerBlock];
            for (ulong i = 0; i < NumsPerBlock; i++)
            {
                //res[i] = (byte)Values[i].Value;
            }
            if (!Delta.ToBytes(out var deltaBytes, out error))
                return false;
            res[BytesPerBlock - 2] = deltaBytes[0];
            res[BytesPerBlock - 1] = deltaBytes[1];
            return true;
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
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < Values.Length; i++)
            {
                var val = Values[i].ToString();
                sb.Append(val);
                sb.Append(", ");
            }
            sb.Remove(sb.Length-2, 2);
            sb.Append("} * ");
            sb.Append(Delta.ToString());
            return sb.ToString();
        }

        protected override ulong GetNumsPerBlock()
        {
            return 32;
        }

        protected override string GetTypeName()
        {
            return "q8_0";
        }

        protected override ulong GetBytesPerBlock()
        {
            return 34;
        }

        protected override bool GetIsQuantized()
        {
            return true;
        }

        //public override bool ToFloats(out float[] res, out string error)
        //{
        //    res = new float[BlockSize];

        //    var delta = new byte[2];
        //    delta[0] = Value[1];
        //    delta[1] = Value[0];

        //    //float16To32Test();
        //    var d = Float16ToFloat32(delta);

        //    for (int x = 2; x < (int)BlockSize; x++)
        //    {
        //        var i = Value[x];
        //        var f = i * d;
        //        res[x - 2] = f;
        //    }

        //    error = null;
        //    return true;
        //}

    //    static float Float16ToFloat32(short i)
    //    {
    //        var bytes = BitConverter.GetBytes(i);
    //        return Float16ToFloat32(bytes);
    //    }

    //    static float Float16ToFloat32(byte[] bytes)
    //    {
    //        // Ensure the byte array has exactly 2 elements
    //        if (bytes.Length != 2)
    //            throw new ArgumentException("Input must be exactly 2 bytes.");

    //        // Construct the 16-bit integer from the byte array
    //        ushort half = BitConverter.ToUInt16(bytes, 0);

    //        // Extract sign, exponent, and mantissa from the half-precision float
    //        int sign = (half >> 15) & 0x0001;
    //        int exponent = (half >> 10) & 0x001F;
    //        int mantissa = half & 0x03FF; //10 bites mask 

    //        // Convert to single-precision float (32-bit)
    //        int sign32 = sign << 31;
    //        int exponent32 = (exponent + 112) << 23;
    //        int mantissa32 = mantissa << 13;

    //        int floatBits = sign32 | exponent32 | mantissa32;

    //        // Convert the bit pattern to a float
    //        return BitConverter.ToSingle(BitConverter.GetBytes(floatBits), 0);
    //    }

    //    static float BFloat16ToFloat32(byte[] bfloat16Bytes)
    //    {
    //        // Ensure the byte array has exactly 2 elements
    //        if (bfloat16Bytes.Length != 2)
    //            throw new ArgumentException("Input must be exactly 2 bytes.");

    //        // Construct the 16-bit integer from the byte array
    //        ushort bfloat16 = BitConverter.ToUInt16(bfloat16Bytes, 0);

    //        // Extract the sign, exponent, and mantissa
    //        int sign = (bfloat16 >> 15) & 0x0001;
    //        int exponent = (bfloat16 >> 7) & 0x00FF; //8 bites mask
    //        int mantissa = bfloat16 & 0x007F; //7 bites mask

    //        // Convert to single-precision float (32-bit)
    //        int sign32 = sign << 31;
    //        int exponent32 = (exponent + 112) << 23; //23=7+16 //ebben nem vagyok biztos
    //        int mantissa32 = mantissa << 16; //ebben nem vagyok biztos

    //        // Combine sign, exponent, and mantissa
    //        int floatBits = sign32 | exponent32 | mantissa32;

    //        // Convert the bit pattern to a float
    //        return BitConverter.ToSingle(BitConverter.GetBytes(floatBits), 0);
    //    }

    //    static void float16To32Test()
    //    {
    //        //Megj.: Azt nem értem, hogy miért pontosabb a gerganov convertere

    //        //GGerganov GGML_FP16_TO_FP32
    //        //Int value: 5338
    //        //Hex bytes: 14 DA
    //        //Dec bytes: 20 218
    //        //Float16 to Float32 után:
    //        //0.00118446350

    //        var b = new byte[2];
    //        b[0] = 218; //fordított a sorrend
    //        b[1] = 20;

    //        var f1 = Float16ToFloat32(b);
    //        var f2 = BFloat16ToFloat32(b);

    //        //Int value: 4410
    //        //Hex bytes: 11 3A
    //        //Dec bytes: 17 58
    //        //0.000638008118

    //        var b2 = new byte[2];
    //        b2[0] = 58; //fordított a sorrend
    //        b2[1] = 17;

    //        var f3 = Float16ToFloat32(b2);
    //        var f4 = BFloat16ToFloat32(b2);

    //        //Gerganov: (int)377 -> (float)4.13060188e-05
    //        //Ozeki: Float16ToFloat32(377) = 4.17530537E-05
    //        //Miért tér el?

    //        //Gerganov: 790->4.70876694e-05
    //        //Ozeki: Float16ToFloat32(790) = 5.40614128E-05
    //        //
    //    }

    }

    //class block_q8_0
    //{
    //    public float d; //delta
    //    public float s; // d * sum(qs[i])
    //    public List<int> qs; //quants
    //}
}

//Q8_0 esetén
//quantizedBlock[34]
//blocksize: 32
//typesize: 34



/**
 * Példa: 
 * 3.14 × 10^2
 * 3.14 a mantissa
 * 2 az expontent
 * 
 * 
 * Converts brain16 to float32.
 *
 * The bfloat16 floating point format has the following structure:
 *
 *       ┌sign
 *       │
 *       │   ┌exponent (8 bit)
 *       │   │
 *       │   │      ┌mantissa (7 bit)
 *       │   │      │
 *       │┌──┴───┐┌─┴───┐
 *     0b0000000000000000 brain16
 *
 * Since bf16 has the same number of exponent bits as a 32bit float,
 * encoding and decoding numbers becomes relatively straightforward.
 *
 *       ┌sign
 *       │
 *       │   ┌exponent(8 bit)
 *       │   │
 *       │   │      ┌mantissa (23 bit)
 *       │   │      │
 *       │┌──┴───┐┌─┴───────────────────┐
 *     0b00000000000000000000000000000000 IEEE binary32
 *
 * For comparison, the standard fp16 format has fewer exponent bits.
 *
 *       ┌sign
 *       │
 *       │  ┌exponent (5 bit)
 *       │  │
 *       │  │    ┌mantissa (10 bit)
 *       │  │    │
 *       │┌─┴─┐┌─┴──────┐
 *     0b0000000000000000 IEEE binary16
 *
 * @see IEEE 754-2008
 */