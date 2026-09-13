using Shared.DTOS.IdentityDTOs;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Abstaction.AuthinticationService
{
    public interface IAuthenticationUser
    {
        public Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        public Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
        public Task<bool> CheckEmail(string email);
        public Task<Result<UserDTO>> GetUser(string email);

    }
}
