using WebServer.Server.Http.Contracts;
namespace WebServer.Server.Routing.Contracts
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Handlers;

    public interface IAppRoutingConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, Dictionary<string,RequestHandler>> Routes { get; }

        void Get(string route, Func<IHttpRequest, IHttpResponse> handler);

        void Post(string route, Func<IHttpRequest, IHttpResponse> handler);

        void AddRoute(string route,  RequestHandler httpHandler);
    }
}
