using E_Commerce.Domain.Entity.BasketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.BasketDTOs;

namespace E_Commerce.Domain.Interfaces.Repository
{
    public interface IBasketRepository
    {
        public Task<BasketCustomer?> GetBasketAsync(string BasketId);
        public Task<BasketCustomer?> CreateOrUpdateBasketAsync(BasketCustomer basket,TimeSpan timeSpan=default );
        public Task<bool> DeleteBasketAsync(string BasketId);
    }
}
