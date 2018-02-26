namespace WebServer.GameStoreApplication.Controllers
{
    using Infrastructure;
    using Server.Http.Contracts;
    using Services;
    using Services.Interfaces;
    using Server.Http;
    using Common;

    public class BaseController : Controller
    {
        protected const string HomePath = "/";

        private readonly IUserService users;

        public BaseController(IHttpRequest request)
        {
            this.Request = request;
            this.Authentication = new Authentication(false,false);
            this.users = new UserService();
            this.ApplyAuthenticationViewData();
        }

        protected IHttpRequest Request { get; private set; } 

        protected Authentication Authentication { get; private set; }

        protected override string ApplicationDirectory => "GameStoreApplication";

        private void ApplyAuthenticationViewData()
        {
            var anonymousDisplay = "flex";
            var authDisplay = "none";
            var adminDisplay = "none";

            var isAuthenticated = this.Request.Session
                .Contains(SessionStore.CurrentUserKey);

            if (isAuthenticated)
            {
                anonymousDisplay = "none";
                authDisplay = "flex";

                var email = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);
                var isAdmin = this.users.IsAdmin(email);

                if (isAdmin)
                {
                    adminDisplay = "flex";
                }

                this.Authentication = new Authentication(true,isAdmin);
            }

            this.ViewData["anonymousDisplay"] = anonymousDisplay;
            this.ViewData["authDisplay"] = authDisplay;
            this.ViewData["adminDisplay"] = adminDisplay;
        }
    }
}
