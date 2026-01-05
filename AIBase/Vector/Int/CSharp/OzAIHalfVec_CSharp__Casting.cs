using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    partial class OzAIIntVec_CSharp
    {

        public override bool ToFloat(out float[] res, out string error)
        {
            res = new float[Values.LongLength];
            for (long i = 0; i < Values.LongLength; i++)
            {
                res[i] = Values[i];
            }
            error = null;
            return true;
        }

        public override bool ToBytes(out byte[] res, out string error)
        {
            res = new byte[Values.LongLength * 4];
            try
            {
                Buffer.BlockCopy(Values, 0, res, 0, res.Length);
            }
            catch (Exception ex)
            {
                error = "Could not convert OzAIIntVector to bytes: " + ex.Message;
                return false;
            }
            error = null;
            return true;
        }
    }
}
