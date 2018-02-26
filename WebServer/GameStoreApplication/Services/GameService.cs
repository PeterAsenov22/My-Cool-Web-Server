namespace WebServer.GameStoreApplication.Services
{
    using System;
    using System.Linq;
    using Data;
    using Data.Models;
    using Interfaces;
    using System.Collections.Generic;
    using ViewModels.Admin;
    using ViewModels.Home;
    using Microsoft.EntityFrameworkCore;

    public class GameService : IGameService
    {
        public void Create(string title, string description, string image, string trailerId, double size, decimal price,
            DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = new Game
                {
                    Title = title,
                    Description = description,
                    Image = image,
                    TrailerId = trailerId,
                    Price = price,
                    Size = size,
                    ReleaseDate = releaseDate
                };

                db.Games.Add(game);
                db.SaveChanges();
            }
        }

        public void Update(int id, string title, string description, string image, string trailerId, double size, decimal price,
            DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.Find(id);

                if (game.Title != title)
                {
                    game.Title = title;
                }

                if (game.Description != description)
                {
                    game.Description = description;
                }

                if (game.Image != image)
                {
                    game.Image = image;
                }

                if (game.TrailerId != trailerId)
                {
                    game.TrailerId = trailerId;
                }

                if (game.Price != price)
                {
                    game.Price = price;
                }

                if (game.Size != size)
                {
                    game.Size = size;
                }

                if (game.ReleaseDate != releaseDate)
                {
                    game.ReleaseDate = releaseDate;
                }

                db.SaveChanges();
            }
        }

        public bool Delete(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.Find(id);

                if (game == null)
                {
                    return false;
                }

                db.Games.Remove(game);
                db.SaveChanges();

                return true;
            }
        }

        public IEnumerable<ListGameViewModel> All()
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games.Select(g => new ListGameViewModel
                {
                    Id = g.Id,
                    Name = g.Title,
                    Price = g.Price,
                    Size = g.Size
                })
                .ToList();
            }
        }

        public IEnumerable<GameHomePageDetails> AllGamesDetails()
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games.Select(g => new GameHomePageDetails
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Description = g.Description,
                        Image = g.Image,
                        Price = g.Price,
                        Size = g.Size
                    })
                    .ToList();
            }
        }

        public IEnumerable<GameHomePageDetails> OwnedGamesDetails(string email)
        {
            using (var db = new GameStoreDbContext())
            {
                var user = db.Users
                    .Include(u=>u.Games)
                    .ThenInclude(u=>u.Game)
                    .FirstOrDefault(u => u.Email == email);

                return user.Games.Select(g => new GameHomePageDetails
                    {
                        Id = g.Game.Id,
                        Title = g.Game.Title,
                        Description = g.Game.Description,
                        Image = g.Game.Image,
                        Price = g.Game.Price,
                        Size = g.Game.Size
                    })
                    .ToList();
            }
        }

        public GameFromDbViewModel Get(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.Find(id);

                if (game == null)
                {
                    return null;
                }

                return new GameFromDbViewModel
                {
                    Id = game.Id,
                    Title = game.Title,
                    Description = game.Description,
                    Image = game.Image,
                    Price = game.Price,
                    Size = game.Size,
                    TrailerId = game.TrailerId,
                    ReleaseDate = game.ReleaseDate
                };
            }
        }
    }
}
