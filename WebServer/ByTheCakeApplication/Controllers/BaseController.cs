namespace WebServer.ByTheCakeApplication.Controllers
{
    using Infrastructure;

    public class BaseController : Controller
    {
        protected override string ApplicationDirectory => "ByTheCakeApplication";
    }
}
