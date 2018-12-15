using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Localization.Core
{
    public static class MemoryCacheExtensions
    {
        /// <summary>فقط یک ترد امکان دسترسی به کد را داشته باشد</summary>
        private static readonly SemaphoreSlim _locker = new SemaphoreSlim(1, 1);

        /// <summary>
        /// A thread-safe way of working with memory cache. First tries to get the key's value from the cache.
        /// Otherwise it will use the factory method to get the value and then inserts it.
        /// </summary>
        public static T GetOrCreateExclusive<T>(this IMemoryCache cache, string cacheKey, Func<T> factory,
            DateTimeOffset? absoluteExpiration = null)
        {
            // locks get and set internally
            if (cache.TryGetValue<T>(cacheKey, out var result))
            {
                return result;
            }

            lock (TypeLock<T>.SyncLock)
            {
                if (cache.TryGetValue(cacheKey, out result))
                {
                    return result;
                }

                result = factory();

                if (absoluteExpiration.HasValue)
                {
                    cache.Set(cacheKey, result, absoluteExpiration.Value);
                }
                else
                {
                    cache.Set(cacheKey, result);
                }

                return result;
            }
        }

        /// <summary>
        /// A thread-safe way of working with memory cache. First tries to get the key's value from the cache.
        /// Otherwise it will use the factory method to get the value and then inserts it.
        /// </summary>
        public static async Task<T> GetOrAddExclusiveAsync<T>(this IMemoryCache cache, string cacheKey, Func<Task<T>> factory,
            DateTimeOffset? absoluteExpiration = null)
        {
            // locks get and set internally
            if (cache.TryGetValue<T>(cacheKey, out var result))
            {
                return result;
            }

            await _locker.WaitAsync();

            try
            {
                if (cache.TryGetValue(cacheKey, out result))
                {
                    return result;
                }

                result = await factory();

                if (absoluteExpiration.HasValue)
                {
                    cache.Set(cacheKey, result, absoluteExpiration.Value);
                }
                else
                {
                    cache.Set(cacheKey, result);
                }

                return result;
            }
            finally
            {
                _locker.Release();
            }
        }

        private static class TypeLock<T>
        {
            public static readonly object SyncLock = new object();
        }
    }
}
