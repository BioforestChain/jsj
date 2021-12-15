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
    /// message channel implementation
    /// </summary>
    public partial class JsjRuntime
    {
        public bool IsTransferable(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            if (obj is ITransferable)
            {
                return true;
            }
            else
            {
                //todo: check other v8 built-in types
            }
            return false;
        }
        private void RegisterMessageChannelApis(JsjRuntime parent = null)
        {
            _v8Engine.DocumentSettings.AddSystemDocument(string.Format(JsjEngineConstants.SystemModuleSpecifierFormat, "channel")
            , ModuleCategory.Standard
            , messageChannelModuleCode
            , _ => CreateMessageChannelModuleContext(this)
            );
        }
        private Func<JsjRuntime, Dictionary<string, object>> CreateMessageChannelModuleContext => (currentEngine) =>
        {
            var initialMessageChannel = new MessageChannel(currentEngine);
            var moduleContext = new Dictionary<string, object> {
                { "MessageChannelImpl", initialMessageChannel },
                { "IsTransferable", new Func<bool,object>(obj => IsTransferable(obj)) }, 
            };
            return moduleContext;
        };
        private const string messageChannelModuleCode = @"
           let context = import.meta;
           export let MessageChannelImpl = context.MessageChannelImpl;
           export let IsTransferable = context.IsTransferable;
        ";
    }
}
