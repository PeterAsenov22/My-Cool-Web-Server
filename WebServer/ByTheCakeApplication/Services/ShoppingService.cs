namespace WebServer.ByTheCakeApplication.Services
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Data.Models;
    using Data;
    using Interfaces;

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
    }
}
