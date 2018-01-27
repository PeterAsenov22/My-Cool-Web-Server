namespace WebServer.ByTheCakeApplication.Controllers
{
    using System.Linq;
    using Server.Http.Contracts;
    using Infrastructure;
    using ViewModels;
    using Server.Http.Response;
    using Services;
    using Services.Interfaces;
    using Server.Http;

    public class ShoppingController : Controller
    {
        private readonly IProductService products;
        private readonly IUserService users;
        private readonly IShoppingService shopping;

        public ShoppingController()
        {
            this.products = new ProductService();
            this.users = new UserService();
            this.shopping = new ShoppingService();
        }

        public IHttpResponse AddToCart(IHttpRequest request)
        {
            var idNumber = int.Parse(request.UrlParameters["id"]);

            var productExists = this.products.Exists(idNumber);

            if (!productExists)
            {
                return new NotFoundResponse();
            }

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            if (!shoppingCart.Products.ContainsKey(idNumber))
            {
                shoppingCart.Products[idNumber] = 0;
            }

            shoppingCart.Products[idNumber]++;

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

            if (!shoppingCart.Products.Any())
            {
                this.ViewData["cartItems"] = "<div>No items in your cart</div>";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                decimal totalCost = 0;
                var cartItems = string.Empty;

                var orders = this.products.FindProductsInCart(shoppingCart.Products);

                foreach (var product in orders)
                {
                    cartItems += $"<div>{product.Name} - ${product.Price:F2} - Quantity: {product.Quantity}</div><br />";
                    totalCost += product.Price * product.Quantity;
                }

                this.ViewData["cartItems"] = cartItems;
                this.ViewData["totalCost"] = totalCost.ToString("f2");
            }

            return this.FileViewResponse(@"shopping\cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest request)
        {
            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            var userId = this.users.GetUserId(username);
            var productsToBuy = shoppingCart.Products;

            if (!productsToBuy.Any())
            {
                return new RedirectResponse("/");
            }

            this.shopping.CreateOrder(userId,productsToBuy);

            shoppingCart.Clear();

            return this.FileViewResponse(@"shopping\finishOrder");
        }
    }
}
