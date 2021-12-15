using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsjEngine.EventLoop.Events;
using JsjEngine.WebApis.Timer;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace JsjEngine
{
    /// <summary>
    /// timer related web api implementation
    /// </summary>
    public partial class JsjRuntime
    {

        public Func<ScriptObject, int, bool, int> InsertTimerCallback => (func, delay, repeated) =>
        {
            var timeoutCallback = new TimeoutCallback(func, delay, repeated);
            lock (_timeoutCallbacksLocker)
            {
                _timeoutCallbacks.Add(timeoutCallback);
            }
            return timeoutCallback.Id;
        };
       
        public Action<int> RemoveTimerCallback => (id) =>
        {
            lock (_timeoutCallbacksLocker)
            {
                var callback = _timeoutCallbacks.FirstOrDefault(it => it.Id == id);
                if(callback != null)
                {
                    _timeoutCallbacks.Remove(callback);
                }
            }
        };

        private void GenerateTimeoutCallBackEvents()
        {
            var now = DateTime.Now;
            IList<TimeoutCallback> duedCallbacks;
            lock (_timeoutCallbacksLocker)
            {
                 duedCallbacks = _timeoutCallbacks.Where(it => it.DueTime <= now).ToList();
            }
            foreach (var callback in duedCallbacks)
            {
                var callbackEvent = new InvokeCallbackEvent(callback.CallbackFunction);
                _eventQueue.Enqueue(callbackEvent);//todo: batch enqueue?
                if (callback.Repeated)
                {
                    callback.DueTime = now.AddMilliseconds(callback.Delay); //setInterval
                }
                else
                {
                    callback.Disabled = true;//setTimeout
                }
            }
            lock (_timeoutCallbacksLocker)
            {
                foreach(var disabledCallback in duedCallbacks.Where(it => it.Disabled))
                {
                    _timeoutCallbacks.Remove(disabledCallback); 
                }
            }
        }

        private void RegisterTimerApis()
        {
            _timeoutCallBackTimer = new Timer(_ => GenerateTimeoutCallBackEvents(), null, 0, 40);
            _v8Engine.DocumentSettings.AddSystemDocument(string.Format(JsjEngineConstants.SystemModuleSpecifierFormat,"timer")
                , ModuleCategory.Standard
                , timerModuleCode
                , _ => new Dictionary<string, object> {
                        { "InsertTimerCallback",InsertTimerCallback },
                        { "RemoveTimerCallback",RemoveTimerCallback },
                    });
        }

        private const string timerModuleCode = @"
            let context = import.meta;
            export let InsertTimerCallback = context.InsertTimerCallback;            
            export let RemoveTimerCallback = context.RemoveTimerCallback; 
        ";

    }
}
