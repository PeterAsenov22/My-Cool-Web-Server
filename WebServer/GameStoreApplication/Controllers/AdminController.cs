namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http.Contracts;
    using ViewModels.Admin;
    using Services;
    using Services.Interfaces;
    using System;
    using System.Linq;

    public class AdminController : BaseController
    {
        private const string AddGameView = @"admin\add-game";
        private const string EditGameView = @"admin\edit-game";
        private const string DeleteGameView = @"admin\delete-game";
        private const string AllGamesView = @"admin\list-games";
        private const string AllGamesPath = "/admin/games/list";

        private readonly IGameService games;

        public AdminController(IHttpRequest request) : base(request)
        {
            this.games = new GameService();
        }

        public IHttpResponse AddGame()
        {
            if (!Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            return this.FileViewResponse(AddGameView);
        }

        public IHttpResponse AddGame(AddGameViewModel model)
        {
            if (!Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            if (!this.ValidateModel(model))
            {
                return this.AddGame();
            }

            this.games.Create(model.Title,model.Description,model.Image,model.TrailerId,model.Size,model.Price, model.ReleaseDate.Value);

            return this.RedirectResponse("/");
        }

        public IHttpResponse ListAllGames()
        {
            if (!Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            var allGames = this.games.All().Select(g => $@"<tr class=""table - warning"">
                <th scope = ""row"">${g.Id}</th>
                <td>${g.Name}</td>
                <td>${g.Size:f2} GB</td>
                <td>${g.Price:f2} &euro;</td>    
                <td>    
                <a href = ""/admin/games/edit/{g.Id}"" class=""btn btn-warning btn-sm"">Edit</a>
                <a href = ""/admin/games/delete/{g.Id}"" class=""btn btn-danger btn-sm"">Delete</a>
                </td>
                </tr>");

            var gamesAsHtml = string.Join(Environment.NewLine, allGames);

            this.ViewData["games"] = gamesAsHtml;

            return this.FileViewResponse(AllGamesView);
        }

        public IHttpResponse EditGame()
        {
            if (!Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            this.SetGameResponse();

            return this.FileViewResponse(EditGameView);
        }

        public IHttpResponse EditGame(AddGameViewModel model)
        {
            if (!Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            if (!this.ValidateModel(model))
            {
                this.SetDefaultData();

                return this.FileViewResponse(EditGameView);
            }

            var gameId = int.Parse(this.Request.UrlParameters["id"]);

            this.games.Update(gameId, model.Title, model.Description, model.Image, model.TrailerId, model.Size, model.Price, model.ReleaseDate.Value);

            return this.RedirectResponse(AllGamesPath);
        }

        public IHttpResponse DeleteGameDetails()
        {
            if (!Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            this.SetGameResponse();

            return this.FileViewResponse(DeleteGameView);
        }

        public IHttpResponse Delete()
        {
            if (!Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            var gameId = int.Parse(this.Request.UrlParameters["id"]);

            var result = this.games.Delete(gameId);

            if (!result)
            {
                this.AddError($"Game with id {gameId} doesn't exist!");

                this.SetDefaultData();

                return this.FileViewResponse(DeleteGameView);
            }

            return this.RedirectResponse(AllGamesPath);
        }

        private void SetDefaultData()
        {
            this.ViewData["title"] = string.Empty;
            this.ViewData["description"] = string.Empty;
            this.ViewData["imageUrl"] = string.Empty;
            this.ViewData["trailerId"] = string.Empty;
            this.ViewData["price"] = string.Empty;
            this.ViewData["size"] = string.Empty;
            this.ViewData["releaseDate"] = string.Empty;
            this.ViewData["button"] = "disabled";
        }

        private void SetGameResponse()
        {
            var gameId = int.Parse(this.Request.UrlParameters["id"]);

            var game = this.games.Get(gameId);

            if (game == null)
            {
                this.AddError($"Game with id {gameId} doesn't exist!");

                this.SetDefaultData();
            }
            else
            {
                this.ViewData["title"] = game.Title;
                this.ViewData["description"] = game.Description;
                this.ViewData["imageUrl"] = game.Image;
                this.ViewData["trailerId"] = game.TrailerId;
                this.ViewData["price"] = game.Price.ToString("f2");
                this.ViewData["size"] = game.Size.ToString("f2");
                this.ViewData["releaseDate"] = game.ReleaseDate.ToString("yyyy-MM-dd");
                this.ViewData["button"] = string.Empty;
            }
        }
    }
}
