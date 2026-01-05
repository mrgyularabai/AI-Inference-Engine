using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public class OzAICPUSharpExec : OzAICPUExecutor
    {
        public override bool Add(OzAIVectorRange src1, OzAIVectorRange src2, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void AddF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            float* pSrc1 = (float*)src1.Addr;
            float* pSrc2 = (float*)src2.Addr;
            float* pDst = (float*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                pDst[i] = pSrc1[i] + pSrc2[i];
            }
        }

        public override unsafe void AddH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            Half* pSrc1 = (Half*)src1.Addr;
            Half* pSrc2 = (Half*)src2.Addr;
            Half* pDst = (Half*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                pDst[i] = pSrc1[i] + pSrc2[i];
            }
        }

        public override bool Div(OzAIVectorRange src, OzAIScalar scalar, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void DivF(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst)
        {
            float* pSrc1 = (float*)src.Addr;
            float scalarVal = *(float*)scalar.Addr;
            float* pDst = (float*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                pDst[i] = pSrc1[i] / scalarVal;
            }
        }

        public override unsafe void DivH(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst)
        {
            Half* pSrc1 = (Half*)src.Addr;
            Half scalarVal = *(Half*)scalar.Addr;
            Half* pDst = (Half*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                pDst[i] = pSrc1[i] / scalarVal;
            }
        }

        public override bool Dot(OzAIVector src1, OzAIVector src2, OzAIScalar dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void DotF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            float* pSrc1 = (float*)src1.Addr;
            float* pSrc2 = (float*)src2.Addr;
            var sum = 0.0f;
            for (nuint i = 0; i < src1.Size; i++)
            {
                sum += pSrc1[i] * pSrc2[i];
            }
            float* pDst = (float*)dst.Addr;
            *pDst = sum;
        }

        public override unsafe void DotH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            Half* pSrc1 = (Half*)src1.Addr;
            Half* pSrc2 = (Half*)src2.Addr;
            var sum = (Half)0;
            for (nuint i = 0; i < src1.Size; i++)
            {
                sum += pSrc1[i] * pSrc2[i];
            }
            Half* pDst = (Half*)dst.Addr;
            *pDst = sum;
        }

        //public override unsafe void ExpF(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        //{
        //    float* pSrc = (float*)src.Addr;
        //    float* pDst = (float*)dst.Addr;
        //    for (nuint i = 0; i < src.Size; i++)
        //    {
        //        pDst[i] = MathF.Exp(pSrc[i]);
        //    }
        //}

        public override bool Had(OzAIVectorRange src1, OzAIVectorRange src2, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void HadF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            float* pSrc1 = (float*)src1.Addr;
            float* pSrc2 = (float*)src2.Addr;
            float* pDst = (float*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                pDst[i] = pSrc1[i] * pSrc2[i];
            }
        }

        public override unsafe void HadH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst)
        {
            Half* pSrc1 = (Half*)src1.Addr;
            Half* pSrc2 = (Half*)src2.Addr;
            Half* pDst = (Half*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                pDst[i] = pSrc1[i] * pSrc2[i];
            }
        }

        public override bool MatMul(OzAIVectorRange src, OzAIMatrixRange mat, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void MatMulF(OzAIRAMDStorage mat, OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            float* pMat = (float*)mat.Addr;
            float* vec = (float*)src.Addr;
            float* pDst = (float*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                var sum = 0.0f;
                for (nuint j = 0; j < src.Size; j++)
                {
                    sum += *pMat++ * vec[j];
                }
                pDst[i] = sum;
            }
        }

        public override unsafe void MatMulH(OzAIRAMDStorage mat, OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            Half* pMat = (Half*)mat.Addr;
            Half* vec = (Half*)src.Addr;
            Half* pDst = (Half*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                var sum = (Half)0.0;
                for (nuint j = 0; j < src.Size; j++)
                {
                    sum += *pMat++ * vec[j];
                }
                pDst[i] = sum;
            }
        }

        public override bool Max(OzAIVectorRange src, OzAIScalar dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void MaxF(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            float* pSrc1 = (float*)src.Addr;
            var max = float.NegativeInfinity;
            for (nuint i = 0; i < src.Size; i++)
            {
                max = pSrc1[i] <= max ? max : pSrc1[i];
            }
            float* pDst = (float*)dst.Addr;
            *pDst = max;
        }

        public override unsafe void MaxH(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            Half* pSrc1 = (Half*)src.Addr;
            var max = Half.NegativeInfinity;
            for (nuint i = 0; i < src.Size; i++)
            {
                max = pSrc1[i] <= max ? max : pSrc1[i];
            }
            Half* pDst = (Half*)dst.Addr;
            *pDst = max;
        }

        public override bool RMS(OzAIScalar epsilon, OzAIScalar part, OzAIVector src, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void RMSF(OzAIRAMDStorage epsilon, OzAIRAMDStorage part, OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            var pEpsilon = (float*)epsilon.Addr;
            var epsilonVal = *pEpsilon;
            var pPart = (float*)part.Addr;
            var partVal = *pPart;

            var pSrc = (float*)src.Addr;
            var pDst = (float*)dst.Addr;
            var count = dst.Size;

            // Squared
            float sum = 0;
            for (nuint i = 0; i < count; i++)
            {
                sum += pSrc[i] * pSrc[i];
            }

            // Mean
            float mean = sum / count;

            // Root
            var meanPlusEps = mean + epsilonVal;
            var sqrt = MathF.Sqrt(meanPlusEps);
            float rsqrt = (float)(1.0 / sqrt);

            //Norm
            for (nuint i = 0; i < count; i++)
            {
                var val = pSrc[i];
                var newVal = (double)val * rsqrt;
                pDst[i] = (float)newVal;
            }
        }

        public override bool RoPE(ulong position, OzAIScalar thetaBase, OzAIVectorRange src, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void RoPEF(nint position, OzAIRAMDStorage thetaBase, OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            var pTheta = (float*)thetaBase.Addr;
            var thetaVal = *pTheta;
            var pSrc = (float*)src.Addr;
            var pDst = (float*)dst.Addr;

            var ropeCount = src.Size / 2;

            // RoPE
            for (ulong i = 0; i < ropeCount; i++)
            {
                var exponent = (2f * (float)i) / (float)src.Size;
                var theta = MathF.Pow(thetaVal, -exponent);
                var scaledTheta = theta * (float)position;
                var val1 = *pSrc++;
                var val2 = *pSrc++;
                *pDst++ = MathF.Cos(scaledTheta) * val1 - MathF.Sin(scaledTheta) * val2;
                *pDst++ = MathF.Sin(scaledTheta) * val1 + MathF.Cos(scaledTheta) * val2;
            }
        }

        public override bool Scale(OzAIVectorRange src, OzAIScalar scalar, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void ScaleF(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst)
        {
            float* pSrc1 = (float*)src.Addr;
            float scalarVal = *(float*)scalar.Addr;
            float* pDst = (float*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                pDst[i] = pSrc1[i] * scalarVal;
            }
        }

        public override unsafe void ScaleH(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst)
        {
            Half* pSrc1 = (Half*)src.Addr;
            Half scalarVal = *(Half*)scalar.Addr;
            Half* pDst = (Half*)dst.Addr;
            for (nuint i = 0; i < dst.Size; i++)
            {
                pDst[i] = pSrc1[i] * scalarVal;
            }
        }

        public override bool SoftMax(OzAIVector src, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        // SoftMax for float values
        public unsafe override void SoftMaxF(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            float* pSrc = (float*)src.Addr;
            float* pDst = (float*)dst.Addr;
            int n = (int)src.Size;

            // Find max for numerical stability
            float max = pSrc[0];
            for (int i = 1; i < n; i++)
            {
                if (pSrc[i] > max) max = pSrc[i];
            }

            // Compute exponentials and sum
            float sum = 0f;
            for (int i = 0; i < n; i++)
            {
                float e = MathF.Exp(pSrc[i] - max);
                pDst[i] = e;
                sum += e;
            }

            // Normalize
            for (int i = 0; i < n; i++)
            {
                pDst[i] /= sum;
            }
        }

        public override bool Sum(OzAIVectorRange src, OzAIScalar scalar, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void SumF(OzAIRAMDStorage src, OzAIRAMDStorage scalar)
        {
            float* pSrc = (float*)src.Addr;
            float* pDst = (float*)scalar.Addr;
            var sum = 0.0f;
            for (nuint i = 0; i < src.Size; i++)
            {
                sum += pSrc[i];
            }
            *pDst = sum;
        }

        public override unsafe void SumH(OzAIRAMDStorage src, OzAIRAMDStorage scalar)
        {
            Half* pSrc = (Half*)src.Addr;
            Half* pDst = (Half*)scalar.Addr;
            var sum = (Half)0;
            for (nuint i = 0; i < src.Size; i++)
            {
                sum += pSrc[i];
            }
            *pDst = sum;
        }

        public override bool Swish1(OzAIVectorRange src, OzAIVectorRange dst, out string error)
        {
            throw new NotImplementedException();
        }

        public override unsafe void Swish1F(OzAIRAMDStorage src, OzAIRAMDStorage dst)
        {
            float* pSrc = (float*)src.Addr;
            float* pDst = (float*)dst.Addr;
            for (nuint i = 0; i < src.Size; i++)
            {
                float x = pSrc[i];
                float sigmoid = 1.0f / (1.0f + MathF.Exp(-x));
                pDst[i] = x * sigmoid;
            }
        }
    }
}
