namespace WebServer.GameStoreApplication
{
    using Server.Contracts;
    using Server.Routing.Contracts;
    using Controllers;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using ViewModels.Account;
    using System;
    using System.Globalization;
    using ViewModels.Admin;

    public class GameStoreApp : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new GameStoreDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRoutingConfig appRouteConfig)
        {
            appRouteConfig.AnonymousPaths.Add("/account/login");
            appRouteConfig.AnonymousPaths.Add("/account/register");

            appRouteConfig.Get("/", request => new HomeController(request).Home());
            appRouteConfig.Get("/home/games/info/{(?<id>[0-9]+)}", request => new HomeController(request).Info());
            appRouteConfig.Get("/account/login", request => new AccountController(request).Login());
            appRouteConfig.Post("/account/login", request => new AccountController(request).Login(new LoginUserViewModel
            {
                Email = request.FormData["email"],
                Password = request.FormData["password"]
            }));
            appRouteConfig.Get("/account/register", request => new AccountController(request).Register());
            appRouteConfig.Post("/account/register", request => new AccountController(request).Register(new RegisterUserViewModel
            {
                Email = request.FormData["email"],
                Name = request.FormData["fullName"],
                Password = request.FormData["password"],
                ConfirmPassword = request.FormData["confirm-password"]
            }));
            appRouteConfig.Get("/account/logout", request => new AccountController(request).Logout());

            appRouteConfig.Get("/admin/games/add", request => new AdminController(request).AddGame());
            appRouteConfig.Post("/admin/games/add", request => new AdminController(request).AddGame(new AddGameViewModel
            {
                Title = request.FormData["title"],
                Description = request.FormData["description"],
                Image = request.FormData["image"],
                TrailerId = request.FormData["trailerId"],
                Price = decimal.Parse(request.FormData["price"]),
                Size = double.Parse(request.FormData["size"]),
                ReleaseDate = DateTime.ParseExact(request.FormData["releaseDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
            }));

            appRouteConfig.Get("/admin/games/list", request => new AdminController(request).ListAllGames());
            appRouteConfig.Get("/admin/games/edit/{(?<id>[0-9]+)}", request => new AdminController(request).EditGame());
            appRouteConfig.Post("/admin/games/edit/{(?<id>[0-9]+)}", request => new AdminController(request).EditGame(new AddGameViewModel
            {
                Title = request.FormData["title"],
                Description = request.FormData["description"],
                Image = request.FormData["image"],
                TrailerId = request.FormData["trailerId"],
                Price = decimal.Parse(request.FormData["price"]),
                Size = double.Parse(request.FormData["size"]),
                ReleaseDate = DateTime.ParseExact(request.FormData["releaseDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
            }));

            appRouteConfig.Get("/admin/games/delete/{(?<id>[0-9]+)}", request => new AdminController(request).DeleteGameDetails());
            appRouteConfig.Post("/admin/games/delete/{(?<id>[0-9]+)}", request => new AdminController(request).Delete());

            appRouteConfig.Get("/shopping/buy/{(?<id>[0-9]+)}", request => new ShoppingController(request).AddToCart());
            appRouteConfig.Get("/shopping/cart", request => new ShoppingController(request).ShowCart());
            appRouteConfig.Get("/shopping/cart/remove/{(?<id>[0-9]+)}", request => new ShoppingController(request).RemoveFromCart());
            appRouteConfig.Post("/shopping/cart", request => new ShoppingController(request).Order());
        }
    }
}
