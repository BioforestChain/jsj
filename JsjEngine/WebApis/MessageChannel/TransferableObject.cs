using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.WebApis.MessageChannel
{
    public abstract class TransferableObject : ITransferable
    {
        private JsjRuntime _context;

        public TransferableObject(JsjRuntime context)
        {
            _context = context;
        }
        public JsjRuntime Context  => _context;

        public virtual void TransferTo(JsjRuntime context)
        {
            _context = context;
        }
    }
}
