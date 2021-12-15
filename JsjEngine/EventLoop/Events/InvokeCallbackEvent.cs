using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.EventLoop.Events
{
    internal class InvokeCallbackEvent : IEvent
    {
        private readonly ScriptObject _callback;

        public InvokeCallbackEvent(ScriptObject callback)
        {
            _callback = callback;
        }
        async Task<bool> IEvent.HandleAsync(JsjRuntime runtime)
        {
            try
            {
                await runtime.InvokeFuncAsync(_callback);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in InvokeCallback event handler: " + exception);
                return true;//todo: once exception handling module implemented, this should return false to terminate the event loop.
            }
        }
    }
}
