using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SwaggerApp.Data;
using SwaggerApp.vo;

namespace SwaggerApp.Exceptions.handler
{
    public class ExceptionHandler
    {
        public static async Task HandleGlobalExceptionAsync(HttpContext context)
        {
            var exception =
                context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
            HttpStatusCode statusCode;

            KoboWashResult result;
            // handle custom errors that will be thrown at by the application
            if (exception is ApplicationException)
            {
                result = new KoboWashResult(((ApplicationException)exception).Code, exception.Message);
                statusCode = HttpStatusCode.BadRequest;
                //return applicationResponse;
            }
            else
            {
                // handle unregistered errors thrown by the application
                statusCode = HttpStatusCode.InternalServerError;
                result = new KoboWashResult(ResponseCodesList.InternalServerError, exception?.Message);
                //return applicationResponse;
            }

            context.Response.StatusCode = (int) statusCode;
            context.Response.ContentType = "application/json";
            context.Response.Headers.Remove("X-ERROR");
            context.Response.Headers.Add("X-ERROR", "true");
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));

        }
    }
}
