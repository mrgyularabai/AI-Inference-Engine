using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIFloatMat_CSharp : OzAIFloatMat
    {
        ulong _count;
        ulong _width;
        ulong _height;
        public float[] Values;

        public override bool GetSize(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get OzAIHalfMat's size, because OzAIHalfMat_CSharp is not initialized.";
                return false;
            }
            size = _count * 4;
            error = null;
            return true;
        }

        public override bool GetNumCount(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get OzAIHalfMat's number count, because OzAIHalfMat_CSharp is not initialized.";
                return false;
            }
            size = _count;
            error = null;
            return true;
        }

        public override bool GetBlockCount(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get OzAIHalfMat's block count, because OzAIHalfMat_CSharp is not initialized.";
                return false;
            }
            size = _count;
            error = null;
            return true;
        }

        public override bool GetNumsPerBlock(out ulong size, out string error)
        {
            size = 1;
            error = null;
            return true;
        }

        public override bool GetBytesPerBlock(out ulong size, out string error)
        {
            size = 4;
            error = null;
            return true;
        }

        public override bool GetWidth(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get OzAIHalfMat's width, because OzAIHalfMat_CSharp is not initialized.";
                return false;
            }
            size = _width;
            error = null;
            return true;
        }

        public override bool GetHeight(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "Could not get OzAIHalfMat's height, because OzAIHalfMat_CSharp is not initialized.";
                return false;
            }
            size = _height;
            error = null;
            return true;
        }

        public override bool GetRow(ulong y, out OzAIVector res, out string error)
        {
            res = null;
            if (Values == null)
            {
                error = "Could not get row from OzAIHalfMat_CSharp, because OzAIHalfMat_CSharp is not initialized.";
                return false;
            }
            if (y >= _height)
            {
                error = $"Could not get row from OzAIHalfMat_CSharp, because y value specified ({y}) was out of bounds (max. {_height})";
                return false;
            }

            if (!GetProcMode(out var mode, out error))
            {
                error = $"Could not get row from OzAIHalfMat_CSharp: " + error;
                return false;
            }

            if (!OzAIHalfVec.Create(mode, out var csvec, out error))
            {
                error = $"Could not get row from OzAIHalfMat_CSharp, becuase could not create destination vector: " + error;
                return false;
            }

            if (!csvec.Init(Values, y * _width, _width, out error))
            {
                error = $"Could not get row from OzAIHalfMat_CSharp, becuase could not initialize destination vector: " + error;
                return false;
            }

            res = csvec;
            error = null;
            return true;
        }

        public override bool GetRows(out OzAIVector[] res, out string error)
        {
            res = new OzAIVector[_height];
            for (ulong i = 0; i < _height; i++)
            {
                if (!GetRow(i, out res[i], out error))
                {
                    error = $"Could not get rows from OzAIHalMat_CSharp, becuase failed to get row {i}: " + error;
                    return false;
                }
            }
            error = null;
            return true;
        }
    }
}
