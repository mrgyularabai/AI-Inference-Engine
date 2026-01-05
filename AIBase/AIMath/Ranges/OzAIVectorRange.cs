using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIVectorRange : OzAITensRange
    {
        public OzAIVector Vector;
        public ulong Offset;
        private ulong _count;
        public virtual ulong Length { get { return _count; } set { _count = value; } }

        public static bool Create(OzAIProcMode mode, ulong count, out OzAIVectorRange res, out string error)
        {
            res = new OzAIVectorRange();
            res.Offset = 0;
            res.Length = count;
            if (!OzAIVector.Create(mode, out res.Vector, out error))
                return false;
            if (!res.Vector.Init(count, out error))
                return false;
            error = null;
            return true;
        }

        public static bool ToVecs(OzAIVectorRange[] vectors, out OzAIVector[] res, out string error)
        {
            res = new OzAIVector[vectors.Length];
            for (int i = 0; i < res.Length; i++)
            {
                var vector = vectors[i];
                res[i] = vector.Vector;
            }
            error = null;
            return true;
        }

        public static bool ToFull(OzAIVector vector, out OzAIVectorRange res, out string error)
        {
            res = null;
            if (!vector.GetNumCount(out var len, out error))
                return false;
            res = new OzAIVectorRange()
            {
                Vector = vector,
                Offset = 0,
                Length = len
            };
            return true;
        }

        public static bool SplitByLen(OzAIVector vector, ulong length, out OzAIVectorRange[] res, out string error)
        {
            res = null;
            if (!vector.GetNumCount(out var count, out error))
                return false;
            if (count % length != 0)
            {
                error = "Could not split by length, because length is not compatible with vector number count.";
                return false;
            }
            var rangeCount = count / length;
            res = new OzAIVectorRange[rangeCount];
            var rangeOffset = 0ul;
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new OzAIVectorRange()
                {
                    Vector = vector,
                    Offset = rangeOffset,
                    Length = length
                };
                rangeOffset += length;
            }
            error = null;
            return true;
        }

        public static bool ToFullMany(OzAIVector vector, int count, out OzAIVectorRange[] res, out string error)
        {
            res = new OzAIVectorRange[count];
            if (!vector.GetNumCount(out var len, out error))
                return false;
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new OzAIVectorRange()
                {
                    Vector = vector,
                    Offset = 0,
                    Length = len
                };
            }
            error = null;
            return true;
        }

        public static bool ToFull(OzAIVector[] vectors, out OzAIVectorRange[] res, out string error)
        {
            res = new OzAIVectorRange[vectors.Length];
            for (int i = 0; i < res.Length; i++)
            {
                var vector = vectors[i];
                if (!vector.GetNumCount(out var len, out error))
                    return false;
                res[i] = new OzAIVectorRange()
                {
                    Vector = vector,
                    Offset = 0,
                    Length = len
                };
            }
            error = null;
            return true;
        }

        public override bool IsValid(out string error)
        {
            if (Vector == null)
            {
                error = $"Vector range is not valid for any operation, since it contains no reference to a vector.";
                return false;
            }
            if (!Vector.GetNumCount(out var count, out error))
            {
                error = $"Could not check whether vector range would be valid for any operation: " + error;
                return false;
            }
            if (Offset + Length > count)
            {
                error = $"Vector range is not valid for any operation, since it is out of bounds for its vector: off:{Offset}, len:{Length}, orignial vec len: {count}";
                return false;
            }
            if (Length == 0)
            {
                error = $"Vector range is not valid for any operation, since it contains no elements.";
                return false;
            }
            error = null;
            return true;
        }

        public OzAIScalar GetNth(ulong index)
        {
            var res = new OzAIScalar();
            res.Offset = Offset + index;
            res.Vector = Vector;
            return res;
        }
    }
}
