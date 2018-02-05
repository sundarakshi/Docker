using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DockerSample
{
    class Program
    {
        private readonly HttpListener _listener = new HttpListener();

        public Program()
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

            _listener.Prefixes.Add("http://*:5000/");
        }

        public void start()
        {
            _listener.Start();
            for (; ; )
            {
                HttpListenerContext ctx = _listener.GetContext();
                new Thread(new Worker(ctx).ProcessRequest).Start();
            }
        }

        static void Main(string[] args)
        {
            Program webServer = new Program();
            webServer.start();
        }
    }

    public class Worker
    {
        private HttpListenerContext context;

        public Worker(HttpListenerContext context)
        {
            this.context = context;
        }

        public void ProcessRequest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><body><h1>Hello World from C# and Distelli</h1>");

            byte[] b = Encoding.UTF8.GetBytes(sb.ToString());
            context.Response.ContentLength64 = b.Length;
            context.Response.OutputStream.Write(b, 0, b.Length);
            context.Response.OutputStream.Close();
        }
    }
}
