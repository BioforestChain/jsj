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


     
        private void RegisterMessageChannelApis(JsjEngine parent = null)
        {
            _v8Engine.DocumentSettings.AddSystemDocument(string.Format(JsjEngineConstants.SystemModuleSpecifierFormat, "channel")
            , ModuleCategory.Standard
            , messageChannelModuleCode
            , _ => CreateMessageChannelModuleContext(this)
            );
        }
        private Func<JsjEngine, Dictionary<string, object>> CreateMessageChannelModuleContext => (currentEngine) =>
        {
            var initialMessageChannel = new MessageChannel(currentEngine);
            var moduleContext = new Dictionary<string, object> {{ "MessageChannelImpl", initialMessageChannel } };
            return moduleContext;
        };
        private const string messageChannelModuleCode = @"
            let context = import.meta;
            let channelImpl = context.MessageChannelImpl;
            function MessagePort(portImpl) {
                this.postMessage = data => portImpl.PostMessage(JSON.stringify(data));
            }
            export function MessageChannel() {
                this.port1 = new MessagePort(channelImpl.Port1);
                this.port2 = new MessagePort(channelImpl.Port2);
            };
        ";
    }
}
