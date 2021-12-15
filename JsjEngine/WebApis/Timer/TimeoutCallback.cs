using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.WebApis.Timer
{
    internal class TimeoutCallback
    {
        private static int _idCounter = 0;
        private int _id;
        public TimeoutCallback(ScriptObject callbackFunc, int delay, bool repeated=false)
        {
            _id = Interlocked.Increment(ref _idCounter);
            CallbackFunction = callbackFunc;
            Delay = delay;
            DueTime = DateTime.Now.AddMilliseconds(delay);
            Repeated = repeated;
        }

        public int Id => _id;
        public ScriptObject CallbackFunction { get; set; }
        public int Delay { get; set; }
        public DateTime DueTime { get; set; }
        public bool Repeated { get; set; }
        public bool Disabled { get; set; }
    }
}
