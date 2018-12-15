using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Localization.Core
{
    public static class DistributedCacheExtensions
    {
        /// <summary>فقط یک ترد امکان دسترسی به کد را داشته باشد</summary>
        private static readonly SemaphoreSlim _locker = new SemaphoreSlim(1, 1);

        /// <summary>
        /// A thread-safe way of working with memory cache. First tries to get the key's value from the cache.
        /// Otherwise it will use the factory method to get the value and then inserts it.
        /// </summary>
        public static string GetOrCreateExclusive(this IDistributedCache cache, string cacheKey, Func<string> factory)
        {
            // locks get and set internally
            string value = cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            lock (TypeLock<string>.SyncLock)
            {
                value = cache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }

                value = factory();

                cache.SetString(cacheKey, value);

                return value;
            }
        }

        /// <summary>
        /// A thread-safe way of working with memory cache. First tries to get the key's value from the cache.
        /// Otherwise it will use the factory method to get the value and then inserts it.
        /// </summary>
        public static async Task<string> GetOrAddExclusiveAsync(this IDistributedCache cache, string cacheKey, Func<Task<string>> factory)
        {
            // locks get and set internally
            string value = cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            await _locker.WaitAsync();

            try
            {
                value = cache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }

                value = await factory();

                cache.SetString(cacheKey, value);

                return value;
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
