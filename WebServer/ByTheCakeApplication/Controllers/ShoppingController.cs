namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using Server.Http.Contracts;
    using ViewModels;
    using Server.Http.Response;
    using Services;
    using Services.Interfaces;
    using Server.Http;

    public class ShoppingController : BaseController
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

        public IHttpResponse ShowOrders(IHttpRequest request)
        {
            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);
            var orders = this.shopping.GetUserOrders(username);

            if (!orders.Any())
            {
                this.ViewData["showResult"] = "none";
                this.AddError("You don't have any orders!");
            }
            else
            {
                var result = String.Empty;

                foreach (var order in orders)
                {
                    result += $@"<tr><td><a href=""/orderDetails/{order.Id}"">{order.Id}</a></td><td>{order.CreatedOn.ToShortDateString()}</td><td>{order.Sum:f2}</td></tr>";
                }

                this.ViewData["showResult"] = "block";
                this.ViewData["result"] = result;
            }

            return this.FileViewResponse(@"shopping\orders");
        }

        public IHttpResponse OrderDetails(int orderId)
        {
            var productsFromOrder = this.shopping.GetOrderProducts(orderId);

            if (productsFromOrder == null)
            {
                this.ViewData["showResult"] = "none";
                this.AddError($"Order with id - {orderId} doesn't exist.");
            }
            else
            {
                var result = String.Empty;
                var creationDate = this.shopping.GetOrderCreationDate(orderId);

                foreach (var product in productsFromOrder)
                {
                    result += $@"<tr><td><a href=""/cakes/{product.Id}"">{product.Name}</a></td><td>{product.Price}</td><td>{product.Quantity}</td><td>{product.Price*product.Quantity}</td></tr>";
                }

                this.ViewData["showResult"] = "block";
                this.ViewData["orderId"] = orderId.ToString();
                this.ViewData["products"] = result;
                this.ViewData["creationDate"] = creationDate.ToShortDateString();
            }

            return this.FileViewResponse(@"shopping\orderDetails");
        }
    }
}
