namespace WebServer.Server.Http.Response
{
    using Server.Contracts;
    using Enums;
    using Exceptions;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpStatusCode responseCode, IView view)
        {
            this.ValidateStatusCode();
            this.view = view;
            this.StatusCode = responseCode;
        }

        private void ValidateStatusCode()
        {
            var statusCodeNumber = (int)this.StatusCode;

            if (statusCodeNumber > 299 && statusCodeNumber < 400)
            {
                throw new InvalidResponseException("View responses need a status code below 300 and above 400 (inclusive).");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.view.View()}";
        }
    }
}
