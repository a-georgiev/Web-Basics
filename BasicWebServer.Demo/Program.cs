using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BasicWebServer.Demo
{
    class Program
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
            Name: <input type='text' name='Name'/>
            Age: <input type='number' name ='Age'/>
            <input type='submit' value ='Save' />
        </form>";

        private const string DownloadForm = @"<form action='/Content' method='POST'>
            <input type='submit' value ='Download Sites Content' /> 
        </form>";

        private const string FileName = "content.txt";

        public static async Task Main()
        {
            await DownloadSiteAsTextFile(Program.FileName,
                    new string[] { "https://judge.softuni.org", "https://softuni.org" });

            var server = new HttpServer(routes => routes
               .MapGet("/", new TextResponse("Text response from the server."))
               .MapGet("/Redirect", new RedirectResponse("https://github.com/"))
               .MapGet("/HTML", new HtmlResponse(Program.HtmlForm))
               .MapPost("/HTML", new TextResponse("", Program.AddFormDataAction))
               .MapGet("/Content", new HtmlResponse(Program.DownloadForm))
               .MapPost("/Content", new TextFileResponse(Program.FileName))
               .MapGet("/Cookies", new HtmlResponse("", Program.AddCoockieAction)));

            await server.Start();
        }

        private static void AddCoockieAction(Request request, Response response)
        {
            var requestHasCookies = request.Cookies.Any();
            var body = "";

            if(requestHasCookies)
            {
                var cookieTxt = new StringBuilder();
                cookieTxt.Append("<table border = '1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in request.Cookies)
                {
                    cookieTxt.Append($@"<tr><td>{HttpUtility.HtmlEncode(cookie.Name)}</td>
                        <td>{HttpUtility.HtmlEncode(cookie.Value)}</td></tr>");
                }
                cookieTxt.Append("</table>");

                body = cookieTxt.ToString();
            }
            else
            {
                body = "<h1>Cookie Set!</h1>";
            }
            if(!requestHasCookies)
            {
                response.Cookies.Add("My-Cookie", "My-Value");
                response.Cookies.Add("My-Cookie-2", "My-Value-2");
            }

            response.Body=body;
        }
        private static async Task DownloadSiteAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();
            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }
            var response = await Task.WhenAll(downloads);
            var responseString = string.Join(Environment.NewLine, new String('-', 100), response);

            await File.WriteAllTextAsync(fileName, responseString);

        }
        private static async Task<string> DownloadWebSiteContent (string url)
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();

                return html.Substring(0, 200);
            }
        }
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
