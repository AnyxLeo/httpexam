using EmbedIO;
<<<<<<< HEAD
using EmbedIO.Actions;
using EmbedIO.Routing;
using Swan.Formatters;
using Swan.Logging;
=======
using EmbedIO.Routing;
using Swan.Formatters;
>>>>>>> b4a4f57564fdf3d99bc42b82d8fb281572c41343
using Swan.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace httpexam
{
<<<<<<< HEAD
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
=======
    public class Program 
    {
        public IWebServer WebServer { get; private set; }

        static void Main(string[] args)
        {
            Register();
        }

        private void Register()
        {
            WebServer = new WebServer("https://localhost:8080")
               .WithWebApi()
>>>>>>> b4a4f57564fdf3d99bc42b82d8fb281572c41343
        }
    }
}
