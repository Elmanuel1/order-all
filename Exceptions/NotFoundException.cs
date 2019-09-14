using SwaggerApp.Data;

namespace SwaggerApp.Exceptions
{
    public class NotFoundException : KoboWashException
    {
        
        public NotFoundException() : base(ResponseCodesList.NotFound, "No values was found")
        {

        }

        public NotFoundException(string message) : base(ResponseCodesList.NotFound, message)
        {
            
        }
    }

    
}
