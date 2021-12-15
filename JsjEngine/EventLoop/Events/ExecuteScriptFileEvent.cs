using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.EventLoop.Events
{
    internal class ExecuteScriptFileEvent : IEvent
    {
        private readonly string _url;
        private static readonly HttpClient _httpClient = new HttpClient();
        public ExecuteScriptFileEvent(string url) => _url = url;

        async Task<bool> IEvent.HandleAsync(JsjRuntime runtime)
        {
            try
            {
                if (!string.IsNullOrEmpty(_url))
                {
                    string scriptSource;
                    if (_url.StartsWith("http://") || _url.StartsWith("https://"))
                    {
                        scriptSource = await _httpClient.GetStringAsync(_url);
                    }
                    else
                    {
                        scriptSource = await File.ReadAllTextAsync(_url);
                    }
                    //todo: multiple ways to get script source code from a url(download from web, get from database... etc.)

                    runtime.ExecuteCode(scriptSource);
                }

                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception in ExecuteScriptFile event handler: " + exception);
                return true;//todo: once exception handling module implemented, this should return false to terminate the event loop.

            }
        }
    }
}
