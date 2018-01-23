namespace WebServer.Server.Http.Response
{
    using Enums;
    using Common;

    public class NotFoundResponse : ViewResponse
    {
        public NotFoundResponse()
            :base(HttpStatusCode.NotFound, new NotFoundView())
        {
        }
    }
}
