using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAIAddition : OzAIBinaryOperation
    {
        public override OzAIOperationType Type => OzAIOperationType.Addition;
    }
}
