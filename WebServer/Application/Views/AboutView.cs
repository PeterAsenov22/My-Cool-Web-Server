namespace WebServer.Application.Views
{
    using System.IO;
    using Server.Contracts;

    public class AboutView : IView
    {
        public string View()
        {
            string file = File.ReadAllText("Application\\Resources\\AboutPage.html");
            return file;
        }
    }
}
