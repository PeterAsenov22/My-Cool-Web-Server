﻿namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using Server.Http.Contracts;
    using ViewModels;
    using Services;
    using Services.Interfaces;
    using ViewModels.Products;
    using Server.Http.Response;

    public class ProductsController : BaseController
    {
        private const string AddView = @"products\add";

        private readonly IProductService products;

        public ProductsController()
        {
            this.products = new ProductService();
        }

        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(AddView);
        }

        public IHttpResponse Add(AddProductViewModel model)
        {
            if (model.Name.Length < 3
                || model.Name.Length > 30
                || model.ImageUrl.Length < 3
                || model.ImageUrl.Length > 2000)
            {
                this.ViewData["showResult"] = "none";
                this.AddError("Product information is not valid.");

                return this.FileViewResponse(AddView);
            }

            this.products.Create(model.Name, model.Price, model.ImageUrl);

            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = model.Price.ToString("f2");
            this.ViewData["imageUrl"] = model.ImageUrl;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(AddView);
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            const string searchTermKey = "searchTerm";

            var urlParameters = request.UrlParameters;

            this.ViewData["searchTerm"] = string.Empty;
            this.ViewData["results"] = string.Empty;

            var searchTerm = urlParameters.ContainsKey(searchTermKey)
                ? urlParameters[searchTermKey]
                : null;

            this.ViewData["searchTerm"] = searchTerm;

            var result = this.products.All(searchTerm);

            if (!result.Any())
            {
                this.ViewData["result"] = "No cakes found";
            }
            else
            {
                var allProducts = result
                    .Select(c => $@"<div><a href=""/cakes/{c.Id}"">{c.Name}</a> - ${c.Price} <a href=""/shopping/add/{c.Id}?{searchTermKey}={searchTerm}"">Order</a></div>")
                    .ToList();

                var allProductsAsString = string.Join(Environment.NewLine, allProducts);
                this.ViewData["results"] = allProductsAsString;
            }

            this.ViewData["showCart"] = "none";

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.Products.Any())
            {
                var totalProducts = shoppingCart.Products.Values.Sum();
                var productsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {productsText}";
            }

            return this.FileViewResponse(@"products\search");
        }

        public IHttpResponse Details(int id)
        {
            var product = this.products.Find(id);

            if (product == null)
            {
                return new NotFoundResponse();
            }

            this.ViewData["name"] = product.Name;
            this.ViewData["price"] = product.Price.ToString("f2");
            this.ViewData["imageUrl"] = product.ImageUrl;

            return this.FileViewResponse(@"products\details");
        }
    }
}
