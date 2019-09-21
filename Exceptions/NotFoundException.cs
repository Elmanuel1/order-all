using SwaggerApp.Data;

namespace SwaggerApp.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        
        public NotFoundException() : base(ResponseCodesList.NotFound, "No values was found", 404)
        {

        }

        public NotFoundException(string message) : base(ResponseCodesList.NotFound, message, 404)
        {
            
        }
    }

    
}
