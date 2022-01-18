using System;

namespace BasicWebServer.Server.HTTP
{
    public class Response
    {
        public Response(StatusCode statusCode)
        {
            this.StatusCode = statusCode;

            this.Headers.Add("Server", "My Web Server");
            this.Headers.Add("Date", $"{DateTime.UtcNow}");
        }

        public StatusCode StatusCode { get; set; }
        public HeaderCollection Headers { get; set; }
        public string Body { get; set; }
    }
}
