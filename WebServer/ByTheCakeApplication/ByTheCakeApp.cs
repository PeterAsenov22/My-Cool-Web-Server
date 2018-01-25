﻿namespace WebServer.ByTheCakeApplication
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using ViewModels.Account;

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
            appRouteConfig.Get("/add", request => new CakesController().Add());
            appRouteConfig.Post("/add", request => new CakesController().Add(request.FormData["name"], request.FormData["price"]));
            appRouteConfig.Get("/search", request => new CakesController().Search(request));
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
        }
    }
}
