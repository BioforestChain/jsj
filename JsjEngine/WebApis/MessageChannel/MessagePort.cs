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
        public MessagePort(JsjRuntime context) :base(context)
        {
        }

        public void PostJson(string json)
        {
            //https://nodejs.org/api/worker_threads.html#workerparentport parentPort only makes sense in a "worker" thread
            if (!Context.IsMainThread)
            {
                Context.PostJson(json, Context.Worker);
            }
           
        }


    }
}
