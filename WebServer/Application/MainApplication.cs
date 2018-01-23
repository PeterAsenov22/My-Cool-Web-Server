namespace WebServer.Application
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using Server.Handlers;

    public class MainApplication : IApplication
    {
        public void Configure(IAppRoutingConfig appRouteConfig)
        {
            appRouteConfig.Get("/",request => new HomeController().Index());
            appRouteConfig.Get("/about", request => new HomeController().About());
            appRouteConfig.Get("/add", request => new CakeController().AddCake());
            appRouteConfig.Get("/search", request => new CakeController().BrowseCake());
            appRouteConfig.Get("/register", request => new UserController().RegisterGet());
            appRouteConfig.Get("/user/{(?<name>[a-z]+)}", request => new UserController().Details(request.UrlParameters["name"]));
            appRouteConfig.AddRoute("/register", new PostHandler(httpContext => new UserController().RegisterPost(httpContext.FormData["name"])));

            appRouteConfig.Get("/testsession", request => new HomeController().SessionTest(request));
        }
    }
}
