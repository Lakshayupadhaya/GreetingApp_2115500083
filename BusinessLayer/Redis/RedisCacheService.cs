using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace BusinessLayer.Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        //private readonly IDistributedCache _cache;
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }


        public  void SetCache<T>(string key, T value, TimeSpan expiration)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            var serializedValue = JsonSerializer.Serialize(value);
            _cache.StringSet(key, serializedValue, expiration);
        }

        public T GetCache<T>(string key)
        {
            var cachedValue =_cache.StringGet(key);
            if (string.IsNullOrEmpty(cachedValue))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(cachedValue);
        }

        public void RemoveCache(string key)
        {
            _cache.KeyDelete(key);
        }
    }
}
