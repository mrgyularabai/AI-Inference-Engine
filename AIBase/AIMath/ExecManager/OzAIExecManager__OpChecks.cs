using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIExecManager
    {
        bool checkUnaryOpInps(Array src, OzAIVectorRange dst, out string error)
        {
            List<object> objs = [src, dst];
            List<string> names = ["Source", "Destination"];
            if (!OzAICheckable.CheckIfNull(objs, names, out error))
                return false;

            if ((ulong)src.LongLength != dst.Length)
            {
                error = "Div not possible, because the number of destinations does not match the number of sources provided.";
                return false;
            }

            error = null;
            return true;
        }

        bool checkUnaryOpInps(Array src, Array dst, out string error)
        {
            List<object> objs = [src, dst];
            List<string> names = ["Source", "Destination"];
            if (!OzAICheckable.CheckIfNull(objs, names, out error))
                return false;

            if (src.LongLength != dst.LongLength)
            {
                error = "Div not possible, because the number of destinations does not match the number of sources provided.";
                return false;
            }

            error = null;
            return true;
        }

        bool checkBinaryOpInps(Array src1, Array src2, OzAIVectorRange dst, out string error)
        {
            List<object> objs = [src1, src2, dst];
            List<string> names = ["Source 1", "Source 2", "Destination"];
            if (!OzAICheckable.CheckIfNull(objs, names, out error))
                return false;
            if (src1.LongLength != src2.LongLength)
            {
                error = "Add not possible, because the number of sources from source 1 does not match the number of source from source 2 provided.";
                return false;
            }
            if ((ulong)src1.LongLength != dst.Length)
            {
                error = "Add not possible, because the number of destinations does not match the number of sources provided.";
                return false;
            }

            error = null;
            return true;
        }

        bool checkBinaryOpInps(Array src1, Array src2, Array dst, out string error)
        {
            List<object> objs = [src1, src2, dst];
            List<string> names = ["Source 1", "Source 2", "Destination"];
            if (!OzAICheckable.CheckIfNull(objs, names, out error))
                return false;
            if (src1.LongLength != src2.LongLength)
            {
                error = "Add not possible, because the number of sources from source 1 does not match the number of source from source 2 provided.";
                return false;
            }
            if (src1.LongLength != dst.LongLength)
            {
                error = "Add not possible, because the number of destinations does not match the number of sources provided.";
                return false;
            }

            error = null;
            return true;
        }

        void getOperandCounts(int opCount, out ulong mainCount, out ulong normalCount)
        {
            var mainAddition = opCount % _cpu.ThreadCount;
            normalCount = (ulong)(opCount - mainAddition) / _cpu.ThreadCount;
            mainCount = normalCount + (ulong)mainAddition;
        }
    }
}
