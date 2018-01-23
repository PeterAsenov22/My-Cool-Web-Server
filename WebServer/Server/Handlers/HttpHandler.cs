using System;
using WebServer.Server.Http;

namespace WebServer.Server.Handlers
{
    using Contracts;
    using Http.Contracts;
    using System.Text.RegularExpressions;
    using Common;
    using Http.Response;
    using Routing.Contracts;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            try
            {
                const string loginPath = "/login";
                //Check if user is authenticated
                if (httpContext.Request.Path != loginPath && !httpContext.Request.Session.Contains(SessionStore.CurrentUserKey))
                {
                    return new RedirectResponse(loginPath);
                }

                var requestMethod = httpContext.Request.RequestMethod;
                var path = httpContext.Request.Path;

                foreach (var registeredRoute in serverRouteConfig.Routes[requestMethod])
                {
                    var pattern = registeredRoute.Key;
                    var routingContext = registeredRoute.Value;
                    var regex = new Regex(pattern);
                    var match = regex.Match(path);

                    if (!match.Success)
                    {
                        continue;
                    }

                    var parameters = routingContext.Parameters;

                    foreach (var parameter in parameters)
                    {
                        var parameterValue = match.Groups[parameter].Value;
                        httpContext.Request.AddUrlParameter(parameter, parameterValue);
                    }

                    return routingContext.RequestHandler.Handle(httpContext);
                }
            }
            catch (Exception exception)
            {
                return new InternalServerErrorResponse(exception);
            }

            return new NotFoundResponse();
        }
    }
}
