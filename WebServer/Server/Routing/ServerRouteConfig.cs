namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Enums;
    using Contracts;

    public class ServerRouteConfig : IServerRouteConfig
    {
        private readonly IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> routes;

        public ServerRouteConfig(IAppRoutingConfig appRouteConfig)
        {
            this.AnonymousPaths = new List<string>(appRouteConfig.AnonymousPaths);

            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>>();

            var availableMethods = Enum.GetValues(typeof(HttpRequestMethod)).Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                this.routes.Add(method, new Dictionary<string, IRoutingContext>());
            }

            this.InitializeServerConfig(appRouteConfig);
        }

        public IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes => this.routes;

        public ICollection<string> AnonymousPaths { get; private set; }

        private void InitializeServerConfig(IAppRoutingConfig appRouteConfig)
        {
            foreach (var registeredRoute in appRouteConfig.Routes)
            {
                var requestMethod = registeredRoute.Key;
                var routesWithHandlers = registeredRoute.Value;

                foreach (var routeWithHandler in routesWithHandlers)
                {
                    var route = routeWithHandler.Key;
                    var handler = routeWithHandler.Value;

                    var parameters = new List<string>();

                    var parsedRouteRegex = this.ParseRoute(route, parameters);

                    var routingContext = new RoutingContext(handler, parameters);

                    this.routes[requestMethod].Add(parsedRouteRegex, routingContext);
                }
            }
        }

        private string ParseRoute(string route, List<string> parameters)
        {
            var parsedRegex = new StringBuilder();
            parsedRegex.Append("^");

            if (route == "/")
            {
                parsedRegex.Append($"{route}$");
                return parsedRegex.ToString();
            }

            var tokens = route.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            this.ParseTokens(parameters,tokens,parsedRegex);

            return parsedRegex.ToString();
        }

        private void ParseTokens(List<string> parameters, string[] tokens, StringBuilder parsedRegex)
        {
            parsedRegex.Append("/");

            for (int i = 0; i < tokens.Length; i++)
            {
                string end = i == tokens.Length - 1 ? "$" : "/";

                var currentToken = tokens[i];
                if (!currentToken.StartsWith("{") && !currentToken.EndsWith("}"))
                {
                    parsedRegex.Append($"{currentToken}{end}");
                    continue;
                }

                var regex = new Regex("<\\w+>");
                var match = regex.Match(currentToken);

                if (!match.Success)
                {
                    throw new InvalidOperationException($"Route parameter in '{currentToken}' is not valid.");
                }

                var matchString = match.Groups[0].Value;
                string paramName = matchString.Substring(1, matchString.Length - 2);
                parameters.Add(paramName);
                parsedRegex.Append($"{currentToken.Substring(1,currentToken.Length-2)}{end}");
            }
        }
    }
}
