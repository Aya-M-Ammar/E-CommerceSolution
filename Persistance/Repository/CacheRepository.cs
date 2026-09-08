using E_Commerce.Domain.Interfaces.Repository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Repository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();


        }
        public async Task<string?> GetAsync(string CacheKey)
        {
            var value = await _database.StringGetAsync(CacheKey);
            if(value.IsNullOrEmpty)
            {
                return null;
            }
            return value.ToString();
        }

        public async Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeYoLive)
        {
           await _database.StringSetAsync(CacheKey, CacheValue, TimeYoLive);
        }
    }
}
