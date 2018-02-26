namespace WebServer.GameStoreApplication.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Admin;
    using ViewModels.Home;

    public interface IGameService
    {
        void Create(string title, string description, string image, string trailerId, double size, decimal price, DateTime releaseDate);

        void Update(int id, string title, string description, string image, string trailerId, double size, decimal price, DateTime releaseDate);

        bool Delete(int id);

        IEnumerable<ListGameViewModel> All();

        IEnumerable<GameHomePageDetails> AllGamesDetails();

        IEnumerable<GameHomePageDetails> OwnedGamesDetails(string email);

        GameFromDbViewModel Get(int id);
    }
}
