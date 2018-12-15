using System;
using System.Globalization;
using System.Linq;
using Localization.Core;
using Localization.Core.Extentions;
using Localization.EntityFramework.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace Localization.EntityFramework
{
    public class EFDistributedCacheStringLocalizer<TContext> : BaseStringLocalization, IStringLocalizer
        where TContext : DbContext
    {
        private readonly CultureInfo _culture;
        private readonly IServiceProvider _resolver;
        private readonly string _resourceName;
        private readonly IDistributedCache _cache;

        public EFDistributedCacheStringLocalizer(
            string resourceName,
            IServiceProvider resolver,
            IDistributedCache cache):base(resourceName)
        {
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