using BasicWebServer.Server;
using System;

namespace BasicWebServer.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpServer("", 8080);
            server.Start();
        }
    }
}
