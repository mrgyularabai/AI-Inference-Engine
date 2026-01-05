using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAIExecManager
    {
        public bool Sum(OzAIVector[] src, OzAIVectorRange dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var ranges, out error))
                return false;
            return Sum(ranges, dst, out error);
        }

        public bool RMS(OzAIScalar epsilon, OzAIScalar part, OzAIVector[] src, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(dst, out var ranges, out error))
                return false;
            return RMS(epsilon, part, src, ranges, out error);
        }

        public bool Swish1(OzAIVector[] src, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var srcRanges, out error))
                return false;
            if (!OzAIVectorRange.ToFull(dst, out var dstRanges, out error))
                return false;
            return Swish1(srcRanges, dstRanges, out error);
        }

        public bool Swish1(OzAIVector[] src, OzAIVectorRange[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var ranges, out error))
                return false;
            return Swish1(ranges, dst, out error);
        }

        public bool SoftMax(OzAIVector src, OzAIVector dst, out string error)
        {
            return SoftMax([src], [dst], out error);
        }

        public bool SoftMax(OzAIVector[] src, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(dst, out var ranges, out error))
                return false;
            return SoftMax(src, ranges, out error);
        }

        public bool RoPE(OzAIVector[] src, OzAIScalar thetaBase, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(dst, out var ranges, out error))
                return false;
            return RoPE(src, thetaBase, ranges, out error);
        }

        public bool RoPE(OzAIVector[] src, OzAIScalar thetaBase, OzAIVectorRange[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var ranges, out error))
                return false;
            return RoPE(ranges, thetaBase, dst, out error);
        }

        public bool Scale(OzAIVector[] src, OzAIScalar scalar, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(dst, out var ranges, out error))
                return false;
            return Scale(src, scalar, ranges, out error);
        }

        public bool Scale(OzAIVector[] src, OzAIScalar scalar, OzAIVectorRange[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var ranges, out error))
                return false;
            return Scale(ranges, scalar, dst, out error);
        }

        public bool Div(OzAIVector src, OzAIScalar scalar, OzAIVector dst, out string error)
        {
            return Div([src], scalar, [dst], out error);
        }

        public bool Div(OzAIVector[] src, OzAIScalar scalar, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(dst, out var ranges, out error))
                return false;
            return Div(src, scalar, ranges, out error);
        }

        public bool Div(OzAIVector[] src, OzAIScalar scalar, OzAIVectorRange[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var ranges, out error))
                return false;
            return Div(ranges, scalar, dst, out error);
        }

        public bool Add(OzAIVector[] src, OzAIVector weights, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var srcRanges, out error))
                return false;
            if (!OzAIVectorRange.ToFullMany(weights, srcRanges.Length, out var wieghtsRanges, out error))
                return false;
            if (!OzAIVectorRange.ToFull(dst, out var dstRanges, out error))
                return false;
            return Add(srcRanges, wieghtsRanges, dstRanges, out error);
        }

        public bool Add(OzAIVector[] src1, OzAIVector[] src2, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(dst, out var ranges, out error))
                return false;
            return Add(src1, src2, ranges, out error);
        }

        public bool Add(OzAIVector[] src1, OzAIVector[] src2, OzAIVectorRange[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src1, out var ranges1, out error))
                return false;
            if (!OzAIVectorRange.ToFull(src2, out var ranges2, out error))
                return false;
            return Add(ranges1, ranges2, dst, out error);
        }

        public bool Had(OzAIVector[] src, OzAIVector weights, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var srcRanges, out error))
                return false;
            if (!OzAIVectorRange.ToFullMany(weights, srcRanges.Length, out var wieghtsRanges, out error))
                return false;
            if (!OzAIVectorRange.ToFull(dst, out var dstRanges, out error))
                return false;
            return Had(srcRanges, wieghtsRanges, dstRanges, out error);
        }

        public bool Had(OzAIVector[] src1, OzAIVector[] src2, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(dst, out var ranges, out error))
                return false;
            return Had(src1, src2, ranges, out error);
        }

        public bool Had(OzAIVector[] src1, OzAIVector[] src2, OzAIVectorRange[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src1, out var ranges1, out error))
                return false;
            if (!OzAIVectorRange.ToFull(src2, out var ranges2, out error))
                return false;
            return Had(ranges1, ranges2, dst, out error);
        }

        public bool MatMul(OzAIVector[] src, OzAIMatrix mat, OzAIVector[] dst, out string error)
        {
            if (!OzAIVectorRange.ToFull(src, out var srcRanges, out error))
                return false;
            if (!OzAIMatrixRange.ToFull(mat, out var matRange, out error))
                return false;
            if (!OzAIVectorRange.ToFull(dst, out var dstRanges, out error))
                return false;
            return MatMul(srcRanges, matRange, dstRanges, out error);
        }
    }
}
