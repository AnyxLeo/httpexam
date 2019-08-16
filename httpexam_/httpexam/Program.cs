using EmbedIO;
using EmbedIO.Routing;
using Swan.Formatters;
using Swan.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace httpexam
{
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
        }
    }
}
