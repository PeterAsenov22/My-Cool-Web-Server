﻿namespace WebServer.ByTheCakeApplication.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IShoppingService
    {
        void CreateOrder(int userId, IEnumerable<int> productIds);
    }
}