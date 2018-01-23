namespace WebServer.Server.Exceptions
{
    using System;

    public class BadRequestException : Exception
    {
        private const string BadRequestExceptionMessage = "Request is not valid.";

        public static object ThrowFromInvalidRequest()
        {
            throw new BadRequestException(BadRequestExceptionMessage);
        }

        public BadRequestException(string message)
            :base(message) { }
    }
}
