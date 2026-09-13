using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.IdentityDTOs
{
    public record RegisterDTO( [EmailAddress] string Email, string UserName, string DisplayName, string Password,[Phone]string PhoneNumber);


}
