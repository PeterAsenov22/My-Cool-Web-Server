namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using Infrastructure;
    using Server.Http.Contracts;
    using ViewModels;
    using Data;

    public class CakesController : Controller
    {
        private readonly CakesData cakesData;

        public CakesController()
        {
            this.cakesData = new CakesData();
        }

        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse Add(string name, string price)
        {
            this.cakesData.Add(name,price);

            this.ViewData["name"] = name;
            this.ViewData["price"] = price;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            const string searchTermKey = "searchTerm";

            this.ViewData["searchTerm"] = string.Empty;
            this.ViewData["results"] = string.Empty;

            var urlParameters = request.UrlParameters;

            if (urlParameters.ContainsKey(searchTermKey))
            {
                var searchTerm = urlParameters[searchTermKey];

                this.ViewData["searchTerm"] = searchTerm;

                var savedCakeDivs = this.cakesData
                    .All()
                    .Where(l => l.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Select(c => $@"<div>{c.Name} - ${c.Price} <a href=""/shopping/add/{c.Id}?{searchTermKey}={searchTerm}"">Order</a></div>");

                var results = string.Join(Environment.NewLine, savedCakeDivs);
                this.ViewData["results"] = results;
            }

            this.ViewData["showCart"] = "none";

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.Orders.Any())
            {
                var totalProducts = shoppingCart.Orders.Count;
                var productsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {productsText}";
            }

            return this.FileViewResponse(@"cakes\search");
        }
    }
}
