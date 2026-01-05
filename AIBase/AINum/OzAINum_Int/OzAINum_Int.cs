using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAINum_Int : OzAINum
    {
        protected override ulong GetBlockCount()
        {
            return GetCount();
        }

        protected override bool GetIsQuantized()
        {
            return false;
        }

        protected override ulong GetNumsPerBlock()
        {
            return 1;
        }
    }
}
