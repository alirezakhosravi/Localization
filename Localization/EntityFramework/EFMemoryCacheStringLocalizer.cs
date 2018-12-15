using System;
using System.Globalization;
using System.Linq;
using Localization.Core;
using Localization.Coreson;
using Localization.EntityFramework.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.EntityFramework
{
    public class EFMemoryCacheStringLocalizer<TContext> : BaseStringLocalization, IStringLocalizer
        where TContext : DbContext
    {
        private readonly ILogger _logger;
        private readonly CultureInfo _culture;
        private readonly IServiceProvider _resolver;
        private readonly string _resourceName;
        private readonly IMemoryCache _cache;

        public EFMemoryCacheStringLocalizer(
            string resourceName,
            IServiceProvider resolver,
            ILogger logger,
            IMemoryCache cache) : base(resourceName)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _culture = CultureInfo.CurrentUICulture;
            _resolver = resolver ?? throw new ArgumentException(nameof(resolver));
            _resourceName = resourceName;
            _cache = cache ?? throw new ArgumentException(nameof(cache));
        }

        protected override string GetString(string name)
        {
            string value = string.Empty;

            value = _cache.GetOrCreateExclusive(base.GetString(name), () =>
            {
                return _resolver.RunScopedService<string, TContext>(context =>
                 {
                     return value = context.Set<LocalizationRecord>()
                     .SingleOrDefault(a => a.Name == name && a.CultureName == CultureInfo.CurrentUICulture.Name)
                      ?.Value ?? name;
                 });
            });

            return value;
        }
    }
}