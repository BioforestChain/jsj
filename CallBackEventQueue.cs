using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript;

namespace JsjEngine
{
    internal class CallBackEventQueue : IEnumerable<CallBackEvent>
    {
        private static readonly object obj = new();
        private List<CallBackEvent> _queue = new List<CallBackEvent>();

        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<CallBackEvent> GetEnumerator()
        {
            return ((IEnumerable<CallBackEvent>)_queue).GetEnumerator();
        }

        public int Count
        {
            get { return _queue.Count; }
        }

        public CallBackEventQueue Clone()
        {
            lock (obj)
            {
                var queue = new CallBackEventQueue();
                foreach (var @event in _queue)
                {
                   _queue.Add(@event); 
                }
                return queue;
            }
        }
        public void RemoveDisabled()
        {
            lock (obj)
            {
                var goOn = true;
                while (goOn)
                {
                    goOn = false;
                    foreach (var @event in _queue)
                    {
                        if (@event.Disabled)
                        {
                            _queue.Remove(@event);
                            goOn = true;
                            break;
                        }
                    }
                }
            }
        }
        public CallBackEventQueue GetEventsWithNoDelay()
        {
            lock (obj)
            {
                var queue = new CallBackEventQueue();
                foreach (var @event in _queue)
                {
                    if (@event.Delay == 0 && @event.Enabled)
                    {
                        queue.EnqueueNotSafe(@event);
                    }
                }
                return queue;
            }
        }
        public CallBackEventQueue CloneEventWithDelay()
        {
            lock (obj)
            {
                var queue = new CallBackEventQueue();
                foreach (var @event in _queue)
                {
                    if (@event.Delay > 0 && @event.Enabled)
                    {
                        queue.EnqueueNotSafe(@event);
                    }
                }
                return queue;
            }
        }
        public void Clear()
        {
            // No need to lock because this method is only called by
            // the event loop
            _queue.Clear();
        }

        public void ExecuteScript(string source)
        {
            lock (obj)
            {
                //entry poist of each egine instance
                _queue.Insert(0, new CallBackEvent(source));
            }
        }
        public void EndqueueCallBackExecution(ScriptObject function, double delay, CallBackType type)
        {
            lock (obj)
            {
                _queue.Add(new CallBackEvent(function, delay, type));
            }
        }
        public CallBackEvent Enqueue(CallBackEvent @event)
        {
            lock (obj)
            {
                _queue.Add(@event);
            }
            return @event;
        }
        internal void EnqueueNotSafe(CallBackEvent @event)
        {
            _queue.Add(@event);
        }
        public CallBackEvent Dequeue()
        {
            lock (obj)
            {
                var @event = _queue[0];
                _queue.RemoveAt(0);
                return @event;
            }
        }
        public CallBackEvent Peek()
        {
            lock (obj)
            {
                return _queue[0];
            }
        }
        public void ClearCallBackEvent(int id)
        {
            lock (obj)
            {
                var @event = _queue.FirstOrDefault(e => e.Id == id);
                if (@event == null)
                {
                    throw new ArgumentException(string.Format("Cannot find timeout or interval id {0}", id));
                }
                else
                {
                    @event.Disable();
                }
            }
        }
        public void RemoveExecuted(CallBackEvent @event, bool mainQueue)
        {
            lock (obj)
            {
                var toRemove = _queue.Find(e => e == @event);
                if (toRemove != null)
                {
                    _queue.Remove(toRemove);
                }

                if (mainQueue)
                {
                    if (@event.Type == CallBackType.Interval) // for Interval we re add the event at then end of the queue
                    {
                        Enqueue(@event);
                    }
                }
            }
        }

    }
}
