using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIExecManager
    {

        bool awaitAllExecs(List<OzAIExecutor> execs, out string error)
        {
            foreach (var item in execs)
            {
                if (!item.AwaitAll(out error))
                    return false;
            }
            error = null;
            return true;
        }

        public bool Add(OzAIVectorRange[] src1, OzAIVectorRange[] src2, OzAIVectorRange[] dst, out string error)
        {
            if (!checkBinaryOpInps(src1, src2, dst, out error))
                return false;

            getOperandCounts(src1.Length, out var mainCount, out var normalCount);

            var operation = new OzAIAddition()
            {
                Source1 = src1[..(int)mainCount],
                Source2 = src2[..(int)mainCount],
                Destination = dst[..(int)mainCount]
            };

            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(src1, out var src1Vecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(src2, out var src2Vecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. src1Vecs, .. src2Vecs, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount; 
                operation = new OzAIAddition()
                {
                    Source1 = src1[(int)oldOffset..(int)currentOffset],
                    Source2 = src2[(int)oldOffset..(int)currentOffset],
                    Destination = dst[(int)oldOffset..(int)currentOffset]
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }
        public bool Div(OzAIVectorRange[] src, OzAIScalar scalar, OzAIVectorRange[] dst, out string error)
        {
            if (!checkUnaryOpInps(src, dst, out error)) 
                return false;

            getOperandCounts(src.Length, out var mainCount, out var normalCount);

            var operation = new OzAIDivide()
            {
                Scalar = scalar,
                Source = src[..(int)mainCount],
                Destination = dst[..(int)mainCount]
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(src, out var srcVecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. srcVecs, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount; 
                operation = new OzAIDivide()
                {
                    Scalar = scalar,
                    Source = src[(int)oldOffset..(int)currentOffset],
                    Destination = dst[(int)oldOffset..(int)currentOffset]
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }
        public bool Sum(OzAIVectorRange[] src, OzAIVectorRange dst, out string error)
        {
            if (!checkUnaryOpInps(src, dst, out error))
                return false;

            getOperandCounts(src.Length, out var mainCount, out var normalCount);

            var dstRange = new OzAIVectorRange()
            {
                Offset = 0,
                Length = mainCount,
                Vector = dst.Vector
            };
            var operation = new OzAISummation()
            {
                Source = src[..(int)mainCount],
                Destination = dstRange
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(src, out var srcVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. srcVecs, dst.Vector];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount;
                dstRange = new OzAIVectorRange()
                {
                    Offset = oldOffset,
                    Length = normalCount,
                    Vector = dst.Vector
                };
                operation = new OzAISummation()
                {
                    Source = src[(int)oldOffset..(int)currentOffset],
                    Destination = dstRange
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }

        public bool RMS(OzAIScalar epsilon, OzAIScalar part, OzAIVector[] src, OzAIVectorRange[] dst, out string error)
        {
            if (!checkUnaryOpInps(src, dst, out error))
                return false;

            getOperandCounts(src.Length, out var mainCount, out var normalCount);

            var operation = new OzAIOpRMSNorm()
            {
                Part = part,
                Epsilon = epsilon,
                Source = src[..(int)mainCount],
                Destination = dst[..(int)mainCount]
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. src, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount;
                var execOperation = new OzAIOpRMSNorm()
                {
                    Part = part,
                    Epsilon = epsilon,
                    Source = src[..(int)mainCount],
                    Destination = dst[..(int)mainCount]
                };
                execOperation.Source = src[(int)oldOffset..(int)currentOffset];
                execOperation.Destination = dst[(int)oldOffset..(int)currentOffset];
                if (_mode.DoChecks && !execOperation.IsPossible(out error))
                    return false;
                execs[i].Add(execOperation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }

        public bool Swish1(OzAIVectorRange[] src, OzAIVectorRange[] dst, out string error)
        {
            if (!checkUnaryOpInps(src, dst, out error))
                return false;

            getOperandCounts(src.Length, out var mainCount, out var normalCount);

            var operation = new OzAIOpSwish1()
            {
                Source = src[..(int)mainCount],
                Destination = dst[..(int)mainCount]
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(src, out var srcVecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. srcVecs, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount; 
                operation = new OzAIOpSwish1()
                {
                    Source = src[(int)oldOffset..(int)currentOffset],
                    Destination = dst[(int)oldOffset..(int)currentOffset]
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }

        public bool SoftMax(OzAIVector[] src, OzAIVectorRange[] dst, out string error)
        {
            if (!checkUnaryOpInps(src, dst, out error))
                return false;

            getOperandCounts(src.Length, out var mainCount, out var normalCount);

            var operation = new OzAISoftMax()
            {
                Source = src[..(int)mainCount],
                Destination = dst[..(int)mainCount]
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. src, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount;
                operation = new OzAISoftMax()
                {
                    Source = src[(int)oldOffset..(int)currentOffset],
                    Destination = dst[(int)oldOffset..(int)currentOffset]
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;
            return true;
        }

        public bool RoPE(OzAIVectorRange[] src, OzAIScalar thetaBase, OzAIVectorRange[] dst, out string error)
        {
            if (!checkUnaryOpInps(src, dst, out error))
                return false;

            getOperandCounts(src.Length, out var mainCount, out var normalCount);
            ulong[] positions = new ulong[mainCount];
            for (ulong i = 0; i < mainCount; i++)
            {
                positions[i] = (ulong)i;
            }

            var operation = new OzAIRotaryPosEmb()
            {
                ThetaBase = thetaBase,
                Source = src[..(int)mainCount],
                SourcePositions = positions,
                Destination = dst[..(int)mainCount]
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(src, out var srcVecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. srcVecs, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount;
                positions = new ulong[normalCount];
                for (ulong j = 0; j < normalCount; j++)
                {
                    positions[i] = oldOffset + (ulong)i;
                }
                operation = new OzAIRotaryPosEmb()
                {
                    ThetaBase = thetaBase,
                    Source = src[(int)oldOffset..(int)currentOffset],
                    SourcePositions = positions,
                    Destination = dst[(int)oldOffset..(int)currentOffset]
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }

        public bool Scale(OzAIVectorRange[] src, OzAIScalar scalar, OzAIVectorRange[] dst, out string error)
        {
            if (!checkUnaryOpInps(src, dst, out error))
                return false;

            getOperandCounts(src.Length, out var mainCount, out var normalCount);

            var operation = new OzAIScale()
            {
                Scalar = scalar,
                Source = src[..(int)mainCount],
                Destination = dst[..(int)mainCount]
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(src, out var srcVecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. srcVecs, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount; 
                operation = new OzAIScale()
                {
                    Scalar = scalar,
                    Source = src[(int)oldOffset..(int)currentOffset],
                    Destination = dst[(int)oldOffset..(int)currentOffset]
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }

        

        public bool Had(OzAIVectorRange[] src1, OzAIVectorRange[] src2, OzAIVectorRange[] dst, out string error)
        {
            if (!checkBinaryOpInps(src1, src2, dst, out error))
                return false;

            getOperandCounts(src1.Length, out var mainCount, out var normalCount);

            var operation = new OzAIHadamard()
            {
                Source1 = src1[..(int)mainCount],
                Source2 = src2[..(int)mainCount],
                Destination = dst[..(int)mainCount]
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(src1, out var src1Vecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(src2, out var src2Vecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. src1Vecs, .. src2Vecs, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount; 
                operation = new OzAIHadamard()
                {
                    Source1 = src1[(int)oldOffset..(int)currentOffset],
                    Source2 = src2[(int)oldOffset..(int)currentOffset],
                    Destination = dst[(int)oldOffset..(int)currentOffset]
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }

        public bool Dot(OzAIVector[] src1, OzAIVector[] src2, OzAIVectorRange dst, out string error)
        {
            if (!checkBinaryOpInps(src1, src2, dst, out error))
                return false;

            getOperandCounts(src1.Length, out var mainCount, out var normalCount);

            var dstRange = new OzAIVectorRange()
            {
                Offset = 0,
                Length = mainCount,
                Vector = dst.Vector
            };
            var operation = new OzAIDot()
            {
                Source1 = src1[..(int)mainCount],
                Source2 = src2[..(int)mainCount],
                Destination = dstRange
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            List<OzAIVector> vecs = [.. src1, .. src2, dst.Vector];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount;
                dstRange = new OzAIVectorRange()
                {
                    Offset = oldOffset,
                    Length = normalCount,
                    Vector = dst.Vector
                };
                operation = new OzAIDot()
                {
                    Source1 = src1[(int)oldOffset..(int)currentOffset],
                    Source2 = src2[(int)oldOffset..(int)currentOffset],
                    Destination = dstRange
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;

            return true;
        }

        public bool MatMul(OzAIVectorRange[] src, OzAIMatrixRange mat, OzAIVectorRange[] dst, out string error)
        {
            List<object> objs = [src, dst, mat];
            List<string> name = ["Source", "Destination", "Matrix"];
            if (!OzAICheckable.CheckIfNull(objs, name, out error))
            {
                error = "MatMul not possible: " + error;
                return false;
            }

            if (src.LongLength != dst.LongLength)
            {
                error = "MatMul not possible, because the number of sources given does not match the number of destinations. ";
                return false;
            }

            getOperandCounts(src.Length, out var mainCount, out var normalCount);

            var operation = new OzAIMatMul()
            {
                Matrix = mat,
                Source = src[..(int)mainCount],
                Destination = dst[..(int)mainCount]
            };
            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            if (!OzAIVectorRange.ToVecs(src, out var srcVecs, out error))
                return false;
            if (!OzAIVectorRange.ToVecs(dst, out var dstVecs, out error))
                return false;
            List<OzAIVector> vecs = [.. srcVecs, .. dstVecs];
            if (!getMain(vecs, out var main, out error))
                return false;
            main.Add(operation);
            if (!getExecs(vecs, out var execs, out error))
                return false;

            var oldOffset = mainCount;
            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                var currentOffset = oldOffset + normalCount; 
                operation = new OzAIMatMul()
                {
                    Matrix = mat,
                    Source = src[(int)oldOffset..(int)currentOffset],
                    Destination = dst[(int)oldOffset..(int)currentOffset]
                };
                if (_mode.DoChecks && !operation.IsPossible(out error))
                    return false;
                execs[i].Add(operation);
                oldOffset = currentOffset;
            }

            if (!awaitAllExecs(execs, out error))
                return false;
            return true;
        }

        public bool Max(OzAIVectorRange src, OzAIScalar dst, out string error)
        {
            if (src == null)
            {
                error = "MatMul not possible, because no source vectors provided.";
                return false;
            }
            if (dst == null)
            {
                error = "MatMul not possible, because no destination provided.";
                return false;
            }

            var operation = new OzAIOpMax()
            {
                Source = src,
                Destination = dst
            };

            if (_mode.DoChecks && !operation.IsPossible(out error))
                return false;

            List<OzAIVector> vecs = [src.Vector, dst.Vector];
            if (!getMain(vecs, out var main, out error, false))
                return false;
            main.Add(operation);

            if (!main.AwaitAll(out error))
                return false;

            return true;
        }
    }
}
