using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Localization.Core;
using Localization.Json.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace Localization.Json
{
    public class JsonMemoryCacheStringLocalizer : BaseStringLocalization, IStringLocalizer
    {
        private readonly CultureInfo _culture;
        private readonly IServiceProvider _resolver;
        private readonly string _resourceName;
        private readonly IMemoryCache _cache;

        public JsonMemoryCacheStringLocalizer(
            string resourceName,
            IServiceProvider resolver,
            IMemoryCache cache) : base(resourceName)
        {
            _culture = CultureInfo.CurrentUICulture;
            _resolver = resolver ?? throw new ArgumentException(nameof(resolver));
            _resourceName = resourceName;
            _cache = cache ?? throw new ArgumentException(nameof(cache));
            Connect();
        }

        private void Connect()
        {
            var resource = string.IsNullOrEmpty(_resourceName) ? nameof(LocalizationResourceNames.SharedResource) : _resourceName;
            string computedPath = string.Format(JsonConfiguration.FileName(), CultureInfo.CurrentUICulture.Name, resource);

            List<LocalizationRecord> records = JsonReader.Read<List<LocalizationRecord>>(computedPath);

            Parallel.ForEach(records, (record) => {
                _cache.Set(record.ResourceName, record.Value);
            });
        }

        protected override string GetString(string name)
        {
            return _cache.Get(base.GetString(name))?.ToString() ?? name;
        }
    }
}