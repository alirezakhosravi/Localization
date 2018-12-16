using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Raveshmand.Localization.Core;
using Raveshmand.Localization.Json.IO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace Raveshmand.Localization.Json
{
    public class JsonDistributedCacheStringLocalizer : BaseStringLocalization, IStringLocalizer
    {
        private readonly CultureInfo _culture;
        private readonly IServiceProvider _resolver;
        private readonly string _resourceName;
        private readonly IDistributedCache _cache;

        public JsonDistributedCacheStringLocalizer(
            string resourceName,
            IServiceProvider resolver,
            IDistributedCache cache) : base(resourceName)
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

            Parallel.ForEach(records, (record) =>
            {
                _cache.SetString(record.ResourceName, record.Value);
            });

        }

        protected override string GetString(string name)
        {
            return _cache.GetString(base.GetString(name))?.ToString() ?? name;
        }
    }
}