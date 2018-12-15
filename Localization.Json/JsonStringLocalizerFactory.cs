using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Localization.Json
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IServiceProvider _resolver;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly Core.LocalizationOptions _options;
        private readonly ConcurrentDictionary<string, JsonMemoryCacheStringLocalizer> _localizerMemoryCache =
            new ConcurrentDictionary<string, JsonMemoryCacheStringLocalizer>();

        private readonly ConcurrentDictionary<string, JsonDistributedCacheStringLocalizer> _localizeristributedCache =
            new ConcurrentDictionary<string, JsonDistributedCacheStringLocalizer>();

        public JsonStringLocalizerFactory(
            IServiceProvider resolver,
            IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            IOptions<Core.LocalizationOptions> option)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _options = option.Value;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            if (_options.CacheDependency == Core.CacheOption.IMemoryCache)
            {
                if (_localizerMemoryCache.TryGetValue(nameof(resourceSource), out JsonMemoryCacheStringLocalizer instance))
                {
                    return instance;
                }

                return _localizerMemoryCache.GetOrAdd(nameof(resourceSource), new JsonMemoryCacheStringLocalizer(nameof(resourceSource), _resolver, _memoryCache));

            }

            if (_localizeristributedCache.TryGetValue(nameof(resourceSource), out JsonDistributedCacheStringLocalizer instanse))
            {
                return instanse;
            }

            return _localizeristributedCache.GetOrAdd(nameof(resourceSource), new JsonDistributedCacheStringLocalizer(nameof(resourceSource), _resolver, _distributedCache));
        }

        public IStringLocalizer Create(string baseName, string location)
        {

            if (_options.CacheDependency == Core.CacheOption.IMemoryCache)
            {
                if (_localizerMemoryCache.TryGetValue(baseName, out JsonMemoryCacheStringLocalizer instance))
                {
                    return instance;
                }

                return _localizerMemoryCache.GetOrAdd(baseName, new JsonMemoryCacheStringLocalizer(baseName, _resolver, _memoryCache));

            }

            if (_localizeristributedCache.TryGetValue(baseName, out JsonDistributedCacheStringLocalizer instanse))
            {
                return instanse;
            }

            return _localizeristributedCache.GetOrAdd(baseName, new JsonDistributedCacheStringLocalizer(baseName, _resolver, _distributedCache));
        }
    }
}