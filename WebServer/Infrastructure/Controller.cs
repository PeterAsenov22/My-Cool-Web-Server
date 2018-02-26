namespace WebServer.Infrastructure
{
    using System.IO;
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    public abstract class Controller
    {
        public const string DefaultPath = @"{0}\Resources\{1}.html";
        public const string ContentPlaceholder = "{{{content}}}";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["anonymousDisplay"] = "none",
                ["authDisplay"] = "flex",
                ["showError"] = "none"
            };
        }

        protected abstract string ApplicationDirectory { get; }

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

        protected IHttpResponse RedirectResponse(string route)
            => new RedirectResponse(route);

        protected void AddError(string errorMessage)
        {
            this.ViewData["error"] = $"{errorMessage}";
            this.ViewData["showError"] = "block";
        }

        protected bool ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(model, context, results, true) == false)
            {
                foreach (var result in results)
                {
                    if (result != ValidationResult.Success)
                    {
                        this.AddError(result.ErrorMessage);
                        return false;
                    }
                }
            }

            return true;
        }

        private string ProcessFileHtml(string fileName)
        {
            string layoutHtml = File.ReadAllText(string.Format(DefaultPath, this.ApplicationDirectory, "layout"));
            string htmlFile = File.ReadAllText(string.Format(DefaultPath, this.ApplicationDirectory, fileName));

            string finalHtml = layoutHtml.Replace(ContentPlaceholder, htmlFile);
            return finalHtml;
        }
    }
}
