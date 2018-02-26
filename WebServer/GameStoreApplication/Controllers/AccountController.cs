namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http;
    using Server.Http.Contracts;
    using ViewModels.Account;
    using Services;
    using Services.Interfaces;
    using ViewModels.Shopping;

    public class AccountController : BaseController
    {
        private const string LoginView = @"account\login";
        private const string RegisterView = @"account\register";

        private readonly IUserService users;

        public AccountController(IHttpRequest request)
            :base(request)
        {
            this.users = new UserService();
        }

        public IHttpResponse Login()
            => this.FileViewResponse(LoginView);

        public IHttpResponse Login(LoginUserViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Login();
            }

            var success = this.users.Find(model.Email, model.Password);

            if (success)
            {
                this.LogInUser(model.Email);

                return this.RedirectResponse(HomePath);
            }
            else
            {
                this.AddError("Invalid user details!");

                return this.Login();
            }       
        }

        public IHttpResponse Register()
           => this.FileViewResponse(RegisterView);

        public IHttpResponse Register(RegisterUserViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Register();
            }

            var success = this.users.Create(model.Email, model.Name, model.Password);

            if (success)
            {
                this.LogInUser(model.Email);

                return this.RedirectResponse(HomePath);
            }
            else
            {
                this.AddError("This email is already taken!");
                return this.Register();
            }
        }

        public IHttpResponse Logout()
        {
            this.Request.Session.Clear();

            return this.RedirectResponse(HomePath);
        }

        private void LogInUser(string email)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, email);
            this.Request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}
