using JsjEngine;
using Microsoft.ClearScript.V8;

class Program
{
    static void Main()
    {
        Console.WriteLine("test started...");
        var runtime = CreateRuntime();

        //var script = GetTimerTestJsPath();
        var script = GetWorkerTestJsPath();
        runtime.PostExecuteScriptCode(script);
        runtime.WaitForExitAsync().Wait();
        Console.ReadLine();
    }

    private static JsjRuntime CreateRuntime()
    {
        var _v8EngineFactory = () =>
        {
            var v8Engine = new V8ScriptEngine();

            var libPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\JsjEngine.JavascriptLib");
            v8Engine.DocumentSettings.SearchPath = string.Join(";", libPath);
            return v8Engine;
        };

        return new JsjRuntime(_v8EngineFactory);
    }

    private static string GetTimerTestJsPath()
    {
       return System.IO.File.ReadAllText("../../../jsTest/timer/timer.js");
    }

    private static string GetWorkerTestJsPath()
    {
        return System.IO.File.ReadAllText("../../../jsTest/worker/js-file.js");
    }
}