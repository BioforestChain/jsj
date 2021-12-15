using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.EventLoop
{
    internal class AsyncQueue<T>
    {
        private readonly object _locker = new object();
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly Queue<TaskCompletionSource<T>> _waiters = new Queue<TaskCompletionSource<T>>();

        public void Enqueue(T @event)
        {
            lock (_locker)
            {
                if (_waiters.TryDequeue(out var waiter))
                {
                    waiter.SetResult(@event);
                }
                else
                {
                    _queue.Enqueue(@event);
                }
            }
        }

        public Task<T> DequeueAsync()
        {
            lock(_locker)
            {
                if(_queue.TryDequeue(out var @event))
                {
                    return Task.FromResult(@event);
                }

                var waiter = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
                _waiters.Enqueue(waiter);
                return waiter.Task;
            }
        }


    }
}
