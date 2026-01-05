using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public partial class OzAIIntVec_CSharp : OzAIIntVec
    {
        public int[] Values;


        public override bool Clone(out OzAIVector res, out string error)
        {
            res = null;
            error = "Not implemented yet";
            return false;
        }


        public override bool GetSize(out ulong size, out string error)
        {
            if (Values == null)
            {
                size = ulong.MaxValue;
                error = "IntVector not initialized.";
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
                error = "IntVector not initialized.";
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
                error = "IntVector not initialized.";
                return false;
            }

            size = (ulong)Values.LongLength;
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


        public override bool GetNth(ulong index, out float res, out string error)
        {
            res = Values[index];
            error = null;
            return true;
        }

        public override bool GetNthInt(ulong index, out int res, out string error)
        {
            res = Values[index];
            error = null;
            return true;
        }

        public override bool SetNthInt(int res, ulong index, out string error)
        {
            Values[index] = res;
            error = null;
            return true;
        }
    }
}
