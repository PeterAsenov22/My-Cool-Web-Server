using System;

namespace WebServer.ByTheCakeApplication.Controllers
{
    using Server.Http;
    using Infrastructure;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using ViewModels;
    using ViewModels.Account;
    using Services;
    using Services.Interfaces;

    public class AccountController : Controller
    {
        private readonly IUserService users;

        public AccountController()
        {
            this.users = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(@"account\register");
        }

        public IHttpResponse Register(IHttpRequest request, RegisterUserViewModel model)
        {
            this.SetDefaultViewData();

            if (model.Username.Length < 3 ||
                model.Password.Length < 3 ||
                model.Password != model.ConfirmPassword)
            {
                this.ViewData["showError"] = "block";
                this.ViewData["error"] = "Invalid user details.";

                return this.FileViewResponse(@"account\register");
            }

            var success = this.users.Create(model.Username, model.Password);

            if (success)
            {
                this.LogInUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ViewData["showError"] = "block";
                this.ViewData["error"] = "This username is taken.";

                return this.FileViewResponse(@"account\register");
            }
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(@"account\login");
        }

        public IHttpResponse Login(IHttpRequest req, LoginUserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                this.ViewData["error"] = "You have empty fields.";
                this.ViewData["showError"] = "block";
                this.ViewData["authDisplay"] = "none";

                return this.FileViewResponse(@"account\login");
            }

            var success = users.Find(model.Username, model.Password);
            if (success)
            {
                this.LogInUser(req, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ViewData["error"] = "Invalid user details.";
                this.ViewData["showError"] = "block";
                this.ViewData["authDisplay"] = "none";

                return this.FileViewResponse(@"account\login");
            }
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }

        public IHttpResponse Profile(IHttpRequest request)
        {
            if (!request.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is no logged in user.");
            }

            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            var profile = users.Profile(username);

            if (profile == null)
            {
                throw new InvalidOperationException($"The user {username} could not be found in the database!");
            }

            this.ViewData["username"] = profile.Username;
            this.ViewData["registrationDate"] = profile.RegistrationDate.ToShortDateString();
            this.ViewData["totalOrders"] = profile.TotalOrders.ToString();

            return this.FileViewResponse(@"account\profile");
        }

        private void SetDefaultViewData()
        {
            this.ViewData["showError"] = "none";
            this.ViewData["authDisplay"] = "none";
        }

        private void LogInUser(IHttpRequest req, string username)
        {
            req.Session.Add(SessionStore.CurrentUserKey, username);
            req.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}
