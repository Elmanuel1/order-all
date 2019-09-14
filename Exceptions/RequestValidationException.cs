using SwaggerApp.Data;

namespace SwaggerApp.Exceptions
{
    public class RequestValidationException: KoboWashException
    {
        public RequestValidationException() : base(ResponseCodesList.RequestValidationError, "Invalid Request parameters. Please review. ")
        {

        }

        public RequestValidationException(string message) : base(ResponseCodesList.RequestValidationError, message)
        {

        }
    }
}
