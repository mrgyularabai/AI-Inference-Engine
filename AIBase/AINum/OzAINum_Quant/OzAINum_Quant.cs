using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAINum_Quant : OzAINum
    {
        protected override ulong GetBlockCount()
        {
            return ulong.MaxValue;
        }

        protected override ulong GetCount()
        {
            return ulong.MaxValue;
        }

        protected override float GetNumber(ulong index)
        {
            return float.NaN;
        }

        protected override void SetNumber(ulong index, float val)
        {
            
        }

        protected override bool GetIsQuantized()
        {
            return true;
        }
    }
}
