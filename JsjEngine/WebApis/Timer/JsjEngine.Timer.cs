using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsjEngine.WebApis.Event;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
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
            var result = ScriptEngine.Evaluate(new DocumentInfo { Category = ModuleCategory.Standard }, source);
            return result;
        }
        public Func<ScriptObject, int, int> SetTimeout => (func, delay) =>
        {
            return EnqueueCallBackEvent(func, delay, EventType.TimeOut);
        };
        public Func<ScriptObject, int, int> SetInterval => (func, delay) =>
        {
            return EnqueueCallBackEvent(func, delay, EventType.Interval);
        };
        public Action<int> ClearTimer => (id) =>
        {
            DisposeTimer(id);
        };

        private void DisposeTimer(int timerId)
        {
            Timer? timer;
            if (_timers.TryRemove(timerId, out timer))
            {
                timer.Dispose();
            }
        }
        private int EnqueueCallBackEvent(ScriptObject callBack, int delay, EventType eventType)
        {
            var @event = new Event(eventType);
            @event.CallBackFunc = callBack;
            var timerId = @event.Id;
            var dueTime = eventType == EventType.Interval ? 0 : delay;
            var period = eventType == EventType.Interval ? delay : Timeout.Infinite;
            var timer = new Timer(_ => _eventQueue.Enqueue(@event), null, dueTime, period);
            _timers.TryAdd(timerId, timer);
            return timerId;
        }
        private void RegisterTimerApis()
        {
            _v8Engine.DocumentSettings.AddSystemDocument(string.Format(JsjEngineConstants.SystemModuleSpecifierFormat,"timer")
                , ModuleCategory.Standard
                , timerModuleCode
                , _ => new Dictionary<string, object> {
                        { "SetTimeout",SetTimeout },
                        { "SetInterval",SetInterval },
                        { "ClearTimer",ClearTimer },
                    });
        }

        private const string timerModuleCode = @"
            let context = import.meta;
            export function setTimeout(func,delay) {return context.SetTimeout(func,delay);}
            export function setInterval(func,delay) {return context.SetInterval(func,delay);}
            export function clearTimeout(id) {context.ClearTimer(id);}
            export function clearInterval(id) {context.ClearTimer(id);}
        ";

    }
}
