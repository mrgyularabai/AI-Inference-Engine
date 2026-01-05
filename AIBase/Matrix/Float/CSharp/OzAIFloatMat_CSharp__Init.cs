using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIFloatMat_CSharp
    {
        public override bool Init(ulong width, ulong height, out string error)
        {
            _width = width;
            _height = height;
            _count = width * height;
            Values = new float[_count];
            error = null;
            return true;
        }

        public override bool Init(float[] values, ulong width, out string error)
        {
            _count = (ulong)values.LongLength;
            _width = width;
            if (_count % _width != 0)
            {
                error = $"Could not initialize OzAIFloatMat_CSharp, because invalid width given {_width} for the number of values specified {_count}.";
                return false;
            }
            _height = _count / _width;

            Values = new float[_count];
            var byteCount = (ulong)Values.LongLength * 4;

            if (!CheckBlockCopy(values, "Bytes", 0, 0, byteCount, out var needsULong, out error))
            {
                error = "Could not initialize OzAIFloatMat_CSharp, because copying specified data to the 'Values' byte array failed: " + error;
                return false;
            }

            Buffer.BlockCopy(values, 0, Values, 0, (int)byteCount);
            error = null;
            return true;
        }

        public override bool Init(byte[] values, ulong offset, ulong length, ulong width, out string error)
        {
            _count = length;
            _width = width;
            if (_count % _width != 0)
            {
                error = $"Could not initialize OzAIFloatMat_CSharp, because invalid width given {_width} for the number of values specified {_count}.";
                return false;
            }
            _height = _count / _width;

            Values = new float[_count];
            var srcByteOffset = offset * 4;
            var byteCount = (ulong)Values.LongLength * 4;

            if (!CheckBlockCopy(values, "Bytes", srcByteOffset, 0, byteCount, out var needsULong, out error))
            {
                error = "Could not initialize OzAIFloatMat_CSharp, because copying specified data to the 'Values' byte array failed: " + error;
                return false;
            }

            Buffer.BlockCopy(values, (int)srcByteOffset, Values, 0, (int)byteCount);
            error = null;
            return true;
        }

        public override bool Init(OzAIVector[] rows, out string error)
        {
            if (rows == null)
            {
                error = $"Could not initialize OzAIFloatMat_CSharp, because no rows provided.";
                return false;
            }

            _height = (ulong)rows.LongLength;
            if (_height == 0)
            {
                error = $"Could not initialize OzAIFloatMat_CSharp, because invalid number of rows provided: 0.";
                return false;
            }

            if (!rows[0].GetNumCount(out _width, out error))
            {
                error = $"Could not initialize OzAIFloatMat_CSharp, because failed to get width from first row vector provided: " + error;
                return false;
            }

            _count = _width * _height;
            Values = new float[_count];

            for (ulong i = 0; i < _height; i++)
            {
                var vec = rows[i];
                var halfVec = vec as OzAIFloatVec;
                if (halfVec == null)
                {
                    error = $"Could not initialize OzAIFloatMat_CSharp, because row vector number {i} specified was not a float vector.";
                    return false;
                }

                if (!vec.ToFloat(out float[] res, out error))
                {
                    error = $"Could not initialize OzAIFloatMat_CSharp, because could not obtain floats from row vector number {i}: " + error;
                    return false;
                }
                var byteOffset = i * _width * 4;
                var byteCount = (ulong)res.LongLength;
                if (!CheckBlockCopy(res, "Row vector bytes", 0, byteOffset, byteCount, out var needsULong, out error))
                {
                    error = "Could not initialize OzAIFloatMat_CSharp, because copying specified data to the 'Values' float array failed: " + error;
                    return false;
                }
                Buffer.BlockCopy(res, 0, Values, (int)byteOffset, (int)byteCount);
            }
            error = null;
            return true;
        }
    }
}
