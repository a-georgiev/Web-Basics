using BasicWebServer.Server;
using BasicWebServer.Server.Responses;
using System;

namespace BasicWebServer.Demo
{
    class Program
    {
        static void Main()
            => new HttpServer(routes => routes
                .MapGet("/", new TextResponse("Text response from the server."))
                .MapGet("/HTML", new HtmlResponse("<h1>Html Response from server</h1>"))
                .MapGet("/Redirect", new RedirectResponse("https://softuni.org")))
                .Start();
    }
}
