using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract partial class OzAICPUExecutor : OzAIExecutor
    {
        OzAIProcMode _mode;
        Thread _execThread;
        ManualResetEvent _process;
        ManualResetEvent _done;
        bool _run;

        public override bool Start(OzAIProcMode mode, out string error)
        {
            _mode = mode;
            _tasks = new List<OzAIOperation>();
            _execThread = new Thread(execute);
            _process = new ManualResetEvent(false);
            _done = new ManualResetEvent(true);
            _run = true;
            _execThread.Start();
            error = null;
            return true;
        }

        public override void Stop()
        {
            _done.WaitOne();
            _run = false;
            _process.Set();
        }

        List<OzAIOperation> _tasks;

        void execute()
        {
            while (_tasks.Count != 0 || _process.WaitOne())
            {
                while (_tasks.Count != 0)
                {
                    var item = _tasks.Last();
                    if (!perform(item, out _currentError))
                    {
                        _success = false;
                        _done.Set();
                        _process.Reset();
                        return;
                    }
                    _tasks.Remove(item);
                }
                
                if (_run && _tasks.Count == 0)
                {
                    _done.Set();
                    _process.Reset();
                }
            }
        }
        string _currentError;
        bool _success = true;
        public override bool AwaitAll(out string error)
        {
            _done.WaitOne();
            error = _currentError;
            _currentError = null;
            var res = _success;
            _success = true;
            return res;
        }

        bool perform(OzAIOperation op, out string error)
        {
            switch (op.Type)
            {
                case OzAIOperationType.Addition:
                    if (!BulckAdd(op, out error))
                        return false;
                    break;
                case OzAIOperationType.Div:
                    if (!BulckDiv(op, out error))
                        return false;
                    break;
                case OzAIOperationType.Dot:
                    if (!BulckDot(op, out error))
                        return false;
                    break;
                case OzAIOperationType.Had:
                    if (!BulckHad(op, out error))
                        return false;
                    break;
                case OzAIOperationType.MatMul:
                    if (!BulckMatMul(op, out error))
                        return false;
                    break;
                case OzAIOperationType.Max:
                    if (!OpMax(op, out error))
                        return false;
                    break;
                case OzAIOperationType.RMS:
                    if (!BulckRMS(op, out error))
                        return false;
                    break;
                case OzAIOperationType.RoPE:
                    if (!BulckRoPE(op, out error))
                        return false;
                    break;
                case OzAIOperationType.Scale:
                    if (!BulckScale(op, out error))
                        return false;
                    break;
                case OzAIOperationType.SoftMax:
                    if (!BulckSoftMax(op, out error))
                        return false;
                    break;
                case OzAIOperationType.Sum:
                    if (!BulckSum(op, out error))
                        return false;
                    break;
                case OzAIOperationType.Swish1:
                    if (!BulckSwish1(op, out error))
                        return false;
                    break;
                default:
                    error = $"Operation unkown operation: {op}.";
                    return false;
            }
            return true;
        }

        public override void Add(OzAIOperation operation)
        {
            _done.Reset();
            _tasks.Add(operation);
            _process.Set();
        }
    }
}
