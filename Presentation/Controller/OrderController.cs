using Microsoft.AspNetCore.Mvc;
using Service_Abstaction.OrderService;
using Shared.DTOS.OrderDTOs;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controller
{
    public class OrderController:ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //CreateOrder(string Email, OrderDTO orderDTO)
        [HttpPost]
        public async Task<ActionResult<Result<OrderToReturnDTO>>> CreateOrder(string Email, [FromBody] OrderDTO orderDTO)
        {
            var result = await _orderService.CreateOrder(Email, orderDTO);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Errors);

        }

    }
}
