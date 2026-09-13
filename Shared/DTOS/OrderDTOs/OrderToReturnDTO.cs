using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.OrderDTOs
{
    public record OrderToReturnDTO(Guid Id ,string UserEmail,ICollection<OrderItemDTO> items
        ,  string DleviryMethod,string Status,AddsressDTO Address,
        DateTimeOffset OrderDate ,decimal SubTotal,decimal Total );
   
}
