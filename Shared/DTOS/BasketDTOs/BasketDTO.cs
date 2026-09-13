using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.BasketDTOs
{
    public record BasketDTO(string Id, ICollection<BasketItemDTO> items);
   

}
