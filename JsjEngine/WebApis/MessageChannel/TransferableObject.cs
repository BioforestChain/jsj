using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.WebApis.MessageChannel
{
    public abstract class TransferableObject : ITransferable
    {
        private JsjEngine _engine;

        public TransferableObject(JsjEngine engine)
        {
            _engine = engine;
        }
        public JsjEngine EngineContext  => _engine;

        public virtual void TransferTo(JsjEngine context)
        {
            _engine = context;
        }
    }
}
