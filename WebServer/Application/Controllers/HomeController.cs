using System;
using System.IO;
using WebServer.Server.Http.Contracts;

namespace WebServer.Application.Controllers
{
    using Views;
    using Server.Enums;
    using Server.Http.Response;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            string htmlFile = File.ReadAllText("Application\\Resources\\MainPage.html");

            var response = new ViewResponse(HttpStatusCode.Ok, new HomeIndexView(htmlFile));

            response.Cookies.Add("lang","en");

            return response;
        }

        public IHttpResponse About()
        {
            return new ViewResponse(HttpStatusCode.Ok, new AboutView());
        }

        public IHttpResponse SessionTest(IHttpRequest request)
        {
            const string sessionDateKey = "saved_date";

            var session = request.Session;

            if (session.Get(sessionDateKey) == null)
            {
                session.Add(sessionDateKey, DateTime.UtcNow);
            }

            return new ViewResponse(HttpStatusCode.Ok, new SessionTestView(session.Get<DateTime>(sessionDateKey)));
        }
    }
}
