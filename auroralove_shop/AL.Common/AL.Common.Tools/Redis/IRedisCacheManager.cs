using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AL.Common.Tools.Redis
{
    public interface IRedisCacheManager
    {
        bool Exist(string key);

        void Set(string key, object value, DistributedCacheEntryOptions distributedCacheEntryOptions);

        Task SetAsync(string key, object value, DistributedCacheEntryOptions distributedCacheEntryOptions);

        string GetValues(string key);

        T Get<T>(string key, out bool isExisted);

        Task<T> GetAsync<T>(string key);

        void Remove(string key);

        Task RemoveAsync(string key);

        void Refresh(string key);

        Task RefreshAsync(string key);
    }
}
