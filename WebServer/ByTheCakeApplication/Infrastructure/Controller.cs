namespace WebServer.ByTheCakeApplication.Infrastructure
{
    using System.IO;
    using Views;
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Controller
    {
        public const string DefaultPath = @"ByTheCakeApplication\Resources\{0}.html";
        public const string ContentPlaceholder = "{{{content}}}";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["authDisplay"] = "block"
            };
        }

        protected IDictionary<string,string> ViewData { get; private set; }

        protected IHttpResponse FileViewResponse(string fileName)
        {
            var html = this.ProcessFileHtml(fileName);

            if (this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    html = html.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(html));
        }

        private string ProcessFileHtml(string fileName)
        {
            string layoutHtml = File.ReadAllText(string.Format(DefaultPath, @"home\layout"));
            string htmlFile = File.ReadAllText(string.Format(DefaultPath, fileName));

            string finalHtml = layoutHtml.Replace(ContentPlaceholder, htmlFile);
            return finalHtml;
        }
    }
}
