namespace WebServer.Application.Views.CakeViews
{
    using System.IO;
    using Server.Contracts;

    public class AddCakeView : IView
    {
        public string View()
        {
            string file = File.ReadAllText("Application\\Resources\\AddCake.html");
            return file;
        }
    }
}
