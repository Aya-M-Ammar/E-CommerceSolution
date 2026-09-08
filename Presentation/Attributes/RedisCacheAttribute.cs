using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Service_Abstaction.CacheService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace E_Commerce.Presentation.Attributes
{
    public class RedisCacheAttribute:ActionFilterAttribute
    {
        private readonly int _durationInMinutes;

        public RedisCacheAttribute(int durationInMinutes=5)
        {
         _durationInMinutes = durationInMinutes;
        }
        override public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var cacheValue = await cacheService.GetAsync(cacheKey);
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var ExcutedContext=await next.Invoke();
            if(ExcutedContext.Result is OkObjectResult okResult)
            {
                
                await cacheService.SetAsync(cacheKey, okResult, TimeSpan.FromMinutes(_durationInMinutes));
            }

         
        }
        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);
            foreach (var (key, value) in request.Query.OrderBy(q => q.Key))
            {
                keyBuilder.Append($"|{key}:{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
