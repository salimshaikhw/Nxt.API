using System;

namespace Nxt.Common.Helpers.MemoryCache
{
    public interface IMemoryCacheHelper
    {
        T GetCache<T>(string key);
        void RemoveCache(string key);
        void SetCache<T>(string key, T value, TimeSpan expirationTimeSpan);
    }
}
