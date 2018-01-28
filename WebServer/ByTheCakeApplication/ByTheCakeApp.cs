namespace WebServer.ByTheCakeApplication
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using ViewModels.Account;
    using ViewModels.Products;

    public class ByTheCakeApp : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new ByTheCakeDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRoutingConfig appRouteConfig)
        {
            appRouteConfig.Get("/", request => new HomeController().Index());
            appRouteConfig.Get("/about", request => new HomeController().About());
            appRouteConfig.Get("/add", request => new ProductsController().Add());
            appRouteConfig.Post("/add", request => new ProductsController().Add(new AddProductViewModel
            {
                Name = request.FormData["name"],
                Price = decimal.Parse(request.FormData["price"]),
                ImageUrl = request.FormData["imageUrl"]
            }));
            appRouteConfig.Get("/search", request => new ProductsController().Search(request));
            appRouteConfig.Get("/register", request => new AccountController().Register());
            appRouteConfig.Post("/register", request => new AccountController().Register(request, new RegisterUserViewModel
            {
                Username = request.FormData["username"],
                Password = request.FormData["password"],
                ConfirmPassword = request.FormData["confirm-password"]
            }));
            appRouteConfig.Get("/login", request => new AccountController().Login());
            appRouteConfig.Post("/login", request => new AccountController().Login(request, new LoginUserViewModel
            {
                Username = request.FormData["username"],
                Password = request.FormData["password"]
            }));
            appRouteConfig.Get("/shopping/add/{(?<id>[0-9]+)}", request => new ShoppingController().AddToCart(request));
            appRouteConfig.Get("/cart", request => new ShoppingController().ShowCart(request));
            appRouteConfig.Post("/shopping/finish-order", request => new ShoppingController().FinishOrder(request));
            appRouteConfig.Post("/logout", request => new AccountController().Logout(request));
            appRouteConfig.Get("/profile", request => new AccountController().Profile(request));
            appRouteConfig.Get("/cakes/{(?<id>[0-9]+)}", request => new ProductsController().Details(int.Parse(request.UrlParameters["id"])));
            appRouteConfig.Get("/orders", request => new ShoppingController().ShowOrders(request));
            appRouteConfig.Get("/orderDetails/{(?<id>[0-9]+)}", request => new ShoppingController().OrderDetails(int.Parse(request.UrlParameters["id"])));
        }
    }
}
