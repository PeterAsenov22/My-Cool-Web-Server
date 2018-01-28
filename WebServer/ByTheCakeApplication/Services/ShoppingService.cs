namespace WebServer.ByTheCakeApplication.Services
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Data.Models;
    using Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using ViewModels.Shopping;

    public class ShoppingService : IShoppingService
    {
        public void CreateOrder(int userId, Dictionary<int,int> products)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var order = new Order
                {
                    UserId = userId,
                    CreationTime = DateTime.UtcNow,
                    Products = products.Keys.Select(id => new OrderProduct
                    {
                        ProductId = id,
                        Quantity = products[id]
                    }).ToList()
                };

                db.Add(order);
                db.SaveChanges();
            }
        }

        public IEnumerable<OrderFromDbViewModel> GetUserOrders(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db
                    .Users
                    .Include(x => x.Orders)
                       .ThenInclude(x => x.Products)
                       .ThenInclude(x => x.Product)
                    .First(u => u.Username == username)
                    .Orders
                    .Select(o => new OrderFromDbViewModel
                    {
                        Id = o.Id,
                        CreatedOn = o.CreationTime,
                        Sum = o.Products.Sum(p => p.Quantity * p.Product.Price)
                    })
                    .ToList();
            }
        }

        public IEnumerable<ProductFromOrderViewModel> GetOrderProducts(int orderId)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var order = db
                    .Orders
                    .Include(x => x.Products)
                        .ThenInclude(x => x.Product)
                    .FirstOrDefault(o => o.Id == orderId);

                return order?.Products
                    .Select(p => new ProductFromOrderViewModel
                    {
                        Id = p.Product.Id,
                        Name = p.Product.Name,
                        Price = p.Product.Price,
                        Quantity = p.Quantity
                    })
                    .ToList();
            }
        }

        public DateTime GetOrderCreationDate(int orderId)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Orders
                    .First(o => o.Id == orderId).CreationTime;
            }
        }
    }
}
