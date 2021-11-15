using JsjEngine.WebApis.Event;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using System.Collections.Concurrent;

namespace JsjEngine
{
    public partial class JsjEngine : IDisposable
    {
        private readonly Func<V8ScriptEngine> _engineFactory = ()=> { return new V8ScriptEngine(); };
        private readonly ConcurrentQueue<Event> _eventQueue = new ConcurrentQueue<Event>();
        private readonly ConcurrentDictionary<int,Timer> _timers = new ConcurrentDictionary<int,Timer>();
        private Thread? _mainThread = null;
        private int _mainThreadRunningSemaphore = 0;
        private bool _engineRunning = true;
        private V8ScriptEngine _engine;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ScriptObject _worker;
        #region constructors
        public JsjEngine(Func<V8ScriptEngine>? engineFactory = null)
        {
            if(engineFactory != null)
            {
                _engineFactory = engineFactory;
            }
            _engine = _engineFactory();

            InitEngine();
        }
        public JsjEngine(JsjEngine parent, ScriptObject worker)
        {
            _engine = _engineFactory();
            _worker = worker;
            InitEngine(parent);
        }
        #endregion

        public V8ScriptEngine ScriptEngine => _engine;

        public JsjEngine CreateChild(ScriptObject worker) => new JsjEngine(this,worker);

        #region entries
        public void ExecuteScriptSource(string source, bool block = false)
        {
            if (!IsMainThreadRunning)
            {
                Start();
            }

            var @event = new Event(EventType.EvaluateScript);
            @event.Function = source;
            _eventQueue.Enqueue(@event);

            if (block)
            {
                Wait();
                Stop();
            }

        }
        public void ExecuteScriptFile(string fileUrl)
        {
            //todo: relative path 
            //download file, get source and execute source.
            var scriptSource = _httpClient.GetStringAsync(fileUrl).Result;//question: download js file, blocking or non-blocking?
            ExecuteScriptSource(scriptSource);
        }
        #endregion

        #region main control
        public bool Start()
        {
            if (IsMainThreadRunning)
            {
                return false;
            }
            else
            {
                Interlocked.Increment(ref _mainThreadRunningSemaphore);
                _mainThread = new Thread(new ThreadStart(RunEventLoop)) { Name = "JsjEngine.ExecutionThread" };
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
        public void Wait()
        {
            while (_eventQueue.Count > 0)
            {
                Thread.Sleep(100);
            }
        }
     
        #endregion

        #region private methods
        private void InitEngine(JsjEngine parent = null)
        {
            RegisterModuleApis();
            RegisterTimerApis();
            RegisterWorkerApis(parent);
        }
        private void RunEventLoop()
        {

            while (_engineRunning)
            {
                Event @event;
                while (_eventQueue.TryDequeue(out @event))
                {
                    HandleEvent(@event);
                }

                Thread.Sleep(32);

            }

            Interlocked.Decrement(ref _mainThreadRunningSemaphore);
        }
        private void HandleEvent(Event @event)
        {
            switch (@event.Type)
            {

                case EventType.EvaluateScript:
                    {
                        Evaluate(@event.Function);
                        break;
                    }
                case EventType.TimeOut:
                    {
                        ExecuteCallBack(@event.ScriptObject);
                        DisposeTimer(@event.Id);//for settimeout callbacks, once the callback is excuted, the timer should be GCed immediately.
                        break;
                    }
                case EventType.Interval:
                    {
                        ExecuteCallBack(@event.ScriptObject);
                        break;
                    }
                case EventType.Triggered:
                default:
                    {
                        HandleTriggeredEvent(@event);
                        break;
                    }
            }
        }



        private bool IsMainThreadRunning
        {
            get { return _mainThreadRunningSemaphore > 0; }
        }
        #endregion

        public void Dispose()
        {
            Stop();
            //_engine.DocumentSettings.Loader.DiscardCachedDocuments();
            _engine.Dispose();
            foreach(var timerId in _timers.Keys.ToList())
            {
               DisposeTimer(timerId);
            }
        }
    }
}