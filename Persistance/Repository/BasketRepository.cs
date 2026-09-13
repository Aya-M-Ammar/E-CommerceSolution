using E_Commerce.Domain.Entity.BasketModel;
using E_Commerce.Domain.Interfaces.Repository;
using Shared.DTOS.BasketDTOs;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

      

        public async Task<BasketCustomer?> CreateOrUpdateBasketAsync(BasketCustomer basket, TimeSpan timeSpan = default)
        {
            var IsCreated = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), (timeSpan == default) ? TimeSpan.FromDays(7) : timeSpan);

            if (IsCreated)
            {  return JsonSerializer.Deserialize<BasketCustomer>(JsonSerializer.Serialize(basket)); 
            }
            return null;
        }

        public async Task<bool> DeleteBasketAsync(string BasketId)=> await _database.KeyDeleteAsync(BasketId);

        public async Task<BasketCustomer?> GetBasketAsync(string BasketId)
        {
            var Basket=await _database.StringGetAsync(BasketId);
            if(Basket.IsNullOrEmpty)
            {
                return null;
            }
            else
            {
                return  JsonSerializer.Deserialize<BasketCustomer>(Basket!);
            }
        }
    }
}
