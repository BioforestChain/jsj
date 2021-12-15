using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.WebApis.MessageChannel
{
    public class MessageChannel
    {
        private readonly MessagePort _port1;
        private readonly MessagePort _port2;
        private readonly JsjRuntime _runtime;
        public MessageChannel(JsjRuntime runtime)
        {
            _runtime = runtime;
            _port1 = new MessagePort(runtime);
            _port2 = new MessagePort(runtime);
        }


        public JsjRuntime Runtime => _runtime;
        public MessagePort Port1 => _port1;
        public MessagePort Port2 => _port2;
    }
}
