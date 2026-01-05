using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozeki
{
    public partial class OzAIGLU : OzAIArchComp
    {
        public override string Name => "OzAIGLU";

        // GateMem
        public OzAIMemNode GateOut;
        public OzAIMemNode Acc;

        protected override bool InitInner(out string error)
        {
            Acc = new OzAIMemNode();
            GateOut = new OzAIMemNode();

            var actMem = new OzAICompIOMem_Unary()
            {
                Inputs = GateOut,
                Outputs = GateOut
            };

            var compParams = new OzAICompParams()
            {
                Mem = actMem,
                IParams = IParams.ActivationParams,
                HParams = HParams.ActivationParams

            };

            if (!IParams.Activation.Init(compParams, out error))
                return false;

            return true;
        }

        public override bool Forward(out string error)
        {
            if (!InitMem(out error)) return false;
            if (!CalcTop(out error)) return false;
            if (!CalcGate(out error)) return false;
            if (!CalcBottom(out error)) return false;

            error = null;
            return true;
        }

        bool InitMem(out string error)
        {
            if (!Params.IParams.ExecManager.GetProcMode(out var mode, out error))
                return false;
            Acc.Clear();
            if (!Acc.AddVecs(mode, HParams.FFNLength, Mem.Inputs.Count, out error))
                return false;
            GateOut.Clear();
            if (!GateOut.AddVecs(mode, HParams.FFNLength, Mem.Inputs.Count, out error))
                return false;
            return true;
        }

        bool CalcTop(out string error)
        {
            var exec = Params.IParams.ExecManager;
            var inps = Mem.Inputs.GetArray();
            var acc = Acc.GetArray();

            if (!exec.MatMul(inps, IParams.WeightsTop, acc, out error))
                return false;

            var doBias = IParams.BiasTop != null;
            if (doBias && !exec.Add(acc, IParams.BiasTop, acc, out error))
                return false;

            return true;
        }

        bool CalcGate(out string error)
        {
            var exec = Params.IParams.ExecManager;
            var inps = Mem.Inputs.GetArray();
            var gateOut = GateOut.GetArray();
            var acc = Acc.GetArray();

            if (!exec.MatMul(inps, IParams.WeightsGate, gateOut, out error))
                return false;
            if (!IParams.Activation.Forward(out error))
                return false;
            if (!exec.Had(gateOut, acc, acc, out error))
                return false;

            return true;
        }

        bool CalcBottom(out string error)
        {
            var exec = Params.IParams.ExecManager;
            var inps = Mem.Inputs.GetArray();
            var acc = Acc.GetArray();
            var outs = Mem.Outputs.GetArray();

            if (!exec.MatMul(acc, IParams.WeightsBottom, outs, out error))
                return false;

            var doBias = IParams.BiasBottom != null;
            if (doBias && !exec.Add(outs, IParams.BiasBottom, outs, out error))
                return false;

            return true;
        }
    }
}
