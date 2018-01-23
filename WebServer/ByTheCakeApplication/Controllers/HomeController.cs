namespace WebServer.ByTheCakeApplication.Controllers
{
    using Server.Http.Contracts;
    using Infrastructure;

    public class HomeController : Controller
    {
        public IHttpResponse Index() => this.FileViewResponse(@"home\index");

        public IHttpResponse About() => this.FileViewResponse(@"home\about");

        public IHttpResponse Tools() => this.FileViewResponse(@"home\tools");

        public IHttpResponse Calculator()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(@"calculator\calculator");
        }

        public IHttpResponse Calculator(string firstNumber, string sign, string secondNumber)
        {
            var result = Models.Calculator.Calculate(decimal.Parse(firstNumber), decimal.Parse(secondNumber), sign);

            this.ViewData["showResult"] = "block";
            this.ViewData["result"] = $"{result:f2}";

            return this.FileViewResponse(@"calculator\calculator");
        }

        public IHttpResponse Email()
        {
            this.ViewData["result"] = string.Empty;

            return this.FileViewResponse(@"home\email");
        }

        public IHttpResponse Email(string receiver, string subject, string message)
        {
            string result = $"You sucessfully send an email to {receiver} about {subject} with message: \"{message}\"";

            this.ViewData["result"] = result;

            return this.FileViewResponse(@"home\email");
        }
    }
}
