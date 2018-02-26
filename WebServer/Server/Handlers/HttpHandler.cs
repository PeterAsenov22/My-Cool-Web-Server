namespace WebServer.Server.Handlers
{
    using System;
    using System.Linq;
    using Http;
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
                //Check if user is authenticated                
                var anonymousPaths = this.serverRouteConfig.AnonymousPaths;
               
                if (!anonymousPaths.Contains(httpContext.Request.Path) && !httpContext.Request.Session.Contains(SessionStore.CurrentUserKey))
                {
                    return new RedirectResponse(anonymousPaths.First());
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
