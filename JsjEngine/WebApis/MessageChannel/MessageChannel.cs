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
        private readonly JsjEngine _engine;
        public MessageChannel(JsjEngine engine)
        {
            _engine = engine;
            _port1 = new MessagePort(engine);
            _port2 = new MessagePort(engine);
        }


        public JsjEngine Engine => _engine;
        public MessagePort Port1 => _port1;
        public MessagePort Port2 => _port2;
    }
}
