using SwaggerApp.Data;

namespace SwaggerApp.Exceptions
{
    public class RequestValidationException: ApplicationException
    {
        public RequestValidationException() : base(ResponseCodesList.RequestValidationError, "Invalid Request parameters. Please review. ", 400)
        {

        }

        public RequestValidationException(string message) : base(ResponseCodesList.RequestValidationError, message, 400)
        {

        }
    }
}
