using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.EventLoop
{
    internal interface IEvent
    {
        Task<bool> HandleAsync(JsjRuntime runtime);
    }
}
