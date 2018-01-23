namespace WebServer.ByTheCakeApplication
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void Configure(IAppRoutingConfig appRouteConfig)
        {
            appRouteConfig.Get("/", request => new HomeController().Index());
            appRouteConfig.Get("/about", request => new HomeController().About());
            appRouteConfig.Get("/add", request => new CakesController().Add());
            appRouteConfig.Post("/add", request => new CakesController().Add(request.FormData["name"], request.FormData["price"]));
            appRouteConfig.Get("/search", request => new CakesController().Search(request));
            appRouteConfig.Get("/calculator", request => new HomeController().Calculator());
            appRouteConfig.Post("/calculator", request => new HomeController().Calculator(request.FormData["firstNum"],request.FormData["sign"],request.FormData["secondNum"]));
            appRouteConfig.Get("/tools", request => new HomeController().Tools());
            appRouteConfig.Get("/login", request => new AccountController().Login());
            appRouteConfig.Post("/login", request => new AccountController().Login(request));
            appRouteConfig.Get("/email", request => new HomeController().Email());
            appRouteConfig.Post("/email", request => new HomeController().Email(request.FormData["receiver"],request.FormData["subject"],request.FormData["message"]));
            appRouteConfig.Get("/shopping/add/{(?<id>[0-9]+)}", request => new ShoppingController().AddToCart(request));
            appRouteConfig.Get("/cart", request => new ShoppingController().ShowCart(request));
            appRouteConfig.Post("/shopping/finish-order", request => new ShoppingController().FinishOrder(request));
            appRouteConfig.Post("/logout", request => new AccountController().Logout(request));
        }
    }
}
