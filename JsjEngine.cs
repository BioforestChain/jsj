using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace JsjEngine
{
    public partial class JsjEngine
    {
        private readonly CallBackEventQueue _eventQueue = new CallBackEventQueue();
        private Thread? _mainThread = null;
        private int _mainThreadRunningSemaphore = 0;
        private bool _engineRunning = true;
        private V8ScriptEngine? _engine = null;

        public V8ScriptEngine ScriptEngine
        {
            get
            {
                if (_engine == null)
                    _engine = GetV8ScriptEngine();
                return _engine;
            }
        }

        public void AddHostType(string name, Type type)
        {
            ScriptEngine.AddHostType(name, type);
        }

        public void AddHostObject(string name, object obj)
        {
            ScriptEngine.AddHostObject(name, obj);
        }

        public object Evaluate(string source)
        {
            var result = ScriptEngine.Evaluate(source);
            return result;
        }

        public void ClearQueue()
        {
            _eventQueue.Enqueue(new CallBackEvent(CallBackType.ClearQueue));
        }

        public bool ExecuteScript(string source, bool block = false)
        {
            if (!IsMainThreadRunning)
            {
                Start();
            }

            _eventQueue.ExecuteScript(source);

            if (block)
            {
                Wait();
                Stop();
            }

            return true;
        }
       
        public bool Start()
        {
            if (IsMainThreadRunning)
            {
                return false;
            }
            else
            {
                Interlocked.Increment(ref _mainThreadRunningSemaphore);
                _mainThread = new Thread(new ThreadStart(RunEventLoop)) { Name = "JsjEngine.BackgroundExecutionThread" };
                _engineRunning = true;
                _mainThread.Start();
                return true;
            }
        }

        public void Stop(Action sleepMethod = null)
        {
            if (_mainThread != null)
            {
                _engineRunning = false;
                while (_mainThread.IsAlive)
                {
                    if (sleepMethod == null)
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        sleepMethod();
                    }
                }
                _mainThread = null;
            }
        }
        public void Kill()
        {
            if (_mainThread != null)
            {
                _engineRunning = false;
                _mainThread.Abort();
                _mainThread = null;
                Thread.Sleep(100);
            }
        }
        public void Wait()
        {
            while (_eventQueue.Count > 0)
            {
                Thread.Sleep(100);
            }
        }
        public void Reset()
        {
            Stop();
            _engine = null;
        }
        #region private methods

        private V8ScriptEngine GetV8ScriptEngine()
        {
            var engine = new V8ScriptEngine();
            engine.AddHostType("Console", typeof(Console));
            RegisterTimerApis(engine);
            return engine;
        }

        private object ExecuteCallBack(ScriptObject callBackFunction, params object[] args)
        {
            var result = callBackFunction.Invoke(false, args);
            return result;
        }
        private void RunEventLoop()
        {
            const int sleepTime = 32;

            while (_engineRunning)
            {
                if (_eventQueue.Count > 0)
                {
                    // Execute first all events with no delay
                    var tempQueue = _eventQueue.GetEventsWithNoDelay();
                    while (tempQueue.Count > 0)
                    {
                        ExecuteEvent(tempQueue.Peek(), tempQueue);
                    }

                    // Deal with timer event now
                    Thread.Sleep(sleepTime); // Sleep minimal time
                    tempQueue = _eventQueue.CloneEventWithDelay();
                    foreach (var @event in tempQueue)
                    {
                        if (@event.ReadyForExecution(sleepTime))
                        {
                            ExecuteEvent(@event, null);
                        }
                    }
                    _eventQueue.RemoveDisabled();
                }
                else
                {
                    Thread.Sleep(sleepTime);
                }
            }
            Interlocked.Decrement(ref _mainThreadRunningSemaphore);
        }
        private void ExecuteEvent(CallBackEvent @event, CallBackEventQueue tempQueue)
        {
            if (@event.Enabled)
            {
                @event.Enabled = false;
                switch (@event.Type)
                {
                    case CallBackType.ClearQueue:
                        {
                            _eventQueue.Clear();
                            break;
                        }
                    case CallBackType.EvaluateScript:
                        {
                            Evaluate(@event.Source);
                            break;
                        }
                    case CallBackType.TimeOut:
                        {
                            ExecuteCallBack(@event.Function);
                            break;
                        }
                    case CallBackType.Interval:
                        {
                            @event.Enabled = true;
                            ExecuteCallBack(@event.Function);
                            break;
                        }
                }
            }

            _eventQueue.RemoveExecuted(@event, true);

            if (tempQueue != null)
            {
                tempQueue.RemoveExecuted(@event, false);
            }
        }
        private bool IsMainThreadRunning
        {
            get { return _mainThreadRunningSemaphore > 0; }
        }
        #endregion


    }
}