using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIHalfMat_CSharp
    {
        public override bool Init(ulong width, ulong height, out string error)
        {
            _width = width;
            _height = height;
            _count = width * height;
            Values = new byte[_count * 2];
            error = null;
            return true;
        }

        public override bool Init(float[] values, ulong width, out string error)
        {
            if (values == null)
            {
                error = $"Could not initialize OzAIHalfMat_CSharp, because no float values provided.";
                return false;
            }

            _count = (ulong)values.LongLength;
            _width = width;
            if (_count % _width != 0)
            {
                error = $"Could not initialize OzAIHalfMat_CSharp, because invalid width given {_width} for the number of floats provided {_count}.";
                return false;
            }
            _height = _count / _width;


            Values = new byte[_count * 2];

            for (ulong i = 0; i < _count; i++)
            {
                var val = values[i];
                var half = (Half)val;
                var bytes = BitConverter.GetBytes(half);

                var byteIdx = i * 2;
                Values[byteIdx] = bytes[0];
                Values[byteIdx + 1] = bytes[1];
            }
            error = null;
            return true;
        }

        public override bool Init(byte[] values, ulong offset, ulong length, ulong width, out string error)
        {
            _count = length;
            _width = width;
            if (_count % _width != 0)
            {
                error = $"Could not initialize OzAIHalfMat_CSharp, because invalid width given {_width} for the number of values specified {_count}.";
                return false;
            }
            _height = _count / _width;

            Values = new byte[_count * 2];
            var srcByteOffset = offset * 2;

            if (!CheckBlockCopy(values, "Bytes", srcByteOffset, 0, (ulong)Values.LongLength, out var needsULong, out error))
            {
                error = "Could not initialize OzAIHalfMat_CSharp, because copying specified data to the 'Values' byte array failed: " + error;
                return false;
            }

            Buffer.BlockCopy(values, (int)srcByteOffset, Values, 0, Values.Length);
            error = null;
            return true;
        }

        public override bool Init(OzAIVector[] rows, out string error)
        {
            if (rows == null)
            {
                error = $"Could not initialize OzAIHalfMat_CSharp, because no rows provided.";
                return false;
            }

            _height = (ulong)rows.LongLength;
            if (_height == 0)
            {
                error = $"Could not initialize OzAIHalfMat_CSharp, because invalid number of rows provided: 0.";
                return false;
            }

            if (!rows[0].GetNumCount(out _width, out error))
            {
                error = $"Could not initialize OzAIHalfMat_CSharp, because failed to get width from first row vector provided: " + error;
                return false;
            }

            _count = _width * _height;
            Values = new byte[_count * 2];

            for (ulong i = 0; i < _height; i++)
            {
                var vec = rows[i];
                var halfVec = vec as OzAIHalfVec;
                if (halfVec == null)
                {
                    error = $"Could not initialize OzAIHalfMat_CSharp, because row vector number {i} specified was not a half vector.";
                    return false;
                }

                if (!vec.ToBytes(out byte[] res, out error))
                {
                    error = $"Could not initialize OzAIHalfMat_CSharp, because could not obtain bytes from row vector number {i}: " + error;
                    return false;
                }
                var byteOffset = i * _width * 2;
                var byteCount = (ulong)res.LongLength;
                if (!CheckBlockCopy(res, "Row vector bytes", 0, byteOffset, byteCount, out var needsULong, out error))
                {
                    error = "Could not initialize OzAIHalfMat_CSharp, because copying specified data to the 'Values' byte array failed: " + error;
                    return false;
                }
                Buffer.BlockCopy(res, 0, Values, (int)byteOffset, (int)byteCount);
            }
            error = null;
            return true;
        }
    }
}
