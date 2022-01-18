using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BasicWebServer.Server
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;
        private const string serverStartedMsg = "Server started ";
        public HttpServer(string ipAddress, int port)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.serverListener = new TcpListener(this.ipAddress, this.port);

        }

        public void Start()
        {
            this.serverListener.Start();

            Console.WriteLine(serverStartedMsg + this.ipAddress + ':' + this.port);

            while (true)
            {
                TcpClient connection = serverListener.AcceptTcpClient();
                NetworkStream networkStream = connection.GetStream();

                WriteResponse(networkStream, "Kurvi sbogom!");

                connection.Close();
            }
        }

        private void WriteResponse(NetworkStream networkStream, string content)
        {
            var contentLength = Encoding.UTF8.GetByteCount(content);

            var response = $@"HTTP/1.1 200 OK
Content-Type: text/plain; charset=UTF-8
Content-Length: {contentLength}

{content}"; 

            var responseBytes = Encoding.UTF8.GetBytes(response);

            networkStream.Write(responseBytes);
        }
    }
}
