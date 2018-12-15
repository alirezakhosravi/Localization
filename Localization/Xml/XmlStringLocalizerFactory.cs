using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Localization.Xml
{
    public class XmlStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceProvider _resolver;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache1;
        private readonly IDistributedCache _cache2;
        private readonly Core.LocalizationOptions _options;
        private readonly ConcurrentDictionary<string, XmlMemoryCacheStringLocalizer> _localizerMemoryCache =
            new ConcurrentDictionary<string, XmlMemoryCacheStringLocalizer>();

        private readonly ConcurrentDictionary<string, XmlDistributedCacheStringLocalizer> _localizeristributedCache =
            new ConcurrentDictionary<string, XmlDistributedCacheStringLocalizer>();

        public XmlStringLocalizerFactory(
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
                if (_localizerMemoryCache.TryGetValue(nameof(resourceSource), out XmlMemoryCacheStringLocalizer instance))
                {
                    return instance;
                }

                return _localizerMemoryCache.GetOrAdd(nameof(resourceSource), new XmlMemoryCacheStringLocalizer(nameof(resourceSource), _resolver, _logger, _cache1));

            }

            if (_localizeristributedCache.TryGetValue(nameof(resourceSource), out XmlDistributedCacheStringLocalizer instanse))
            {
                return instanse;
            }

            return _localizeristributedCache.GetOrAdd(nameof(resourceSource), new XmlDistributedCacheStringLocalizer(nameof(resourceSource), _resolver, _logger, _cache2));
        }

        public IStringLocalizer Create(string baseName, string location)
        {

            if (_options.CacheDependency == Core.CacheOption.IMemoryCache)
            {
                if (_localizerMemoryCache.TryGetValue(baseName, out XmlMemoryCacheStringLocalizer instance))
                {
                    return instance;
                }

                return _localizerMemoryCache.GetOrAdd(baseName, new XmlMemoryCacheStringLocalizer(baseName, _resolver, _logger, _cache1));

            }

            if (_localizeristributedCache.TryGetValue(baseName, out XmlDistributedCacheStringLocalizer instanse))
            {
                return instanse;
            }

            return _localizeristributedCache.GetOrAdd(baseName, new XmlDistributedCacheStringLocalizer(baseName, _resolver, _logger, _cache2));
        }
    }
}