namespace WebServer.ByTheCakeApplication.Controllers
{
    using Server.Http.Contracts;
    using Infrastructure;

    public class HomeController : Controller
    {
        public IHttpResponse Index() => this.FileViewResponse(@"home\index");

        public IHttpResponse About() => this.FileViewResponse(@"home\about");
    }
}
