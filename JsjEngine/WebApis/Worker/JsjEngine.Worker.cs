using JsjEngine.WebApis.Event;
using JsjEngine.WebApis.MessageChannel;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine
{
    /// <summary>
    /// web workers imlementation
    /// </summary>
    public partial class JsjEngine
    {
        private void HandleTriggeredEvent(Event @event)
        {
            var eventHandler = GetTriggeredEventHandler(@event);
            if (eventHandler != null)
            {
                var data = ParseJson(@event.Data);
                eventHandler.Invoke(false, data);
            }
            //events without handlers registered are simply discarded.
        }

        private ScriptObject? GetTriggeredEventHandler(Event @event)
        {
            ScriptObject? eventHandler = null;
            if (@event.HandlerContainer != null)
            {
                eventHandler = @event.HandlerContainer.GetProperty("onmessage") as ScriptObject;
            }

            if(eventHandler == null)
            {
                //message sent by "worker.postMessage(data)" in the "parent context" is handled within the "worker context" by "onmessage=function(data){}"
                eventHandler = _v8Engine.Script["onmessage"] as ScriptObject;
            }

            return eventHandler;
        }
        private object ParseJson(string json) => _v8Engine.Script.JSON.parse(json);
        public void PostMessage(string data, ScriptObject? sourceWorker = null)
        {
            var @event = new Event(EventType.Triggered);
            @event.HandlerContainer = sourceWorker;
            @event.Data = data;
            _eventQueue.Enqueue(@event);
        }
        public void PostMessage(Event @event)
        {
            _eventQueue.Enqueue(@event);
        }
        public void OnObject(ITransferable transferable)
        {
            transferable.TransferTo(this);
        }
        private void RegisterWorkerApis(JsjEngine parent = null)
        {
            _v8Engine.DocumentSettings.AddSystemDocument(string.Format(JsjEngineConstants.SystemModuleSpecifierFormat, "worker")
            , ModuleCategory.Standard
            , workerModuleCode
            , _ => CreateWorkerModuleContext(this, parent)
            );
        }
        private Func<JsjEngine, JsjEngine, Dictionary<string, object>> CreateWorkerModuleContext => (currentEngine, parentEngine) =>
        {
            var moduleContext = new Dictionary<string, object> { { "WorkerImpl", currentEngine }, { "isMainThread", IsMainThread } };
            //message sent from the "worker context" to the "parent context", should be handled by the "parent"
            //so the message is pushed to the "parent's queue",
            //later it will be handled in the "parent context" with "worker.onmessage = function(data){}" syntax
            //thus we kept the track of the "original" worker here
            if (parentEngine != null)
            {
                moduleContext.Add("postMessage", new Action<object>(data => parentEngine.PostMessage(_v8Engine.Script.JSON.stringify(data), _v8Worker)));
            }
            return moduleContext;
        };
        private string workerModuleCode = @"
            import {MessageChannel} from '@jsj/system/channel';
            let context = import.meta;
            const { port2 } = new MessageChannel();
            export function Worker(workLoad)
            {
                const childImpl = context.WorkerImpl.CreateChild(this);
                this.postMessage = data => childImpl.PostMessage(JSON.stringify(data));
                this.terminate = childImpl.Dispose;
                if (typeof(workLoad) === 'string')
                {
                    childImpl.ExecuteScriptFile(workLoad);
                }
            };
            export function postMessage()
            {
                if (context.postMessage)
                {
                    context.postMessage(...arguments);
                }
            }
            export let parentPort = port2;
            export let isMainThread = context.isMainThread;
        ";
    }
}
