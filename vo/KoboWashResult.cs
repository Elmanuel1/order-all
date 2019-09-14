using System;

namespace SwaggerApp.vo
{
    public class KoboWashResult
    {
        public DateTime ResponseTime = DateTime.Now;

        public String ResponseCode { get; set; }

        public String ResponseMessage { get; set; }

        public Object Data { get; set; }

        public String RequestId { get {  return Guid.NewGuid().ToString("N"); } }

        public KoboWashResult(string responseCode, string responseMessage)
        {
            this.ResponseCode = responseCode;
            this.ResponseMessage = responseMessage;
        }

    }
    
}
