using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.EventLoop.Events
{
    internal class ExitEvent : IEvent
    {
        async Task<bool> IEvent.HandleAsync(JsjRuntime runtime)
        {
            try
            {
                await runtime.InvokeEventHandlerAsync("Exit", null);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in Exit event handler: " + exception);
            }

            return false;
        }
    }
}
