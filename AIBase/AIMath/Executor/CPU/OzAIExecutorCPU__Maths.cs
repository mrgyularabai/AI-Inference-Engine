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
        public abstract bool Add(OzAIVectorRange src1, OzAIVectorRange src2, OzAIVectorRange dst, out string error);
        public abstract void AddF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst);
        public abstract void AddH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst);

        public abstract bool Div(OzAIVectorRange src, OzAIScalar scalar, OzAIVectorRange dst, out string error);
        public abstract void DivF(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst);
        public abstract void DivH(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst);

        public abstract bool Dot(OzAIVector src1, OzAIVector src2, OzAIScalar dst, out string error);
        public abstract void DotF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst);
        public abstract void DotH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst);

        public abstract bool Had(OzAIVectorRange src1, OzAIVectorRange src2, OzAIVectorRange dst, out string error);
        public abstract void HadF(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst);
        public abstract void HadH(OzAIRAMDStorage src1, OzAIRAMDStorage src2, OzAIRAMDStorage dst);

        public abstract bool MatMul(OzAIVectorRange src, OzAIMatrixRange mat, OzAIVectorRange dst, out string error);
        public abstract void MatMulF(OzAIRAMDStorage mat, OzAIRAMDStorage src, OzAIRAMDStorage dst);
        public abstract void MatMulH(OzAIRAMDStorage mat, OzAIRAMDStorage src, OzAIRAMDStorage dst);

        // Think about splitting
        public abstract bool RMS(OzAIScalar epsilon, OzAIScalar part, OzAIVector src, OzAIVectorRange dst, out string error);
        public abstract void RMSF(OzAIRAMDStorage epsilon, OzAIRAMDStorage part, OzAIRAMDStorage src, OzAIRAMDStorage dst);


        // Think about splitting
        public abstract bool RoPE(ulong position, OzAIScalar thetaBase, OzAIVectorRange src, OzAIVectorRange dst, out string error);
        public abstract void RoPEF(nint position, OzAIRAMDStorage thetaBase, OzAIRAMDStorage src, OzAIRAMDStorage dst);

        public abstract bool Scale(OzAIVectorRange src, OzAIScalar scalar, OzAIVectorRange dst, out string error);
        public abstract void ScaleF(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst);
        public abstract void ScaleH(OzAIRAMDStorage src, OzAIRAMDStorage scalar, OzAIRAMDStorage dst);

        public abstract bool SoftMax(OzAIVector src, OzAIVectorRange dst, out string error);
        public abstract void SoftMaxF(OzAIRAMDStorage src, OzAIRAMDStorage dst);

        public abstract bool Sum(OzAIVectorRange src, OzAIScalar scalar, out string error);
        public abstract void SumF(OzAIRAMDStorage src, OzAIRAMDStorage scalar);
        public abstract void SumH(OzAIRAMDStorage src, OzAIRAMDStorage scalar);

        public abstract bool Swish1(OzAIVectorRange src, OzAIVectorRange dst, out string error);
        public abstract void Swish1F(OzAIRAMDStorage src, OzAIRAMDStorage dst);

        public abstract bool Max(OzAIVectorRange src, OzAIScalar dst, out string error);
        public abstract void MaxF(OzAIRAMDStorage src, OzAIRAMDStorage dst);
        public abstract void MaxH(OzAIRAMDStorage src, OzAIRAMDStorage dst);
    }
}
