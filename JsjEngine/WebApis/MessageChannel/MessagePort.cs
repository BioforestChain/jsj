using JsjEngine.WebApis.Event;
using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.WebApis.MessageChannel
{
    public class MessagePort: TransferableObject
    {
        public MessagePort(JsjEngine engine) :base(engine)
        {
        }

        public void PostMessage(string data)
        {
            //https://nodejs.org/api/worker_threads.html#workerparentport parentPort only makes sense in a "worker" thread
            if (!EngineContext.IsMainThread)
            {
                var @event = new Event.Event(EventType.Triggered);
                @event.HandlerContainer = EngineContext.Worker;
                @event.Data = data;
                EngineContext.PostMessage(@event);
            }
           
        }


    }
}
