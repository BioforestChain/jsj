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
    /// web workers implementation
    /// </summary>
    public partial class JsjRuntime
    {

        private void RegisterWorkerApis(JsjRuntime parent = null)
        {
            _v8Engine.DocumentSettings.AddSystemDocument(string.Format(JsjEngineConstants.SystemModuleSpecifierFormat, "worker")
            , ModuleCategory.Standard
            , workerModuleCode
            , _ => CreateWorkerModuleContext(this, parent)
            );
        }
        private Func<JsjRuntime, JsjRuntime, Dictionary<string, object>> CreateWorkerModuleContext => (currentEngine, parentEngine) =>
        {
            var moduleContext = new Dictionary<string, object> { { "WorkerImpl", currentEngine }, { "isMainThread", IsMainThread } };
            //message sent from the "worker context" to the "parent context", should be handled by the "parent"
            //so the message is pushed to the "parent's queue",
            //later it will be handled in the "parent context" with "worker.onmessage = function(data){}" syntax
            //thus we kept the track of the "original" worker here
            if (parentEngine != null)
            {
                moduleContext.Add("PostJson", new Action<object>(data => parentEngine.PostJson(_v8Engine.Script.JSON.stringify(data), _v8Worker)));
            }
            return moduleContext;
        };
        private string workerModuleCode = @"
            let context = import.meta;
            export let isMainThread = context.isMainThread;            
            export let WorkerImpl = context.WorkerImpl;            
            export let PostJson = context.PostJson;            
        ";
    }
}
