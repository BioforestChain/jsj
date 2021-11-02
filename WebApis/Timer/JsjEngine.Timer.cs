using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsjEngine.WebApis.Event;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace JsjEngine
{
    /// <summary>
    /// timer related web api imlementation
    /// </summary>
    public partial class JsjEngine
    {
        private object ExecuteCallBack(ScriptObject callBackFunction, params object[] args)
        {
            var result = callBackFunction.Invoke(false, args);
            return result;
        }
        private object Evaluate(string source)
        {
            var result = ScriptEngine.Evaluate(source);
            return result;
        }
        private Func<ScriptObject, int, int> _setTimeoutDelegate => (func, delay) => {
           
            return EnqueueCallBackEvent(func,delay, EventType.TimeOut);
        };
        private Action<int> _clearTimeoutDelegate => (id) =>
        {
            DisposeTimer(id);
        };
        private void DisposeTimer(int timerId) {
            Timer? timer;
            if(_timers.TryRemove(timerId, out timer))
            {
                timer.Dispose();
            }
        }
        private Func<ScriptObject, int, int> _setIntervalDelegate => (func, delay) => {
            return EnqueueCallBackEvent(func, delay, EventType.Interval);
        };

        private int EnqueueCallBackEvent(ScriptObject callBack, int delay, EventType eventType)
        {
            var @event = new Event(eventType);
            @event.ScriptObject = callBack;
            var timerId = @event.Id;//todo: other id?
            var dueTime = eventType == EventType.Interval ? 0 : delay;
            var period = eventType == EventType.Interval ? delay : Timeout.Infinite;
            var timer = new Timer(_ => _eventQueue.Enqueue(@event), null, dueTime, period);
            _timers.TryAdd(timerId, timer);
            return timerId;
        }
        private void RegisterTimerApis()
        {
            _engine.Script.setTimeout = _setTimeoutDelegate;
            _engine.Script.clearTimeout = _clearTimeoutDelegate;
            _engine.Script.setInterval = _setIntervalDelegate;
            _engine.Script.clearInterval = _clearTimeoutDelegate;
        }

    }
}
