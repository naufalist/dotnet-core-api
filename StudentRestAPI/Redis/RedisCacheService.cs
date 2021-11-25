using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentRestAPI.Redis
{
    public class RedisCacheService : IRedisCache
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase RedisStudent;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            RedisStudent = _connectionMultiplexer.GetDatabase();
        }

        public void Create(string key, object value, TimeSpan? expiry = null)
        {
            RedisStudent.StringSetAsync(key.Trim(), JsonSerializer.Serialize(value), expiry, flags: CommandFlags.FireAndForget);
        }

        public async Task<T> Read<T>(string key)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(await RedisStudent.StringGetAsync(key.Trim()));
            }
            catch
            {
                return default(T);
            }
        }

        public void Update(string key, object value, TimeSpan? expiry = null)
        {
            Delete(key);
            Create(key, value, expiry);
        }

        public void Delete(string key)
        {
            RedisStudent.KeyDeleteAsync(key.Trim(), flags: CommandFlags.FireAndForget);
        }

    }
}
