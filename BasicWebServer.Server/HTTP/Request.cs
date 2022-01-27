using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicWebServer.Server.HTTP
{
    public class Request
    {
        public Method Method { get; set; }
        public string Url { get; set; }
        public HeaderCollection Headers { get; set; }
        public CookieCollection Cookies { get; private set; }
        public string Body { get; set; }
        public IReadOnlyDictionary<string, string> Form { get; private set; }

        public static Request Parse(string request)
        {
            var line = request.Split("\r\n");
            var startLine = line.First().Split(" ");

            var method = ParseMethod(startLine[0]);
            var url = startLine[1];
            var headers = ParseHeaders(line.Skip(1));

            var cookies = ParseCookies(headers);

            var bodyLines = line.Skip(headers.Count + 2).ToArray();
            var body = string.Join("\r\n", bodyLines);

            var form = ParseForm(headers, body);

            return new Request
            {
                Method = method,
                Url = url,
                Headers = headers,
                Cookies = cookies,
                Body = body,
                Form = form
            };
        }

        private static CookieCollection ParseCookies(HeaderCollection headers)
        {
            var cookieCollection = new CookieCollection();

            if(headers.Contains(Header.Cookie))
            {
                var cookieHeader = headers[Header.Cookie];
                var allCookies = cookieHeader.Split(';');
                foreach (var cookieText in allCookies)
                {
                    var cookieParts = cookieText.Split('=');
                    var cookieName = cookieParts[0].Trim();
                    var cookieValue = cookieParts[1].Trim();

                    cookieCollection.Add(cookieName, cookieValue);
                }
            }

            return cookieCollection;
        }

        private static Dictionary<string, string> ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string, string>();

            if(headers.Contains(Header.ContentType)
               && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                var parsedResult = ParseFormData(body);

                foreach (var (name, value) in parsedResult)
                {
                    formCollection.Add(name, value);
                }
            }

            return formCollection; 
        }

        private static Dictionary<string, string> ParseFormData(string bodyLines)
            => HttpUtility.UrlDecode(bodyLines)
                .Split('&')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .ToDictionary(
                    part => part[0],
                    part => part[1],
                    StringComparer.InvariantCultureIgnoreCase);
        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            var headers = new HeaderCollection();
            foreach(var line in headerLines)
            {
                if(line == string.Empty)
                {
                    break;
                }

                var headerParts = line.Split(":",2);

                if(headerParts.Length != 2)
                {
                    throw new InvalidCastException("Request not valid");
                }

                var headerNmae = headerParts[0];
                var headerValue = headerParts[1].Trim();

                headers.Add(headerNmae, headerValue);
            }

            return headers;
        }

        private static Method ParseMethod(string method)
        {
            try
            {
                return (Method)Enum.Parse(typeof(Method), method, true);
            }
            catch (Exception)
            {

                throw new InvalidOperationException($"{method} is not suported");
            }
        }
    }
}
