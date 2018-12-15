using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Internal;

namespace Localization.Json
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceProvider _resolver;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache1;
        private readonly IDistributedCache _cache2;
        private readonly Core.LocalizationOptions _options;
        private readonly ConcurrentDictionary<string, JsonMemoryCacheStringLocalizer> _localizerMemoryCache =
            new ConcurrentDictionary<string, JsonMemoryCacheStringLocalizer>();

        private readonly ConcurrentDictionary<string, JsonDistributedCacheStringLocalizer> _localizeristributedCache =
            new ConcurrentDictionary<string, JsonDistributedCacheStringLocalizer>();

        public JsonStringLocalizerFactory(
            ILoggerFactory loggerFactory,
            IServiceProvider resolver,
            ILogger logger,
            IMemoryCache cache1,
            IDistributedCache cache2,
            IOptions<Core.LocalizationOptions> option)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache1 = cache1;
            _cache2 = cache2;
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

                return _localizerMemoryCache.GetOrAdd(nameof(resourceSource), new JsonMemoryCacheStringLocalizer(nameof(resourceSource), _resolver, _logger, _cache1));

            }

            if (_localizeristributedCache.TryGetValue(nameof(resourceSource), out JsonDistributedCacheStringLocalizer instanse))
            {
                return instanse;
            }

            return _localizeristributedCache.GetOrAdd(nameof(resourceSource), new JsonDistributedCacheStringLocalizer(nameof(resourceSource), _resolver, _logger, _cache2));
        }

        public IStringLocalizer Create(string baseName, string location)
        {

            if (_options.CacheDependency == Core.CacheOption.IMemoryCache)
            {
                if (_localizerMemoryCache.TryGetValue(baseName, out JsonMemoryCacheStringLocalizer instance))
                {
                    return instance;
                }

                return _localizerMemoryCache.GetOrAdd(baseName, new JsonMemoryCacheStringLocalizer(baseName, _resolver, _logger, _cache1));

            }

            if (_localizeristributedCache.TryGetValue(baseName, out JsonDistributedCacheStringLocalizer instanse))
            {
                return instanse;
            }

            return _localizeristributedCache.GetOrAdd(baseName, new JsonDistributedCacheStringLocalizer(baseName, _resolver, _logger, _cache2));
        }
    }
}