using System;

namespace SwaggerApp.Exceptions
{
   

    public class KoboWashException : Exception
    {
        public string Code { get; set; }
        public KoboWashException(string code, string message) : base(message)
        {
            this.Code = code;
        }

        public static explicit operator KoboWashException(string v)
        {
            throw new NotImplementedException();
        }
    }

    
}
