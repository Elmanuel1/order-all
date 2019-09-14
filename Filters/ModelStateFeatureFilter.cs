using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SwaggerApp.Filters
{
    public class ModelStateFeatureFilter// : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //await next();
        }
    }
}
