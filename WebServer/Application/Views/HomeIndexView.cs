namespace WebServer.Application.Views
{
    using Server.Contracts;

    public class HomeIndexView : IView
    {
        private readonly string htmlFile;

        public HomeIndexView(string htmlFile)
        {
            this.htmlFile = htmlFile;
        }

        public string View()
        {         
            return htmlFile;
        }
    }
}
