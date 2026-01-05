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
    partial class OzAIIntVec_CSharp
    {
        public override bool Init(ulong length, out string error)
        {
            Values = new int[length];
            error = null;
            return true;
        }

        public override bool Init(byte[] data, ulong offset, ulong length, out string error)
        {
            if (data == null)
            {
                error = "OzAIIntVec_CSharp could not be initialized, because the byte[] 'data' provided was null.";
                return false;
            }
            try
            {
                Values = new int[length];
                Buffer.BlockCopy(data, (int)offset, Values, 0, (int)length * 4);
            }
            catch (Exception ex)
            {
                error = "Could not init OzAIIntVec_CSharp:" + ex.Message;
                return false;
            }
            error = null;
            return true;
        }

        public override bool Init(float[] data, ulong offset, ulong length, out string error)
        {
            if (data == null)
            {
                error = "OzAIIntVec_CSharp could not be initialized, because the float[] 'data' provided was null.";
                return false;
            }
            try
            {
                Values = new int[length];
                for (ulong i = 0; i < length; i++)
                {
                    Values[i] = (int)data[offset++];
                }
            }
            catch (Exception ex)
            {
                error = "Could not init OzAIIntVec_CSharp:" + ex.Message;
                return false;
            }
            error = null;
            return true;
        }
    }
}
