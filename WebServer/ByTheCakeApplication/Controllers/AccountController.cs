namespace WebServer.ByTheCakeApplication.Controllers
{
    using Server.Http;
    using Infrastructure;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Models;


    public class AccountController : Controller
    {
        public IHttpResponse Login()
        {
            this.ViewData["showError"] = "none";
            this.ViewData["authDisplay"] = "none";

            return this.FileViewResponse(@"account\login");
        }

        public IHttpResponse Login(IHttpRequest req)
        {
            const string formUsernameKey = "username";
            const string formPasswordKey = "password";

            if (!req.FormData.ContainsKey(formPasswordKey) || !req.FormData.ContainsKey(formUsernameKey))
            {
                return new BadRequestResponse();
            }

            var username = req.FormData[formUsernameKey];
            var password = req.FormData[formPasswordKey];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                this.ViewData["error"] = "You have empty fields.";
                this.ViewData["showError"] = "block";
                this.ViewData["authDisplay"] = "none";

                return this.FileViewResponse(@"account\login");
            }

            req.Session.Add(SessionStore.CurrentUserKey,username);
            req.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());

            

            return new RedirectResponse("/");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }
    }
}
