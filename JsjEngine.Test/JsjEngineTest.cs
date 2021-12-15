using Microsoft.ClearScript.V8;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.Test
{
    [TestClass]
    public abstract class JsjEngineTest
    {
        private JsjRuntime _engine;
        public TestContext TestContext { get; set; }

        public JsjRuntime Engine
        {
            get { return _engine; }
            set { _engine = value; }
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            var _v8EngineFactory = () =>
            {
                var v8Engine = new V8ScriptEngine();
                v8Engine.AddHostType("Console", typeof(Console));

                var libPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\JsjEngine.JavascriptLib");
                v8Engine.DocumentSettings.SearchPath = string.Join(";", libPath);
                return v8Engine;
            };

            _engine = new JsjRuntime(_v8EngineFactory);



        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            _engine.Dispose();
        }
    }
}
