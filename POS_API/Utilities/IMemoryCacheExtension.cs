using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace POS_API.Utilities
{
    public static class IMemoryCacheExtension
    {
        public static async Task<IList<T>> GetListFromCache<T>(this IMemoryCache cache, string cacheKey, IQueryable<T> query, double absoluteExpiration, double slidingExpiration, CacheItemPriority cacheItemPriority)
        {
            if (!cache.TryGetValue(cacheKey, out IList<T> dataList))
            {
                dataList = await query.ToListAsync<T>();
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                                         {
                                             AbsoluteExpiration = DateTime.Now.AddMinutes(absoluteExpiration),
                                             Priority = cacheItemPriority,
                                             SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
                                         };
                cache.Set(cacheKey, dataList, cacheExpiryOptions);
            }
            return dataList;
        }
        public static void SetListToCache<T>(this IMemoryCache cache , string cacheKey, IList<T> dataList, double absoluteExpiration, double slidingExpiration, CacheItemPriority cacheItemPriority)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(absoluteExpiration),
                Priority = cacheItemPriority,
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
            };
            cache.Set(cacheKey, dataList, cacheExpiryOptions);
        }
        public static bool IsAvailableInCache(this IMemoryCache cache, string cacheKey)
        {
            return cache.TryGetValue(cacheKey, out dynamic _);
        }
        public static bool UpdateListToCache<T>(this IMemoryCache cache, string cacheKey, IList<T> dataList, double absoluteExpiration, double slidingExpiration, CacheItemPriority cacheItemPriority)
        {
            if (cache.TryGetValue(cacheKey,
                                         out IList<T> _))
            {
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(absoluteExpiration),
                    Priority = cacheItemPriority,
                    SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
                };
                cache.Set(cacheKey, dataList, cacheExpiryOptions);
                return true;
            }
            return false;
        }
        public static async Task<T> GetFromCache<T>(this IMemoryCache cache, string cacheKey, IQueryable<T> query, double absoluteExpiration, double slidingExpiration, CacheItemPriority cacheItemPriority)
        {
            if (!cache.TryGetValue(cacheKey, out T data))
            {
                data = await query.FirstOrDefaultAsync<T>();
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(absoluteExpiration),
                    Priority = cacheItemPriority,
                    SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
                };
                cache.Set(cacheKey, data, cacheExpiryOptions);
            }
            return data;
        }

        public static void SetToCache<T>(this IMemoryCache cache, string cacheKey, T data, double absoluteExpiration, double slidingExpiration, CacheItemPriority cacheItemPriority)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(absoluteExpiration),
                Priority = cacheItemPriority,
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
            };
            cache.Set(cacheKey, data, cacheExpiryOptions);
        }
        public static bool UpdateToCache<T>(this IMemoryCache cache, string cacheKey, T data, double absoluteExpiration, double slidingExpiration, CacheItemPriority cacheItemPriority)
        {
            if (cache.TryGetValue(cacheKey, out T temp))
            {
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(absoluteExpiration),
                    Priority = cacheItemPriority,
                    SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
                };
                cache.Set(cacheKey, data, cacheExpiryOptions);
                return true;
            }
            return false;
        }
    }
}
