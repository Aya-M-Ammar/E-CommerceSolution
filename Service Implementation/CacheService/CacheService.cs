using E_Commerce.Domain.Interfaces.Repository;
using Service_Abstaction.CacheService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service_Implementation.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
        public async Task<string?> GetAsync(string CacheKey)
        {
            return await _cacheRepository.GetAsync(CacheKey);
        }

        public Task SetAsync(string CacheKey, object CacheValue, TimeSpan TimeYoLive)
        {
            var cacheValueString = JsonSerializer.Serialize(CacheValue,new JsonSerializerOptions 
            { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return _cacheRepository.SetAsync(CacheKey, cacheValueString, TimeYoLive);
        }
    }
}
