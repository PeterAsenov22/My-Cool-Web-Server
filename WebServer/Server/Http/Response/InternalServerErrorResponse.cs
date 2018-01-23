using WebServer.Server.Common;

namespace WebServer.Server.Http.Response
{
    using System;
    using Enums;

    public class InternalServerErrorResponse : ViewResponse
    {
        public InternalServerErrorResponse(Exception exception)
            :base(HttpStatusCode.InternalServerError, new InternalServerErrorView(exception)) { }
    }
}
