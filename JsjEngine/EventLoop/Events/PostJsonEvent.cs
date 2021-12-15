using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.EventLoop.Events
{
    internal class PostJsonEvent : IEvent
    {
        private readonly ScriptObject? _sourceWorker;
        private readonly string _json;

        public PostJsonEvent(string json, ScriptObject? sourceWorker=null)
        {
            _json = json;
            _sourceWorker = sourceWorker;
        }
        async Task<bool> IEvent.HandleAsync(JsjRuntime runtime)
        {
            try
            {
                var data = runtime.ParseJson(_json);
                await runtime.InvokeEventHandlerAsync("message", _sourceWorker, data);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in PostJson event handler: " + exception);
                return true;//todo: once exception handling module implemented, this should return false to terminate the event loop.
            }
        }
    }
}
