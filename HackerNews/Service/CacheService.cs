using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Service
{
    public class CacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void SetCache(string cacheKey, object value, TimeSpan expirationTime)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expirationTime);
            _cache.Set(cacheKey, value, cacheOptions);
        }

        public bool TryGetCache<T>(string cacheKey, out T value)
        {
            return _cache.TryGetValue(cacheKey, out value);
        }
    }


}
