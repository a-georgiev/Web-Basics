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

        private const string LoginForm = @"<form action='/Login' method='POST'>
            Username: <input type='text' name='Username'/>
            Password: <input type='text' name='Password'/>
            <input type='submit' value ='Log In' /> 
        </form>";

        private const string Username = "user";
        private const string Password = "user123"; 


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
               .MapGet("/Cookies", new HtmlResponse("", Program.AddCoockieAction))
               .MapGet("/Session", new TextResponse("", Program.DisplaySessioninfoAction))
               .MapGet("/Login", new HtmlResponse(Program.LoginForm))
               .MapPost("/Login", new HtmlResponse("", Program.LoginAction))
               .MapGet("/Logout", new HtmlResponse("", Program.LogoutAction))
               .MapGet("/UserProfile", new HtmlResponse("", Program.GetUserDataAction)));

            await server.Start();
        }
        private static void GetUserDataAction(Request request, Response response)
        {
            if(request.Session.ContainsKey(Session.SessionUserKey))
            {
                response.Body = String.Empty;
                response.Body += $"<h3>Currentry logged-in user is with username '{Username}'</h3>"; 
            }
            else
            {
                response.Body = String.Empty;
                response.Body += "<h3>Currentry logged-in user - <a href='/Login'>Login</a></h3>";
            }
        }
        private static void LogoutAction(Request request, Response response)
        {
            request.Session.Clear();
            response.Body = String.Empty;
            response.Body += "<h3>Logged out successfully!</h3>";
        }
        private static void LoginAction(Request request, Response response)
        {
            request.Session.Clear();

            var bodyText = string.Empty;

            var usernameMatch = request.Form["Username"] == Program.Username;
            var passwordMatch = request.Form["Password"] == Program.Password;

            if(usernameMatch && passwordMatch)
            {
                request.Session[Session.SessionUserKey] = "MyUserId";
                response.Cookies.Add(Session.SessionCookieName, request.Session.ID);

                bodyText = "<h3>Logged Successfully!</h3>";
            }
            else
            {
                bodyText = Program.LoginForm;
            }

            response.Body = String.Empty;
            response.Body += bodyText;
        }

        private static void DisplaySessioninfoAction(Request request, Response response)
        {
            var sessionExist = request.Session.ContainsKey(Session.SessionCurrentDateKey);

            var bodyText = String.Empty;

            if(sessionExist)
            {
                var currentDate = request.Session[Session.SessionCurrentDateKey];
                bodyText = $"Stored Date: {currentDate}";
            }
            else
            {
                bodyText = "Current Date Stored!";
            }

            response.Body = String.Empty;
            response.Body += bodyText;
        }
        private static void AddCoockieAction(Request request, Response response)
        {
            var requestHasCookies = request.Cookies.Any(c=>c.Name != Session.SessionCookieName);
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
