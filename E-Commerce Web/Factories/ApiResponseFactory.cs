using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult CreateInvalidModelStateResponse(ActionContext actionContext)
        {
            var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                   .ToDictionary(K => K.Key, X => X.Value.Errors.Select(x => x.ErrorMessage).ToArray());
            var Problem = new ProblemDetails()
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Extensions = { ["errors"] = errors },
                Detail = "See the errors property for details."
            };
            return new BadRequestObjectResult(Problem);
        }
    }
}
