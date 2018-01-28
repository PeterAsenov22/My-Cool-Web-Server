namespace WebServer.ByTheCakeApplication.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Shopping;

    public interface IShoppingService
    {
        void CreateOrder(int userId, Dictionary<int,int> products);

        IEnumerable<OrderFromDbViewModel> GetUserOrders(string username);

        IEnumerable<ProductFromOrderViewModel> GetOrderProducts(int orderId);

        DateTime GetOrderCreationDate(int orderId);
    }
}
