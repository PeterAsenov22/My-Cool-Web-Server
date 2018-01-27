namespace WebServer.ByTheCakeApplication.Services
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Interfaces;
    using System.Collections.Generic;
    using ViewModels.Products;

    public class ProductService : IProductService
    {
        public void Create(string name, decimal price, string imageUrl)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var product = new Product
                {
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl
                };

                db.Products.Add(product);
                db.SaveChanges();
            }
        }

        public IEnumerable<ProductListingViewModel> All(string searchTerm = null)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var resultsQuery = db.Products.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    resultsQuery = resultsQuery.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()));
                }
                
                return resultsQuery
                    .Select(p => new ProductListingViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToList();
            }
        }

        public ProductDetailsViewModel Find(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Where(p => p.Id == id)
                    .Select(p => new ProductDetailsViewModel
                    {
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl
                    })
                    .FirstOrDefault();
            }
        }

        public bool Exists(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Any(p => p.Id == id);
            }
        }

        public IEnumerable<ProductInCartViewModel> FindProductsInCart(Dictionary<int,int> products)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Where(p => products.Keys.Contains(p.Id))
                    .Select(p => new ProductInCartViewModel
                    {
                        Name = p.Name,
                        Price = p.Price,
                        Quantity = products[p.Id]
                    }).ToList();
            }
        }
    }
}
