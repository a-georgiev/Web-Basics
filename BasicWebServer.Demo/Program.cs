using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using System;

namespace BasicWebServer.Demo
{
    class Program
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
            Name: <input type='text' name='Name'/>
            Age: <input type='number' name ='Age'/>
            <input type='submit' value ='Save' />
        </form>";

        static void Main()
            => new HttpServer(routes => routes
                .MapGet("/", new TextResponse("Text response from the server."))
                .MapGet("/Redirect", new RedirectResponse("https://github.com/"))
                .MapGet("/HTML", new HtmlResponse(Program.HtmlForm))
                .MapPost("/HTML", new TextResponse("", Program.AddFormDataAction)))
                .Start();

        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = "";

            foreach (var (key, value) in request.Form)
            {
                response.Body += $"{key} - {value}";
                response.Body += Environment.NewLine;
            }
        }
    }
}
