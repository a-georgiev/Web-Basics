using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicWebServer.Server.HTTP
{
    public class Request
    {
        public Method Method { get; set; }
        public string Url { get; set; }
        public HeaderCollection Headers { get; set; }
        public string Body { get; set; }

        public static Request Parse(string request)
        {
            var line = request.Split("\r\n");
            var startLine = line.First().Split(" ");

            var method = ParseMethod(startLine[0]);
            var url = startLine[1];
            var headers = ParseHeaders(line.Skip(1));
            
            var bodyLines = line.Skip(headers.Count + 2).ToArray();
            var body = string.Join("\r\n", bodyLines);
            return new Request
            {
                Method = method,
                Url = url,
                Headers = headers,
                Body = body
            };
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            var headers = new HeaderCollection();
            foreach(var line in headerLines)
            {
                if(line == String.Empty)
                {
                    break;
                }

                var headerParts = line.Split(':',2);

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
