using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Localization.Core;
using Localization.Xml.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Localization.Coreson;

namespace Localization.Xml
{
    public class XmlMemoryCacheStringLocalizer : BaseStringLocalization, IStringLocalizer
    {
        private readonly ILogger _logger;
        private readonly CultureInfo _culture;
        private readonly IServiceProvider _resolver;
        private readonly string _resourceName;
        private readonly IMemoryCache _cache;

        public XmlMemoryCacheStringLocalizer(
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
            Connect();
        }

        private void Connect()
        {
            var resource = string.IsNullOrEmpty(_resourceName) ? nameof(LocalizationResourceNames.SharedResource) : _resourceName;
            string computedPath = string.Format(XmlConfiguration.FileName(), CultureInfo.CurrentUICulture.Name, resource);

            List<LocalizationRecord> records = XmlReader.Read<List<LocalizationRecord>>(computedPath);

            Parallel.ForEach(records, (record) =>
            {
                _cache.Set(record.ResourceName, record.Value);
            });
        }

        protected override string GetString(string name)
        {
            return _cache.Get(base.GetString(name))?.ToString() ?? name;

        }
    }
}