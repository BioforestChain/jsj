using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace JsjEngine
{
    /// <summary>
    /// timer related web api imlementation
    /// </summary>
    public partial class JsjEngine
    {
        private Func<ScriptObject, double, int> _setTimeoutDelegate => (func, delay) => {
            return _eventQueue.Enqueue(new CallBackEvent(func, delay, CallBackType.TimeOut)).Id;
        };
        private Action<int> _clearTimeoutDelegate => (id) =>
        {
            _eventQueue.ClearCallBackEvent(id);
        };
        private Func<ScriptObject, double, int> _setIntervalDelegate => (func, delay) => {
            return _eventQueue.Enqueue(new CallBackEvent(func, delay, CallBackType.Interval)).Id;
        };

        private void RegisterTimerApis(V8ScriptEngine engine)
        {
            engine.Script.setTimeout = _setTimeoutDelegate;
            engine.Script.clearTimeout = _clearTimeoutDelegate;
            engine.Script.setInterval = _setIntervalDelegate;
            engine.Script.clearInterval = _clearTimeoutDelegate;
        }

    }
}
