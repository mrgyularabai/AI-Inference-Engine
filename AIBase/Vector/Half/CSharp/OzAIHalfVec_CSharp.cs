using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIHalfVec_CSharp : OzAIHalfVec
    {
        public Half[] Values;

        public override bool GetSize(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get size, because OzAIHalfVec_CSharp not initialized.";
                return false;
            }
            size = (ulong)Values.LongLength * 2;
            error = null;
            return true;
        }

        public override bool GetNumCount(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get number count, because OzAIHalfVec_CSharp not initialized.";
                return false;
            }
            size = (ulong)Values.LongLength;
            error = null;
            return true;
        }

        public override bool GetBlockCount(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get block count, because OzAIHalfVec_CSharp not initialized.";
                return false;
            }
            size = (ulong)Values.LongLength;
            error = null;
            return true;
        }

        public override bool GetNumsPerBlock(out ulong size, out string error)
        {
            size = 1ul;
            error = null;
            return true;
        }

        public override bool GetBytesPerBlock(out ulong size, out string error)
        {
            size = 2ul;
            error = null;
            return true;
        }

        public override bool GetNth(ulong index, out float res, out string error)
        {
            if (Values == null)
            {
                res = float.NaN;
                error = $"Could not get element number {index}, because OzAIHalfVec_CSharp not initialized.";
                return false;
            }
            if (index >= (ulong)Values.LongLength)
            {
                res = float.NaN;
                error = $"Could not get element number {index}, because this OzAIHalfVec_CSharp only has {Values.LongLength} elements.";
                return false;
            }
            res = (float)Values[index];
            error = null;
            return true;
        }

        public override bool Clone(out OzAIVector res, out string error)
        {
            res = null;
            if (Values == null)
            {
                error = "Could not clone OzAIHalfVec_CSharp, because it is not initialized.";
                return false;
            }
            if (!GetProcMode(out var mode, out error))
            {
                error = "Could not clone OzAIHalfVec_CSharp: " + error;
                return false;
            }
            if (!Create(mode, out var vecRes, out error))
            {
                error = "Could not clone OzAIHalfVec_CSharp: " + error;
                return false;
            }
            if (!vecRes.Init((ulong)Values.LongLength, out error))
            {
                error = "Could not clone OzAIHalfVec_CSharp: " + error;
                return false;
            }
            var halfVec = vecRes as OzAIHalfVec_CSharp;
            Values.CopyTo(halfVec.Values, 0);
            res = halfVec;
            return true;
        }

        public override bool SetNthHalf(Half val, ulong index, out string error)
        {
            if (Values == null)
            {
                error = "Could not set nth half, because OzAIHalfVec_CSharp not initialized.";
                return false;
            }
            Values[index] = val; 
            error = null;
            return true;
        }
    }
}
