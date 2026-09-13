using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.OrderDTOs
{
    public record OrderDTO(string BasketId,int DeliveryMethod,AddsressDTO Address);
}
