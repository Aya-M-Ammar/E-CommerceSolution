

using E_Commerce.Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service_Abstaction.AuthinticationService;
using Shared.DTOS.IdentityDTOs;
using Shared.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service_Implementation.AuthenticationService
{
    public class AuthenticationUser : IAuthenticationUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationUser(UserManager<ApplicationUser> userManager,IConfiguration configuration )
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<bool> CheckEmail(string email)
        {
           var user=await _userManager.FindByEmailAsync(email);
                    return user != null;

        }

        public async Task<Result<UserDTO>> GetUser(string email)
        {
            var user=await _userManager.FindByEmailAsync(email);
            //used result pattern to return error if user not found instead of throwing exception
            if (user == null)
                return Error.NotFound( $"User By Email {email} Not Found", "User.NotFound");
            //if result is success return userDTO with token
            return new UserDTO(user.Email!,user.DisplayName,await CreateToken(user));  

        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            //expected user enter invalid email or password mustn't be thrown exception can used result pattern
            //but we don't want to tell them which one is invalid for security reason so we return same error message for both cases
            if (User == null)
                return Error.InvalidCredentials("Invalid Email or Password");
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (!IsPasswordValid)
                return Error.InvalidCredentials("Invalid Email or Password");
            return new UserDTO(User.Email!, User.DisplayName,await CreateToken(User));
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber
            };
            var result = await _userManager.CreateAsync(User, registerDTO.Password);
            if (result.Succeeded)
            {
                var Token =await CreateToken(User);
                return new UserDTO(User.Email!, User.DisplayName, Token);
            }
            return result.Errors.Select(e => Error.Validation(e.Description, e.Code)).ToList();
        }


        private async Task<string> CreateToken(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
               // new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName!)
            };

            var Roles =await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role,role));
            }

            var secritKey = _configuration["JWTOptions:SecretKey"];
            var Key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secritKey));
            var Cred=new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: Claims,
                signingCredentials: Cred,
                expires: DateTime.UtcNow.AddHours(3)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}




