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
    public partial class OzAIFloatVec_CSharp : OzAIFloatVec
    {
        public float[] Values;

        public override bool GetSize(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get size, because OzAIFloatVec_CSharp not initialized.";
                return false;
            }
            size = (ulong)Values.LongLength * 4;
            error = null;
            return true;
        }

        public override bool GetNumCount(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get number count, because OzAIFloatVec_CSharp not initialized.";
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
                error = "Could not get block count, because OzAIFloatVec_CSharp not initialized.";
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
            size = 4ul;
            error = null;
            return true;
        }

        public override bool GetNth(ulong index, out float res, out string error)
        {
            if (Values == null)
            {
                res = float.NaN;
                error = $"Could not get element number {index}, because OzAIFloatVec_CSharp not initialized.";
                return false;
            }
            if (index >= (ulong)Values.LongLength)
            {
                res = float.NaN;
                error = $"Could not get element number {index}, because this OzAIFloatVec_CSharp only has {Values.LongLength} elements.";
                return false;
            }
            res = Values[index];
            error = null;
            return true;
        }

        public override bool Clone(out OzAIVector res, out string error)
        {
            res = null;
            if (Values == null)
            {
                error = "Could not clone OzAIFloatVec_CSharp, because it is not initialized.";
                return false;
            }
            if (!GetProcMode(out var mode, out error))
            {
                error = "Could not clone OzAIFloatVec_CSharp: " + error;
                return false;
            }
            if (!Create(mode, out var vecRes, out error))
            {
                error = "Could not clone OzAIFloatVec_CSharp: " + error;
                return false;
            }
            if (!vecRes.Init(Values, 0, (ulong)Values.LongLength, out error))
            {
                error = "Could not clone OzAIFloatVec_CSharp: " + error;
                return false;
            }
            res = vecRes;
            return true;
        }
    }
}
