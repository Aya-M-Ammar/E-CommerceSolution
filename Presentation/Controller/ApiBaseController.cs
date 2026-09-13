using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing.Template;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ApiBaseController:ControllerBase
    {
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();//204
            else
            {
                return HandelProblem(result.Errors);
            }
        }
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess )
                return Ok(result.Value);//200
            else
            {
                return HandelProblem(result.Errors);
            }
        }

        private ActionResult HandelProblem(IReadOnlyList<Error> errors)
        {
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An UxpextedError");

            if(errors.All(e => e.type == ErrorType.Validation))
                return HandelValidationProblem(errors);
            return HandelSingleErrorProblem(errors[0]);
        }

        private ActionResult HandelSingleErrorProblem(Error error)
        {
            return Problem(

                     title: error.Code,
                     detail: error.Description,
                     statusCode : MapErrorTypeToStatusCode(error.type)
                );  



        }

        private static int MapErrorTypeToStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Failuer => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private ActionResult HandelValidationProblem(IReadOnlyList<Error> errors)
        {
            var modelsatate = new ModelStateDictionary();
            foreach (var error in errors)
           
                modelsatate.AddModelError(error.Code, error.Description);
            

            return ValidationProblem(modelsatate);
        }
    }
}
