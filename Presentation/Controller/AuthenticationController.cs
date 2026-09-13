
using Microsoft.AspNetCore.Mvc;
using Service_Abstaction.AuthinticationService;
using Shared.DTOS.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace E_Commerce.Presentation.Controller
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationUser _authenticationUser;

        public AuthenticationController(IAuthenticationUser authenticationUser)
        {
            this._authenticationUser = authenticationUser;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var result = await _authenticationUser.LoginAsync(loginDTO);
            return HandleResult(result);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var result = await _authenticationUser.RegisterAsync(registerDTO);
            return HandleResult(result);
        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var Result =await _authenticationUser.CheckEmail(email);
            return Ok(Result);

        }
        [HttpGet("GetUser")]

        public async Task<ActionResult<UserDTO>> GetUser()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var user=await _authenticationUser.GetUser(email);
            return HandleResult(user);
        }

    }
}