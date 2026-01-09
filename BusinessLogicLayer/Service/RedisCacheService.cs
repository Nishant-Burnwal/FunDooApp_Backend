using BusinessLogicLayer.Interface;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace BusinessLogicLayer.Service
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public void Set<T>(string key, T value, TimeSpan expiry)
        {
            var json = JsonSerializer.Serialize(value);
            _db.StringSet(key, json, expiry);
        }

        public T Get<T>(string key)
        {
            var value = _db.StringGet(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public void Remove(string key)
        {
            _db.KeyDelete(key);
        }
    }
}
