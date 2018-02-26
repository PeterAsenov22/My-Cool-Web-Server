namespace WebServer.GameStoreApplication.Services.Interfaces
{
    using System.Collections.Generic;
    using ViewModels.Admin;

    public interface IShoppingService
    {
        void CreateOrder(string email, IEnumerable<GameFromDbViewModel> games);
    }
}
