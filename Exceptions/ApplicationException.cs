using System;

namespace SwaggerApp.Exceptions
{
   

    public class ApplicationException : Exception
    {
        public string Code { get;  }
        public int Status { get;  }
        
        public ApplicationException(string code, string message, int status) : base(message)
        {
            this.Code = code;
            this.Status = status;
        }

        public static explicit operator ApplicationException(string v)
        {
            throw new NotImplementedException();
        }
    }

    
}
