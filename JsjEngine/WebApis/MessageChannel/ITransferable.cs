using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.WebApis.MessageChannel
{
    public interface ITransferable
    {
        public JsjEngine EngineContext { get; }
        public void TransferTo(JsjEngine engine);
        public bool IsTransferable
        {
            get
            {
                //todo:
                return true;
            }
        }
    }
}
