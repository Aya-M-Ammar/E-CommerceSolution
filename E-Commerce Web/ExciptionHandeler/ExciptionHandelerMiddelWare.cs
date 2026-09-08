using Microsoft.AspNetCore.Mvc;
using Service_Implementation.Exciptions;

namespace E_Commerce_Web.ExciptionHandeler
{
    public class ExciptionHandelerMiddelWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExciptionHandelerMiddelWare> _logger;

        public ExciptionHandelerMiddelWare(RequestDelegate next, ILogger<ExciptionHandelerMiddelWare> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                await  HandelNotFoundMiddelwareAsync(context);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
               
                var Problem= new ProblemDetails
                {
                   
                    Title = "An error occurred while processing your request.",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Status = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                }
            };
            
              await  context.Response.WriteAsJsonAsync(Problem);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }

        private static async Task  HandelNotFoundMiddelwareAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound&&!context.Response.HasStarted)
            {
                var Problem = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found.",
                    Detail = $"The requested resource '{context.Request.Path}' was not found.",
                    Instance = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }

}
