using SwaggerApp.Data;

namespace SwaggerApp.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException() : base(ResponseCodesList.BadRequest, "Bad Request Received. Please review!", 400)
        {

        }

        public BadRequestException(string message) : base(ResponseCodesList.BadRequest, message, 400)
        {
            
        }
    }
}