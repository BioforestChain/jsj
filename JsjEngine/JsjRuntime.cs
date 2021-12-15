using JsjEngine.EventLoop;
using JsjEngine.EventLoop.Events;
using JsjEngine.WebApis.Timer;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Collections.Concurrent;

namespace JsjEngine
{
    public partial class JsjRuntime : IDisposable
    {
        #region private fields
        private readonly Func<V8ScriptEngine> _v8EngineFactory = ()=> { return new V8ScriptEngine(); };
        private readonly AsyncQueue<IEvent> _eventQueue = new AsyncQueue<IEvent>();
        private readonly TaskCompletionSource _exitSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly IList<TimeoutCallback> _timeoutCallbacks = new List<TimeoutCallback>();
        private readonly object _timeoutCallbacksLocker = new object();
        private Timer _timeoutCallBackTimer;
        private V8ScriptEngine _v8Engine;
        private readonly ScriptObject? _v8Worker;
        private readonly bool _isMainThread;
        #endregion

        #region constructors
        public JsjRuntime(Func<V8ScriptEngine>? engineFactory = null)
        {
            if(engineFactory != null)
            {
                _v8EngineFactory = engineFactory;
            }
            _v8Engine = _v8EngineFactory();
            _isMainThread = true;
            InitEngine();
        }
        public JsjRuntime(JsjRuntime parent, ScriptObject worker)
        {
            _v8Engine = _v8EngineFactory();
            _v8Worker = worker;
            InitEngine(parent);
        }
        #endregion

        #region public properties & fields
        public ScriptObject? Worker => _v8Worker;
        public bool IsMainThread => _isMainThread;
        #endregion


        #region public methods
        public JsjRuntime CreateChild(ScriptObject worker) => new JsjRuntime(this, worker);

        #region enqueue events
        public void PostExecuteScriptCode(string code) => _eventQueue.Enqueue(new ExecuteScriptCodeEvent(code));
        public void PostExecuteScriptFile(string fileUrl) => _eventQueue.Enqueue(new ExecuteScriptFileEvent(fileUrl));
        public void PostExit() => _eventQueue.Enqueue(new ExitEvent());
        public void PostJson(string json,ScriptObject? sourceWorker = null) => _eventQueue.Enqueue(new PostJsonEvent(json, sourceWorker));
        #endregion
        #region "host side" event handlers
        internal void ExecuteCode(string code)
        {
            _v8Engine.Execute(new DocumentInfo { Category = ModuleCategory.Standard }, code);
        }
        internal async Task InvokeEventHandlerAsync(string eventName, ScriptObject? sourceWorker, params object[] args)
        {
            var handlerFuncName = "on" + eventName;
            if (sourceWorker?.GetProperty(handlerFuncName) is ScriptObject workerFunc)
            {
                await InvokeFuncAsync(workerFunc, args);
            }
            else if (_v8Engine.Script[handlerFuncName] is ScriptObject globalFunc)
            {
                await InvokeFuncAsync(globalFunc, args);
            }
        }

        internal async Task InvokeFuncAsync(ScriptObject func, params object[] args)
        {
            var result = func.Invoke(false, args);
            if (result is Task task)
            {
                await task;
            }
        }

        #endregion



        public Task WaitForExitAsync() => _exitSource.Task;
        public void Dispose()
        {
            PostExit();
            WaitForExitAsync().Wait();
            _timeoutCallBackTimer.Dispose();
            _timeoutCallbacks.Clear();
            _v8Engine.Dispose();
        }
        #endregion
        #region private methods
        private void InitEngine(JsjRuntime? parent = null)
        {
            _v8Engine.AddHostType("Console", typeof(Console));//todo: remove this

            if (parent is not null)
            {
                _v8Engine.DocumentSettings.SearchPath = parent._v8Engine.DocumentSettings.SearchPath;
            }
            RegisterModuleApis();
            RegisterTimerApis();
            RegisterMessageChannelApis();//channel api must be registered before worker api as the later depends on the former
            RegisterWorkerApis(parent);
            Task.Run(RunEventLoop);

        }
        private async Task RunEventLoop()
        {
            while (true)
            {
                var @event = await _eventQueue.DequeueAsync();
                while (!await @event.HandleAsync(this))
                {
                    break;
                }
            }

            _exitSource.SetResult();
        }
        internal object ParseJson(string json) => _v8Engine.Script.JSON.parse(json);
        #endregion
    }
}