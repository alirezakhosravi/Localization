using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Raveshmand.Localization.Xml
{
    public class XmlStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IServiceProvider _resolver;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly Core.LocalizationOptions _options;
        private readonly ConcurrentDictionary<string, XmlMemoryCacheStringLocalizer> _localizerMemoryCache =
            new ConcurrentDictionary<string, XmlMemoryCacheStringLocalizer>();

        private readonly ConcurrentDictionary<string, XmlDistributedCacheStringLocalizer> _localizeristributedCache =
            new ConcurrentDictionary<string, XmlDistributedCacheStringLocalizer>();

        public XmlStringLocalizerFactory(
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
                if (_localizerMemoryCache.TryGetValue(nameof(resourceSource), out XmlMemoryCacheStringLocalizer instance))
                {
                    return instance;
                }

                return _localizerMemoryCache.GetOrAdd(nameof(resourceSource), new XmlMemoryCacheStringLocalizer(nameof(resourceSource), _resolver, _memoryCache));

            }

            if (_localizeristributedCache.TryGetValue(nameof(resourceSource), out XmlDistributedCacheStringLocalizer instanse))
            {
                return instanse;
            }

            return _localizeristributedCache.GetOrAdd(nameof(resourceSource), new XmlDistributedCacheStringLocalizer(nameof(resourceSource), _resolver, _distributedCache));
        }

        public IStringLocalizer Create(string baseName, string location)
        {

            if (_options.CacheDependency == Core.CacheOption.IMemoryCache)
            {
                if (_localizerMemoryCache.TryGetValue(baseName, out XmlMemoryCacheStringLocalizer instance))
                {
                    return instance;
                }

                return _localizerMemoryCache.GetOrAdd(baseName, new XmlMemoryCacheStringLocalizer(baseName, _resolver, _memoryCache));

            }

            if (_localizeristributedCache.TryGetValue(baseName, out XmlDistributedCacheStringLocalizer instanse))
            {
                return instanse;
            }

            return _localizeristributedCache.GetOrAdd(baseName, new XmlDistributedCacheStringLocalizer(baseName, _resolver, _distributedCache));
        }
    }
}