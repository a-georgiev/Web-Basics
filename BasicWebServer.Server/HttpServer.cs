using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Routing;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;
        private const string serverStartedMsg = "Server started ";
        private readonly RoutingTable routingTable;

        public HttpServer(
            string ipAddress,
            int port,
            Action<IRoutingTable> routingTableConfig)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.serverListener = new TcpListener(this.ipAddress, this.port);

            routingTableConfig(this.routingTable = new RoutingTable());
        }

        public HttpServer(int port, Action<IRoutingTable> routingTable)
            :this("192.168.88.1", port, routingTable)
        {
        }

        public HttpServer(Action<IRoutingTable> routingTable)
            : this("192.168.88.1", 1488, routingTable)
        {
        }


        public async Task Start()
        {
            this.serverListener.Start();

            Console.WriteLine(serverStartedMsg + this.ipAddress + ':' + this.port);

            while (true)
            {
                var connection = await serverListener.AcceptTcpClientAsync();

                _ = Task.Run(async () =>
                {
                    NetworkStream networkStream = connection.GetStream();

                    var requestTxt = await this.ReadResponse(networkStream);
                    Console.WriteLine(requestTxt);

                    var request = Request.Parse(requestTxt);
                    var response = this.routingTable.MatchResponse(request);

                    if (response.PreRenderAction != null)
                        response.PreRenderAction(request, response);

                    AddSession(request, response);

                    await WriteResponseAsync(networkStream, response);

                    connection.Close();
                });
            }
        }
        private static void AddSession(Request request, Response response)
        {
            var sessionExist = request.Session.ContainsKey(Session.SessionCurrentDateKey);

            if(!sessionExist)
            {
                request.Session[Session.SessionCurrentDateKey] = DateTime.Now.ToString();
                response.Cookies.Add(Session.SessionCookieName, request.Session.ID);
            }
        }
        private async Task WriteResponseAsync(NetworkStream networkStream, Response response)
        {

            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            await networkStream.WriteAsync(responseBytes);
        }

        private async Task<string> ReadResponse(NetworkStream networkStream)
        {
            int bufferLength = 1024;
            byte[] buffer = new byte[bufferLength];

            var totalBytes = 0;

            StringBuilder requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);

                totalBytes+=bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request too long");

                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

            } while (networkStream.DataAvailable);

            return requestBuilder.ToString();
        }
    }
}
