namespace WebServer.Application.Controllers
{
    using Server.Enums;
    using Server.Http.Response;
    using Views.CakeViews;

    public class CakeController
    {
        public HttpResponse AddCake()
        {
            return new ViewResponse(HttpStatusCode.Ok, new AddCakeView());
        }

        public HttpResponse BrowseCake()
        {
            return new ViewResponse(HttpStatusCode.Ok, new BrowseCakeView());
        }
    }
}
