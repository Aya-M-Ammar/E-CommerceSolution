using Microsoft.AspNetCore.Mvc;
using Service_Abstaction.BasketService;
using Shared.DTOS.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controller
{
    
    public class BasketsController: ApiBaseController
    {
        private readonly IBasketService _basketservice;

        public BasketsController(IBasketService basketservice)
        {
            _basketservice = basketservice;
        }


        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasketByUserId(string baskeId)
        {
           var basket=await _basketservice.GetBasketByIdAsync(baskeId);
            if (basket == null)
            {
                return NotFound();
            }
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdate([FromBody] BasketDTO basket)
        {
            var createdBasket = await _basketservice.CreateOrUpdateAsync(basket);
            return Ok(createdBasket);
        }
        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>> DeletBasket(string BasketId)
        {
            var result = await _basketservice.DeleteBasket(BasketId);
            return result;
        }

    }
}
