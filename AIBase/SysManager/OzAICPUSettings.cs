using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public class OzAICPUSettings
    {
        public static bool GetDefault(out OzAICPUSettings res, out string error)
        {
            res = new OzAICPUSettings();
            res.UseAVX = false;
            res.ThreadCount = (uint)Environment.ProcessorCount;
            res.EnableColumSplit = false;
            res.DefaultProcType = OzAINumType.Float16;
            error = null;
            return true;
        }

        public bool IsInitialized(out string error)
        {
            if (ThreadCount == 0)
            { 
                error = "CPU settings not initialized properly, because 'ThreadCount' has to be greater than 0.";
                return false;
            }
            if (DefaultProcType == OzAINumType.None)
            {
                error = "CPU settings not initialized properly, because 'DefaultProcType' cannot be 'None'.";
                return false;
            }
            error = null;
            return true;
        }

        /// <summary>
        /// Advanced Vector Extension enables operations to be performed in batches of 128/256bit on the CPU.
        /// </summary>
        public bool UseAVX;

        /// <summary>
        /// How many threads should be used to run on the CPU.
        /// </summary>
        public uint ThreadCount;

        /// <summary>
        /// Splitting by columns means that threads each get a partial result vector which can then be used to do a part of
        /// the rows of the next matrix multiplication. <br/>
        /// This means:  <br/>
        /// - Threads get to perform 2 matrix multiplcations consecutively without waiting for other threads. <br/>
        /// - Tthe main thread does not perform maths, but only synchronization. <br/>
        /// - Using partial results takes up ThreadCount times as much memory as than if they were all combined like
        /// in the row split mode. 
        /// </summary>
        public bool EnableColumSplit;

        /// <summary>
        /// This is the data type used by default to perform all computations.
        /// </summary>
        public OzAINumType DefaultProcType;
    }
}
