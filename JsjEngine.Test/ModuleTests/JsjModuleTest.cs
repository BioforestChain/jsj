using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JsjEngine.Test.ModuleTests
{
    [TestClass]
    public class JsjModuleTest: JsjEngineTest
    {
        [TestMethod]
        public void Should_Load_Timer_Apis()
        {
            Exception exception = null;
            var script = System.IO.File.ReadAllText("../../../ModuleTests/js/timer-api-test.js");
            try
            {
                Engine.ExecuteScriptSource(script);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNull(exception);
        }
    }
}