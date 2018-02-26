namespace WebServer.GameStoreApplication.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Data.Models;
    using ViewModels.Admin;
    using Interfaces;

    public class ShoppingService : IShoppingService
    {
        public void CreateOrder(string email, IEnumerable<GameFromDbViewModel> games)
        {
            using (var db = new GameStoreDbContext())
            {
                var user = db.Users.First(u => u.Email == email);
                var gamesToAdd = new List<UserGame>();

                foreach (var game in games)
                {
                    if (!db.UserGames.Any(g => g.GameId == game.Id && g.UserId == user.Id))
                    {
                        gamesToAdd.Add(new UserGame
                        {
                            UserId = user.Id,
                            GameId = game.Id
                        });
                    }
                }

                user.Games.AddRange(gamesToAdd);

                db.SaveChanges();
            }
        }
    }
}
