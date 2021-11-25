using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Redis
{
    public interface IRedisCache
    {
        void Create(string key, object value, TimeSpan? expiry = null);
        Task<T> Read<T>(string key);
        void Update(string key, object value, TimeSpan? expiry = null);
        void Delete(string key);
    }
}
