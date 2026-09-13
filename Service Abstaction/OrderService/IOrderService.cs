using Shared.DTOS.OrderDTOs;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Abstaction.OrderService
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrder(string Email,OrderDTO orderDTO);
    }
}
