using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Result
{
    public class Error
    {
        

        public string Description { get; }//Description of the error
        public string Code { get;  } //Status Code 
        public ErrorType type { get;  } //Error Type

        private Error(string description, string code, ErrorType type)
        {
            Description = description;
            Code = code;
            this.type = type;
        }
        //Factory Methods for creating different types of errors
        public static Error Failure(string description="General Failure Occuer", string code= "General.Failure")
        {
            return new Error(description, code, ErrorType.Failure);
        }
        public static Error Validation(string description = "General Validation Occuer", string code = "General.Validation")
        {
            return new Error(description, code, ErrorType.Validation);
        }
        public static Error NotFound(string description = "General NotFound Occuer", string code = "General.NotFound")
        {
            return new Error(description, code, ErrorType.NotFound);
        }
        public static Error Unauthorized(string description = "General Unauthorized Occuer", string code = "General.Unauthorized")
        {
            return new Error(description, code, ErrorType.Unauthorized);
        }
        public static Error Forbidden(string description = "General Forbidden Occuer", string code = "General.Forbidden")
        {
            return new Error(description, code, ErrorType.Forbidden);
        }
        public static Error InvalidCredentials(string description = "General InvalidCredentials Occuer", string code = "General.InvalidCredentials")
        {
            return new Error(description, code, ErrorType.InvalidCredentials);
        }


    }
}
