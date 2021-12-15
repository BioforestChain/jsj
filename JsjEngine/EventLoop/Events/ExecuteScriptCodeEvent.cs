using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.EventLoop.Events
{
    internal class ExecuteScriptCodeEvent : IEvent
    {
        private readonly string _code;
        public ExecuteScriptCodeEvent(string code) => _code = code;


        Task<bool> IEvent.HandleAsync(JsjRuntime runtime)
        {
            try
            {
                runtime.ExecuteCode(_code);
                return Task.FromResult(true);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in ExecuteScriptCode event handler: " + exception);
                return Task.FromResult(true);//todo: once exception handling module implemented, this should return false to terminate the event loop.

            }
        }
    }
}
