using JsjEngine.WebApis.Event;
using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine
{
    public partial class JsjEngine
    {
        private void HandleTriggeredEvent(Event @event)
        {
            var funcName =@event.Function;
            var sourceWorker = @event.ScriptObject;
            ScriptObject? eventHandler = null;

            if (sourceWorker != null)
            {
                //message sent by "postMessage(data)" in the "worker context" is handled within the "parent context" by "worker.onmesage=functoin(data){}"
                eventHandler = sourceWorker.GetProperty(funcName) as ScriptObject;
            }

            if (eventHandler == null)
            {
                //message sent by "worker.postMessage(data)" in the "parent context" is handled within the "worker context" by "onmessage=function(data){}"
                eventHandler = _engine.Script[funcName] as ScriptObject;
            }

            if (eventHandler != null)
            {
                var data = ParseJson(@event.Data);
                eventHandler.Invoke(false, data);
            }
        }
        public object ParseJson(string json) => _engine.Script.JSON.parse(json);
        public void PostMessage(string data, ScriptObject sourceWorker = null)
        {

            var @event = new Event(EventType.Triggered);
            @event.Function = "onmessage";
            @event.ScriptObject = sourceWorker;
            @event.Data = data;
            _eventQueue.Enqueue(@event);
               
        }
        private void RegisterWorkerApis(JsjEngine parent = null)
        {
            ((ScriptObject)_engine.Evaluate(@"(function (impl) {
                Worker = function (workLoad) {
                    const childImpl = impl.CreateChild(this);
                    this.postMessage = data => childImpl.PostMessage(JSON.stringify(data));
                    this.terminate = childImpl.Dispose;
                    if (typeof(workLoad) === 'string') {
                        childImpl.ExecuteScriptFile(workLoad);
                    }else{
                        //todo: other workload format
                    }
                }
            })")).Invoke(false, this);

            if (parent != null)
            {
                //message sent from the "worker context" to the "parent context", should be handled by the "parent"
                //so the message is pushed to the "parent's queue",
                //later it will be handled in the "parent context" with "worker.onmessage = function(data){}" syntax
                //thus we kept the track of the "original" worker here
                _engine.Script.postMessage = new Action<object>(data => parent.PostMessage(_engine.Script.JSON.stringify(data), _worker));
            }

        }
    }
}
