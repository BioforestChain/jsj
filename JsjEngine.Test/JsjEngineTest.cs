using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.Test
{
    [TestClass]
    public abstract class JsjEngineTest
    {
        private JsjEngine _engine;
        public TestContext TestContext { get; set; }

        public JsjEngine Engine
        {
            get { return _engine; }
            set { _engine = value; }
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _engine = new JsjEngine();
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            _engine.Dispose();
        }
    }
}
