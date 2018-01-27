namespace WebServer.ByTheCakeApplication.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IShoppingService
    {
        void CreateOrder(int userId, Dictionary<int,int> products);
    }
}
