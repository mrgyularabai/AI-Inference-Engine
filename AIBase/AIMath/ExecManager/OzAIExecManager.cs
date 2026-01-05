using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public partial class OzAIExecManager
    {
        Dictionary<OzAINumType, OzAIExecutor> _main;
        Dictionary<OzAINumType, List<OzAIExecutor>> _executors;
        OzAIProcMode _mode;
        OzAICPUSettings _cpu;
        public bool GetProcMode(out OzAIProcMode mode, out string error)
        {
            if (_mode == null)
            {
                mode = null;
                error = "Execution Manager not initialized.";
                return false;
            }
            mode = _mode;
            error = null;
            return true;
        }

        public bool Init(OzAIProcMode mode, out string error)
        {
            _mode = mode;
            _executors = new Dictionary<OzAINumType, List<OzAIExecutor>>();
            _main = new Dictionary<OzAINumType, OzAIExecutor>();

            if (!_mode.GetCPUSettings(out _cpu, out error))
                return false;
            var defaultType = _cpu.DefaultProcType;
            if (!createExecs(defaultType, out error))
            {
                error = "Could not create executors for given default vector data type: " + error;
                return false;
            }
            error = null;
            return true;
        }

        bool getMain(List<OzAIVector> vecs, out OzAIExecutor res, out string error, bool checkDtypes = true)
        {
            res = null;
            var dType = vecs[0].GetNumType();

            if (checkDtypes)
            {
                foreach (var vec in vecs)
                {
                    if (dType != vec.GetNumType())
                    {
                        error = "Data types of provided vectors do not agree!";
                        return false;
                    }
                }
            }

            if (!_main.ContainsKey(dType))
            {
                if (!createExecs(dType, out error))
                {
                    error = "Could not create executors for given vector data type: " + error;
                    return false;
                }
            }

            res = _main[dType];

            error = null; 
            return true;
        }

        bool getExecs(List<OzAIVector> vecs, out List<OzAIExecutor> res, out string error, bool checkDtypes = true)
        {
            res = null;
            var dType = vecs[0].GetNumType();

            if (checkDtypes)
            {
                foreach (var vec in vecs)
                {
                    if (dType != vec.GetNumType())
                    {
                        error = "Data types of provided vectors do not agree!";
                        return false;
                    }
                }
            }

            if (!_executors.ContainsKey(dType))
            {
                if (!createExecs(dType, out error))
                {
                    error = "Could not create executors for given vector data type: " + error;
                    return false;
                }
            }

            res = _executors[dType];

            error = null;
            return true;
        }

        bool createExecs(OzAINumType dType, out string error)
        {
            if (!dType.CreateCPUExec(out var cpuExec, out error))
                return false;
            _main.Add(dType, cpuExec);
            if (!_main[dType].Start(_mode, out error))
                return false;

            var execs = new List<OzAIExecutor>();
            _executors.Add(dType, execs);

            execs.Add(cpuExec);

            for (int i = 1; i < _cpu.ThreadCount; i++)
            {
                if (!dType.CreateCPUExec(out cpuExec, out error))
                    return false;
                OzAIExecutor exec = cpuExec;
                if (!exec.Start(_mode, out error))
                    return false;
                execs.Add(exec);
            }
            return true;
        }

        ~OzAIExecManager()
        {
            foreach (var item in _executors)
            {
                foreach (var exec in item.Value)
                {
                    exec.Stop();
                }
            }
        }
    }
}
