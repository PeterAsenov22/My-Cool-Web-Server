namespace WebServer.ByTheCakeApplication.Controllers
{
    using System.Linq;
    using Data;
    using Server.Http.Contracts;
    using Infrastructure;
    using Models;
    using Server.Http.Response;

    public class ShoppingController : Controller
    {
        private readonly CakesData cakesData;

        public ShoppingController()
        {
            this.cakesData = new CakesData();
        }

        public IHttpResponse AddToCart(IHttpRequest request)
        {
            var idNumber = int.Parse(request.UrlParameters["id"]);

            var cake = this.cakesData.Find(idNumber);

            if (cake == null)
            {
                return new NotFoundResponse();
            }

            request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey).Orders.Add(cake);

            var redirectUrl = "/search";

            const string searchTermKey = "searchTerm";

            if (request.UrlParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={request.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest request)
        {
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.Orders.Any())
            {
                this.ViewData["cartItems"] = "<div>No items in your cart</div>";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                decimal totalCost = 0;
                var cartItems = string.Empty;

                foreach (var cake in shoppingCart.Orders)
                {
                    cartItems += $"<div>{cake}</div>";
                    totalCost += cake.Price;
                }

                this.ViewData["cartItems"] = cartItems;
                this.ViewData["totalCost"] = totalCost.ToString("f2");
            }

            return this.FileViewResponse(@"shopping\cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest request)
        {
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shoppingCart.Clear();

            return this.FileViewResponse(@"shopping\finishOrder");
        }
    }
}
