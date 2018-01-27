namespace WebServer.ByTheCakeApplication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Data;
    using Interfaces;

    public class ShoppingService : IShoppingService
    {
        public void CreateOrder(int userId, IEnumerable<int> productIds)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var order = new Order
                {
                    UserId = userId,
                    CreationTime = DateTime.UtcNow,
                    Products = productIds.Select(id => new OrderProduct
                    {
                        ProductId = id
                    }).ToList()
                };

                db.Add(order);
                db.SaveChanges();
            }
        }
    }
}
