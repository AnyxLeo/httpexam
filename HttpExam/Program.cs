using EmbedIO;
using EmbedIO.Actions;
using Swan.Logging;
using System;

namespace httpexam
{
    public class Program
    {
        public IWebServer WebServer { get; private set; }

        public static void Main(string[] args)
        {
            var url = "http://localhost:9696/";
            if (args.Length > 0)
                url = args[0];

            // Our web server is disposable.
            using (var server = LoadWebServer(url))
            {
                // Once we've registered our modules and configured them, we call the RunAsync() method.
                server.RunAsync();

                var browser = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true }
                };
                browser.Start();
                // Wait for any key to be pressed before disposing of our web server.
                // In a service, we'd manage the lifecycle of our web server using
                // something like a BackgroundWorker or a ManualResetEvent.
                Console.ReadKey(true);
            }
        }

        private static WebServer LoadWebServer(string url)
        {
           var server = new WebServer( s => s
           .WithUrlPrefix(url)
           .WithMode(HttpListenerMode.EmbedIO))
                // First, we will configure our web server by adding Modules.
                .WithModule(new CustomModule("/"))
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));

            // Listen for state changes.
            server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

            return server;

        }
    }
}
