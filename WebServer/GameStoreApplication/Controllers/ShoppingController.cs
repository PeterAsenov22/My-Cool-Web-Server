namespace WebServer.GameStoreApplication.Controllers
{
    using System;
    using System.Linq;
    using Services.Interfaces;
    using ViewModels.Shopping;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using Server.Http;

    public class ShoppingController : BaseController
    {
        private const string CartPageView = @"shopping\cart";
        private const string CartPagePath = @"/shopping/cart";

        private readonly IGameService games;
        private readonly IShoppingService shopping;

        public ShoppingController(IHttpRequest request) : base(request)
        {
            this.games = new GameService();
            this.shopping = new ShoppingService();
        }

        public IHttpResponse AddToCart()
        {
            var gameId = int.Parse(this.Request.UrlParameters["id"]);

            var game = this.games.Get(gameId);

            if (game == null)
            {
                return new NotFoundResponse();
            }

            var shoppingCart = this.Request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.Contains(gameId))
            {
                shoppingCart.Add(game);
            }

            return this.RedirectResponse(HomePath);
        }

        public IHttpResponse RemoveFromCart()
        {
            var gameId = int.Parse(this.Request.UrlParameters["id"]);
            var shoppingCart = this.Request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            shoppingCart.Remove(gameId);

            return this.RedirectResponse(CartPagePath);
        }

        public IHttpResponse ShowCart()
        {
            var shoppingCart = this.Request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            var allGames = shoppingCart.Products().Select(g => $@"<div class=""list-group-item"">
                <div class=""media"">
                     <a class=""btn btn-outline-danger btn-lg align-self-center mr-3"" href=""/shopping/cart/remove/{g.Id}"">X</a>
                     <img class=""d-flex mr-4 align-self-center img-thumbnail"" height=""127"" src=""{g.Image}"" width=""227"" alt=""Generic placeholder image"">
                     <div class=""media-body align-self-center"">
                         <a href = ""#"" >
                         <h4 class=""mb-1 list-group-item-heading"">{g.Title}</h4>
                         </a>
                         <p>{g.Description}</p>
                    </div>
                    <div class=""col-md-2 text-center align-self-center mr-auto"">
                        <h2>{g.Price}&euro;</h2>
                    </div>
                </div>
                </div>");

            var gamesAsHtml = string.Join(Environment.NewLine, allGames);

            this.ViewData["disabled"] = string.Empty;
            this.ViewData["games"] = gamesAsHtml;
            this.ViewData["totalPrice"] = shoppingCart.TotalPrice.ToString("f2");

            if (shoppingCart.TotalPrice == 0)
            {
                this.ViewData["disabled"] = "disabled";
            }

            return this.FileViewResponse(CartPageView);
        }

        public IHttpResponse Order()
        {
            var email = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);
            var shoppingCart = this.Request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            var productsToBuy = shoppingCart.Products();

            if (!productsToBuy.Any())
            {
                return this.RedirectResponse(HomePath);
            }

            this.shopping.CreateOrder(email, productsToBuy);

            shoppingCart.Clear();

            return this.RedirectResponse(HomePath);
        }
    }
}
