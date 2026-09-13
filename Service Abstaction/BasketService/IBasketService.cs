using Shared.DTOS.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Abstaction.BasketService
{
    public interface IBasketService
    {
        public Task<BasketDTO> GetBasketByIdAsync(string basketId);
        public Task<BasketDTO> CreateOrUpdateAsync(BasketDTO basket);
        public Task<bool> DeleteBasket(string basketId);


    }
}
