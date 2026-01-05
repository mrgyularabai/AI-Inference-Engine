using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIHalfCPUExec : OzAICPUExecutor
    {
        public override bool Add(OzAIVectorRange src1, OzAIVectorRange src2, OzAIVectorRange dst, out string error)
        {
            try
            {
                var src1Vec = src1.Vector as OzAIHalfVec_CSharp;
                var src2Vec = src2.Vector as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;
                var src1Offset = src1.Offset;
                var src2Offset = src2.Offset;
                var dstOffset = dst.Offset;
                var count = src1.Length;
                for (ulong i = 0; i < count; i++)
                {
                    dstVec.Values[dstOffset++] = src1Vec.Values[src1Offset++] + src2Vec.Values[src2Offset++];
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform addition: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void AddF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override void AddH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool Div(OzAIVectorRange src, OzAIScalar scalar, OzAIVectorRange dst, out string error)
        {
            try
            {
                var scalarVec = scalar.Vector as OzAIHalfVec_CSharp;
                var scalarVal = scalarVec.Values[scalar.Offset];
                var srcVec = src.Vector as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;
                var srcOffset = src.Offset;
                var dstOffset = dst.Offset;
                var count = src.Length;
                for (ulong i = 0; i < count; i++)
                {
                    dstVec.Values[dstOffset++] = srcVec.Values[srcOffset++] / scalarVal;
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform division: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void DivF(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override void DivH(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool Dot(OzAIVector src1, OzAIVector src2, OzAIScalar dst, out string error)
        {
            try
            {
                var src1Vec = src1 as OzAIHalfVec_CSharp;
                var src2Vec = src2 as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;
                if (!src1.GetNumCount(out var count, out error))
                {
                    error = "Failed to perform dot product: " + error;
                    return false;
                }
                for (ulong i = 0; i < count; i++)
                {
                    dstVec.Values[dst.Offset] += src1Vec.Values[i] * src2Vec.Values[i];
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform dot product: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void DotF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override void DotH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool Had(OzAIVectorRange src1, OzAIVectorRange src2, OzAIVectorRange dst, out string error)
        {
            try
            {
                var src1Vec = src1.Vector as OzAIHalfVec_CSharp;
                var src2Vec = src2.Vector as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;
                var src1Offset = src1.Offset;
                var src2Offset = src2.Offset;
                var dstOffset = dst.Offset;
                var count = src1.Length;
                for (ulong i = 0; i < count; i++)
                {
                    dstVec.Values[dstOffset++] = src1Vec.Values[src1Offset++] * src2Vec.Values[src2Offset++];
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform hadamard product: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void HadF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override void HadH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool MatMul(OzAIVectorRange src, OzAIMatrixRange mat, OzAIVectorRange dst, out string error)
        {
            try
            {
                var matrix = mat.Matrix as OzAIHalfMat_CSharp;
                var srcVec = src.Vector as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;

                var yOffset = mat.StartCoords.Item2;
                var rowOffset = mat.StartCoords.Item1;
                var srcOffset = src.Offset;
                var dstOffset = dst.Offset;
                var resNums = new Half[dst.Length];

                var width = mat.Counts.Item1;
                var height = mat.Counts.Item2;
                for (ulong y = 0; y < height; y++)
                {
                    if(!matrix.GetRow(yOffset++, out var row, out error))
                    {
                        error = "Failed to perform matrix multiplication: " + error;
                        return false;
                    }
                    var rowVec = row as OzAIHalfVec_CSharp;

                    for (ulong x = 0; x < width; x++)
                    {
                        var offset = rowOffset + x;
                        resNums[y] += rowVec.Values[offset] * srcVec.Values[offset];
                    }
                }
                for (ulong i = 0; i < height; i++)
                {
                    dstVec.Values[i] = resNums[i];
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform matrix multiplication: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void MatMulF(OzAIRAMDStorage mat, OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override void MatMulH(OzAIRAMDStorage mat, OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool Max(OzAIVectorRange src, OzAIScalar dst, out string error)
        {
            try
            {
                var dstVec = dst.Vector as OzAIIntVec_CSharp;
                var srcVec = src.Vector as OzAIHalfVec_CSharp;

                var srcOffset = src.Offset;
                var dstOffset = dst.Offset;
                var count = src.Length;

                var max = (Half)0;
                for (ulong i = 0; i < count; i++)
                {
                    var val = srcVec.Values[srcOffset];
                    if (val > max)
                    {
                        max = val;
                        if (!dstVec.SetNthInt((int)srcOffset, dstOffset, out error))
                            return false;
                    }
                    srcOffset++;
                }
            }
            catch (Exception ex)
            {
                error = "Failed to obtain maximum element of vector: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void MaxF(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override void MaxH(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool RMS(OzAIScalar epsilon, OzAIScalar part, OzAIVector src, OzAIVectorRange dst, out string error)
        {
            try
            {
                var epsilonVec = epsilon.Vector as OzAIHalfVec_CSharp;
                var epsilonVal = epsilonVec.Values[epsilon.Offset];
                var partVec = part.Vector as OzAIHalfVec_CSharp;
                var partVal = partVec.Values[part.Offset];

                var srcVec = src as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;

                var dstOffset = dst.Offset;
                var count = dst.Length;

                // Squared
                Half sum = (Half)0;
                for (ulong i = 0; i < count; i++)
                {
                    sum += srcVec.Values[i] * srcVec.Values[i];
                }

                // Mean
                Half mean = sum / (Half)count;

                // Root
                Half sqrt = Half.Sqrt(mean + epsilonVal);

                //Norm
                for (ulong i = 0; i < count; i++)
                {
                    dstVec.Values[dstOffset++] = srcVec.Values[i] / sqrt;
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform RMS Normalization: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void RMSF(OzAIRAMDStorage epsilon, OzAIRAMDStorage part, OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool RoPE(ulong position, OzAIScalar thetaBase, OzAIVectorRange src, OzAIVectorRange dst, out string error)
        {
            try
            {
                var thetasVec = thetaBase.Vector as OzAIHalfVec_CSharp;
                var thetaVal = thetasVec.Values[thetaBase.Offset];
                var srcVec = src.Vector as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;

                var srcOffset = src.Offset;
                var dstOffset = dst.Offset;
                //var thetasOffset = thetas.Offset;

                var ropeCount = src.Length / 2;
                //var forwardCount = dst.Length - thetas.Length * 2;

                // RoPE
                for (ulong i = 0; i < ropeCount; i++)
                {
                    var exponent = ((Half)2 * (Half)i) / (Half)src.Length;
                    var theta = Half.Pow(thetaVal, -exponent);
                    var scaledTheta = theta * (Half)position;
                    var val1 = srcVec.Values[srcOffset++];
                    var val2 = srcVec.Values[srcOffset++];
                    dstVec.Values[dstOffset++] = Half.Cos(scaledTheta) * val1 - Half.Sin(scaledTheta) * val2;
                    dstVec.Values[dstOffset++] = Half.Sin(scaledTheta) * val1 + Half.Cos(scaledTheta) * val2;
                }

                if (srcVec == dstVec)
                {
                    error = null;
                    return true;
                }

                // Forward
                //for (ulong i = 0; i < forwardCount; i++)
                //{
                //    dstVec.Values[dstOffset++] = srcVec.Values[srcOffset++];
                //}
            }
            catch (Exception ex)
            {
                error = "Failed to perform rotary positional embedding: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void RoPEF(nint position, OzAIRAMDStorage thetaBase, OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool Scale(OzAIVectorRange src, OzAIScalar scalar, OzAIVectorRange dst, out string error)
        {
            try
            {
                var scalarVec = scalar.Vector as OzAIHalfVec_CSharp;
                var scalarVal = scalarVec.Values[scalar.Offset];
                var srcVec = src.Vector as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;
                var srcOffset = src.Offset;
                var dstOffset = dst.Offset;
                var count = src.Length;
                for (ulong i = 0; i < count; i++)
                {
                    dstVec.Values[dstOffset++] = srcVec.Values[srcOffset++] * scalarVal;
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform scaling: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void ScaleF(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override void ScaleH(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool SoftMax(OzAIVector src, OzAIVectorRange dst, out string error)
        {
            try
            {
                var srcVec = src as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;

                var dstOffset = dst.Offset;
                var count = dst.Length;

                // Exp
                Half sum = (Half)0;
                for (ulong i = 0; i < count; i++)
                {
                    var val = srcVec.Values[i];
                    var exp = Half.Exp(val); 
                    dstVec.Values[dstOffset + i] = exp;
                    sum += exp;
                }

                //Norm
                for (ulong i = 0; i < count; i++)
                {
                    dstVec.Values[dstOffset++] /= sum;
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform softmax: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void SoftMaxF(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }

        public override bool Sum(OzAIVectorRange src, OzAIScalar dst, out string error)
        {
            try
            {
                var srcVec = src.Vector as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;

                var srcOffset = src.Offset;
                var count = src.Length;

                for (ulong i = 0; i < count; i++)
                {
                    dstVec.Values[dst.Offset] += srcVec.Values[srcOffset++];
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform summation: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void SumF(OzAIRAMDStorage src, OzAIRAMDStorage scalar)
        {
            throw new NotImplementedException();
        }

        public override void SumH(OzAIRAMDStorage src, OzAIRAMDStorage scalar)
        {
            throw new NotImplementedException();
        }

        public override bool Swish1(OzAIVectorRange src, OzAIVectorRange dst, out string error)
        {
            try
            {
                var srcVec = src.Vector as OzAIHalfVec_CSharp;
                var dstVec = dst.Vector as OzAIHalfVec_CSharp;

                var srcOffset = src.Offset;
                var dstOffset = dst.Offset;
                var count = dst.Length;

                for (ulong i = 0; i < count; i++)
                {
                    var val = srcVec.Values[srcOffset++];
                    var neg = -val;
                    var exp = Half.Exp(neg);
                    dstVec.Values[dstOffset++] = val / ((Half)1 + exp);
                }
            }
            catch (Exception ex)
            {
                error = "Failed to perform swish with beta = 1: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override void Swish1F(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            throw new NotImplementedException();
        }
    }
}
