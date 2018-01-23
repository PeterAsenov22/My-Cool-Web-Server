namespace WebServer.Application.Views.CakeViews
{
    using System.IO;
    using Server.Contracts;

    public class BrowseCakeView : IView
    {
        public string View()
        {
            string file = File.ReadAllText("Application\\Resources\\BrowseCake.html");
            return file;
        }
    }
}
