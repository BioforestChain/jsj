using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine
{
    internal class CallBackEvent
    {
        private static int _timeOutIdCounter = 0;

        internal ScriptObject? Function;
        internal double Delay;
        internal int Id;
        internal CallBackType Type;
        internal bool Enabled = true;
        internal string? Source;
        private int _alreadyWaited;

        internal bool ReadyForExecution(int waitedMilliSecond)
        {
            this._alreadyWaited += waitedMilliSecond;
            if (this._alreadyWaited > this.Delay)
            {
                _alreadyWaited = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool Disabled
        {
            get { return !this.Enabled; }
        }

        private CallBackEvent()
        {
            this.Id = _timeOutIdCounter++;
        }



        public CallBackEvent(string source) : this()
        {
            this.Source = source;
            this.Type = CallBackType.EvaluateScript;
        }

        public CallBackEvent(CallBackType callBackType) : this()
        {
            this.Type = callBackType;
        }

        public CallBackEvent(ScriptObject function, double delay, CallBackType type) : this()
        {
            this.Function = function;
            this.Delay = delay;
            this.Type = type;
        }

        public void Disable()
        {
            this.Enabled = false;
        }
    }
}
