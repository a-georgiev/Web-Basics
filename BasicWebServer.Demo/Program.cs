using BasicWebServer.Server;
using System;

namespace BasicWebServer.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpServer("172.26.32.1", 8080);
            server.Start();
        }
    }
}
