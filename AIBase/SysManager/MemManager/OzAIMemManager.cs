using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIMemManager
    {
        /// <summary>
        /// 64 for 512-bit AVX-512 alignment.
        /// </summary>
        public static nuint AlignmentBytes = 64;
        public static OzAIRAM RAM = new OzAIRAM();
    }
}
