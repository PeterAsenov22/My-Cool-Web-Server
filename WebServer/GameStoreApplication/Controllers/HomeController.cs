namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http.Contracts;
    using System;
    using System.Linq;
    using System.Text;
    using Services;
    using Services.Interfaces;
    using ViewModels.Home;
    using Server.Http;

    public class HomeController : BaseController
    {
        private const string HomePageView = @"home\home-page";
        private const string AdminHomePageView = @"home\admin-home-page";
        private const string InfoPageView = @"home\game-info";
        private const string AdminInfoPageView = @"home\admin-game-info";

        private readonly IGameService games;

        public HomeController(IHttpRequest request)
            : base(request)
        {
            this.games = new GameService();
        }

        public IHttpResponse Home()
        {
            bool isAll = true;

            if (this.Request.UrlParameters.Any())
            {
                if (this.Request.UrlParameters["filter"] == "Owned")
                {
                    isAll = false;
                }
            }

            if (this.Authentication.IsAdmin)
            {
                this.ViewData["games"] = this.AdminHomePageHtml(isAll);

                return this.FileViewResponse(AdminHomePageView);
            }

            this.ViewData["games"] = this.DefaultHomePageHtml(isAll);

            return this.FileViewResponse(HomePageView);
        }

        public IHttpResponse Info()
        {
            var gameId = int.Parse(this.Request.UrlParameters["id"]);

            var game = this.games.Get(gameId);

            if (game == null)
            {
                return this.RedirectResponse(HomePath);
            }
            else
            {
                this.ViewData["title"] = game.Title;
                this.ViewData["description"] = game.Description;
                this.ViewData["trailerId"] = game.TrailerId;
                this.ViewData["price"] = game.Price.ToString("f2");
                this.ViewData["size"] = game.Size.ToString("f2");
                this.ViewData["releaseDate"] = game.ReleaseDate.ToString("yyyy-MM-dd");
                this.ViewData["backTo"] = "/";
                this.ViewData["id"] = game.Id.ToString();
            }

            if (this.Authentication.IsAdmin)
            {
                return this.FileViewResponse(AdminInfoPageView);
            }

            return this.FileViewResponse(InfoPageView);
        }

        private string DefaultHomePageHtml(bool isAll)
        {
            GameHomePageDetails[] games;

            if (isAll)
            {
                games = this.games.AllGamesDetails().ToArray();
            }
            else
            {
                var email = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);
                games = this.games.OwnedGamesDetails(email).ToArray();
            }

               var gamesHtml = games.Select(g => 
                $@"<div class=""card col-md-4 thumbnail"">
                      <img class=""card-image-top img-fluid img-thumbnail"" src=""{g.Image}"" onerror=""this.src='https://i.ytimg.com/vi/%7BYouTube%20Video%20Id%7D/maxresdefault.jpg';"">
                      <div class=""card-body"">
                          <h4 class=""card-title"">{g.Title}</h4>
                          <p class=""card-text""><strong>Price</strong> - {g.Price}&euro;</p>
                          <p class=""card-text""><strong>Size</strong> - {g.Size} GB</p>
                          <p class=""card-text"">{g.Description}</p>
                      </div>
                      <div class=""card-footer"">
                          <a class=""card-button btn btn-outline-primary"" name=""info"" href=""/home/games/info/{g.Id}"">Info</a>
                          <a class=""card-button btn btn-primary"" name=""buy"" href=""/shopping/buy/{g.Id}"">Buy</a>
                      </div>
                   </div>").ToArray();

            var html = new StringBuilder();

            for (int i = 0; i < gamesHtml.Length; i += 3)
            {
                html.AppendLine(@"<div class=""card-group"">");

                for (int j = i; j < Math.Min(i + 3, gamesHtml.Length); j++)
                {
                    html.AppendLine(gamesHtml[j]);
                }

                html.AppendLine("</div>");
            }

            return html.ToString().Trim();
        }

        private string AdminHomePageHtml(bool isAll)
        {
            GameHomePageDetails[] games;

            if (isAll)
            {
                games = this.games.AllGamesDetails().ToArray();
            }
            else
            {
                var email = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);
                games = this.games.OwnedGamesDetails(email).ToArray();
            }

            var gamesHtml = games
                .Select(g =>
                    $@"<div class=""card col-md-4 thumbnail"">
                      <img class=""card-image-top img-fluid img-thumbnail"" src=""{g.Image}"" onerror=""this.src='https://i.ytimg.com/vi/%7BYouTube%20Video%20Id%7D/maxresdefault.jpg';"">
                      <div class=""card-body"">
                          <h4 class=""card-title"">{g.Title}</h4>
                          <p class=""card-text""><strong>Price</strong> - {g.Price}&euro;</p>
                          <p class=""card-text""><strong>Size</strong> - {g.Size} GB</p>
                          <p class=""card-text"">{g.Description}</p>
                      </div>
                      <div class=""card-footer"">
                          <a class=""card-button btn btn-warning"" name=""edit"" href=""/admin/games/edit/{g.Id}"">Edit</a>
                          <a class=""card-button btn btn-danger"" name=""delete"" href=""/admin/games/delete/{g.Id}"">Delete</a>
                          <a class=""card-button btn btn-outline-primary"" name=""info"" href=""/home/games/info/{g.Id}"">Info</a>
                          <a class=""card-button btn btn-primary"" name=""buy"" href=""/shopping/buy/{g.Id}"">Buy</a>
                      </div>
                   </div>")
                .ToArray();

            var html = new StringBuilder();

            for (int i = 0; i < gamesHtml.Length; i += 3)
            {
                html.AppendLine(@"<div class=""card-group"">");

                for (int j = i; j < Math.Min(i + 3, gamesHtml.Length); j++)
                {
                    html.AppendLine(gamesHtml[j]);
                }

                html.AppendLine("</div>");
            }

            return html.ToString().Trim();
        }
    }
}
