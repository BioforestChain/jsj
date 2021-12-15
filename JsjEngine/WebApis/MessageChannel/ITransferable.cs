using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.WebApis.MessageChannel
{
    public interface ITransferable
    {
        public JsjRuntime Context { get; }
        public void TransferTo(JsjRuntime context);
       
    }
}
