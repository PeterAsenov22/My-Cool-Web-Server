﻿using WebServer.Server.Common;
using WebServer.Server.Handlers;
using WebServer.Server.Http;
using WebServer.Server.Routing.Contracts;

namespace WebServer.Server
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRouteConfig serverRouteConfig;

        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(client,nameof(client));
            CoreValidator.ThrowIfNull(serverRouteConfig,nameof(serverRouteConfig));

            this.client = client;
            this.serverRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            if (!string.IsNullOrEmpty(httpRequest))
            {
                var httpContext = new HttpContext(new HttpRequest(httpRequest));
                var httpHandler = new HttpHandler(this.serverRouteConfig);
                var httpResponse = httpHandler.Handle(httpContext);

                var responseBytes = Encoding.UTF8.GetBytes(httpResponse.ToString());
                var byteSegments = new ArraySegment<byte>(responseBytes);

                await this.client.SendAsync(byteSegments, SocketFlags.None);

                Console.WriteLine("-----REQUEST-----");
                Console.WriteLine(httpRequest);
                Console.WriteLine("-----RESPONSE-----");
                Console.WriteLine(httpResponse);
                Console.WriteLine();
            }          

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<string> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int bytesRead = await this.client.ReceiveAsync(data, SocketFlags.None);

                if (bytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, bytesRead);
                result.Append(bytesAsString);

                if (bytesRead < 1024)
                {
                    break;
                }
            }

            return result.ToString();
        }
    }
}
