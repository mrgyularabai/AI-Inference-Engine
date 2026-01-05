using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ozeki
{
    partial class OzAICPUExecutor
    {
        bool BulckAdd(OzAIOperation op, out string error)
        {
            var operation = op as OzAIAddition;
            var src1 = operation.Source1;
            var src2 = operation.Source2;
            var dst = operation.Destination;
            for (long i = 0; i < src1.LongLength; i++)
            {
                var src1Range = src1[i];
                var src2Range = src2[i];
                var dstRange = dst[i];
                if (!Add(src1Range, src2Range, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckDiv(OzAIOperation op, out string error)
        {
            var operation = op as OzAIDivide;
            var src = operation.Source;
            var scalar = operation.Scalar;
            var dst = operation.Destination;
            for (long i = 0; i < src.LongLength; i++)
            {
                var srcRange = src[i];
                var dstRange = dst[i];
                if (!Div(srcRange, scalar, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckDot(OzAIOperation op, out string error)
        {
            var operation = op as OzAIDot;
            var src1 = operation.Source1;
            var src2 = operation.Source2;
            var dst = operation.Destination;
            for (long i = 0; i < src1.LongLength; i++)
            {
                var src1Vec = src1[i];
                var src2Vec = src2[i];
                var dstScalar = dst.GetNth((ulong)i);
                if (!Dot(src1Vec, src2Vec, dstScalar, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckHad(OzAIOperation op, out string error)
        {
            var operation = op as OzAIHadamard;
            var src1 = operation.Source1;
            var src2 = operation.Source2;
            var dst = operation.Destination;
            for (long i = 0; i < src1.LongLength; i++)
            {
                var src1Range = src1[i];
                var src2Range = src2[i];
                var dstRange = dst[i];
                if (!Had(src1Range, src2Range, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckMatMul(OzAIOperation op, out string error)
        {
            var operation = op as OzAIMatMul;
            var src = operation.Source;
            var mat = operation.Matrix;
            var dst = operation.Destination;
            for (long i = 0; i < src.LongLength; i++)
            {
                var srcRange = src[i];
                var dstRange = dst[i];
                if (!MatMul(srcRange, mat, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckRMS(OzAIOperation op, out string error)
        {
            var operation = op as OzAIOpRMSNorm;
            var epsilon = operation.Epsilon;
            var part = operation.Part;
            var src = operation.Source;
            var dst = operation.Destination;
            for (long i = 0; i < src.LongLength; i++)
            {
                var srcVec = src[i];
                var dstRange = dst[i];
                if (!RMS(epsilon, part, srcVec, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckRoPE(OzAIOperation op, out string error)
        {
            var operation = op as OzAIRotaryPosEmb;
            var positions = operation.SourcePositions;
            var thetas = operation.ThetaBase;
            var src = operation.Source;
            var dst = operation.Destination;
            for (long i = 0; i < src.LongLength; i++)
            {
                var pos = positions[i];
                var srcVec = src[i];
                var dstRange = dst[i];
                if (!RoPE(pos, thetas, srcVec, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckScale(OzAIOperation op, out string error)
        {
            var operation = op as OzAIScale;
            var src = operation.Source;
            var scalar = operation.Scalar;
            var dst = operation.Destination;
            for (long i = 0; i < src.LongLength; i++)
            {
                var srcRange = src[i];
                var dstRange = dst[i];
                if (!Scale(srcRange, scalar, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckSoftMax(OzAIOperation op, out string error)
        {
            var operation = op as OzAISoftMax;
            var src = operation.Source;
            var dst = operation.Destination;
            for (long i = 0; i < src.LongLength; i++)
            {
                var srcVec = src[i];
                var dstRange = dst[i];
                if (!SoftMax(srcVec, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckSum(OzAIOperation op, out string error)
        {
            var operation = op as OzAISummation;
            var src = operation.Source;
            var dst = operation.Destination;
            for (long i = 0; i < src.LongLength; i++)
            {
                var srcRange = src[i];
                var dstScalar = dst.GetNth((ulong)i);
                if (!Sum(srcRange, dstScalar, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool BulckSwish1(OzAIOperation op, out string error)
        {
            var operation = op as OzAIOpSwish1;
            var src = operation.Source;
            var dst = operation.Destination;
            for (long i = 0; i < src.LongLength; i++)
            {
                var srcRange = src[i];
                var dstRange = dst[i];
                if (!Swish1(srcRange, dstRange, out error))
                    return false;
            }
            error = null;
            return true;
        }

        bool OpMax(OzAIOperation op, out string error)
        {
            var operation = op as OzAIOpMax;
            var src = operation.Source;
            var dst = operation.Destination;
            if (!Max(src, dst, out error))
                return false;
            error = null;
            return true;
        }
    }
}
