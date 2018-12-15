using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Localization.Core;
using Localization.Json.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;

namespace Localization.Json
{
    public class LocalizationCroud : ILocalizerCroud
    {
        private readonly ILogger _logger;
        private readonly CultureInfo _culture;
        private readonly IServiceProvider _resolver;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly LocalizationOptions _options;

        public LocalizationCroud(IServiceProvider resolver,
            ILogger logger,
            IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            IOptions<LocalizationOptions> options)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _culture = CultureInfo.CurrentUICulture;
            _resolver = resolver ?? throw new ArgumentException(nameof(resolver));
            _options = options.Value;

            if (_options.CacheDependency == CacheOption.IMemoryCache)
            {
                _memoryCache = memoryCache ?? throw new ArgumentException(nameof(memoryCache));
            }
            else
            {
                _distributedCache = distributedCache ?? throw new ArgumentException(nameof(distributedCache));
            }
        }

        public void Delete(string name, string cultureName, string resourceName)
        {
            var resource = string.IsNullOrEmpty(resourceName) ? nameof(LocalizationResourceNames.SharedResource) : resourceName;

            string computedKey = string.Format(DefaultConfiguration.LocalizationCacheKeyTemplate, CultureInfo.CurrentUICulture.Name, resource, name);

            string computedPath = string.Format(JsonConfiguration.FileName(), CultureInfo.CurrentUICulture.Name, resource);

            List<LocalizationRecord> records = JsonReader.Read<List<LocalizationRecord>>(computedPath);

            LocalizationRecord entity = records.FirstOrDefault(a => a.Name == name && a.CultureName == cultureName
                        && a.ResourceName == computedKey);

            if (entity != null)
            {
                records.Remove(records.FirstOrDefault(a => a.Name == name && a.CultureName == cultureName
                        && a.ResourceName == computedKey));

                JsonReader.Write(records, computedPath);

                if (_options.CacheDependency == CacheOption.IMemoryCache)
                {
                    _memoryCache.Remove(computedKey);
                }
                else
                {
                    _distributedCache.Remove(computedKey);
                }

            }
        }

        public void Delete(IEnumerable<string> names, string cultureName, string resourceName)
        {
            var resource = string.IsNullOrEmpty(resourceName) ? nameof(LocalizationResourceNames.SharedResource) : resourceName;

            string computedPath = string.Format(JsonConfiguration.FileName(), CultureInfo.CurrentUICulture.Name, resource);

            List<LocalizationRecord> records = JsonReader.Read<List<LocalizationRecord>>(computedPath);

            List<string> isSuccess = new List<string>();
            foreach (string item in names)
            {
                string computedKey = string.Format(DefaultConfiguration.LocalizationCacheKeyTemplate, CultureInfo.CurrentUICulture.Name, resource, item);
                LocalizationRecord entity = records.FirstOrDefault(a => a.Name == item && a.CultureName == cultureName
                       && a.ResourceName == computedKey);

                if (entity != null)
                {
                    records.Remove(records.FirstOrDefault(a => a.Name == item && a.CultureName == cultureName
                            && a.ResourceName == computedKey));

                    isSuccess.Add(computedKey);
                }
            }

            JsonReader.Write(records, computedPath);

            if (_options.CacheDependency == CacheOption.IMemoryCache)
            {
                foreach (string item in isSuccess)
                {
                    _memoryCache.Remove(item);
                }
            }
            else
            {
                foreach (string item in isSuccess)
                {
                    _distributedCache.Remove(item);
                }
            }
        }

        public string ExportJson(string cultureName, string resourceName)
        {
            var resource = string.IsNullOrEmpty(resourceName) ? nameof(LocalizationResourceNames.SharedResource) : resourceName;
            string computedPath = string.Format(JsonConfiguration.FileName(), cultureName, resource);

            return File.ReadAllText(computedPath);
        }

        public string ExportXml(string cultureName, string resourceName)
        {
            var resource = string.IsNullOrEmpty(resourceName) ? nameof(LocalizationResourceNames.SharedResource) : resourceName;
            string computedPath = string.Format(JsonConfiguration.FileName(), cultureName, resource);
            List<LocalizationRecord> records = JsonReader.Read<List<LocalizationRecord>>(computedPath);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LocalizationRecord>));
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };

            xmlSerializer.Serialize(xmlTextWriter, records);

            string output = Encoding.UTF8.GetString(memoryStream.ToArray());
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (output.StartsWith(_byteOrderMarkUtf8, StringComparison.Ordinal))
            {
                output = output.Remove(0, _byteOrderMarkUtf8.Length);
            }

            return output;
        }

        public void Insert(string name, string value, string cultureName, string resourceName)
        {
            var resource = string.IsNullOrEmpty(resourceName) ? nameof(LocalizationResourceNames.SharedResource) : resourceName;

            string computedKey = string.Format(DefaultConfiguration.LocalizationCacheKeyTemplate, CultureInfo.CurrentUICulture.Name, resource, name);

            string computedPath = string.Format(JsonConfiguration.FileName(), CultureInfo.CurrentUICulture.Name, resource);

            List<LocalizationRecord> records = JsonReader.Read<List<LocalizationRecord>>(computedPath);

            LocalizationRecord entity = records.FirstOrDefault(a => a.Name == name && a.CultureName == cultureName
                        && a.ResourceName == computedKey);

            if (entity == null)
            {
                entity = new LocalizationRecord
                {
                    Name = name,
                    Value = value,
                    CultureName = cultureName,
                    ResourceName = computedKey,
                };

                records.Add(entity);

                JsonReader.Write(records, computedPath);
                if (_options.CacheDependency == CacheOption.IMemoryCache)
                {
                    _memoryCache.Set(computedKey, value);
                }
                else
                {
                    _distributedCache.SetString(computedKey, value);
                }
            }
            else
            {
                Update(name, value, cultureName, resourceName);
            }
        }

        public void Insert(IEnumerable<KeyValuePair<string, string>> keyValue, string cultureName, string resourceName)
        {
            var resource = string.IsNullOrEmpty(resourceName) ? nameof(LocalizationResourceNames.SharedResource) : resourceName;

            string computedPath = string.Format(JsonConfiguration.FileName(), CultureInfo.CurrentUICulture.Name, resource);

            List<LocalizationRecord> records = JsonReader.Read<List<LocalizationRecord>>(computedPath);

            List<KeyValuePair<string, string>> isSuccess = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> item in keyValue)
            {
                string computedKey = string.Format(DefaultConfiguration.LocalizationCacheKeyTemplate, CultureInfo.CurrentUICulture.Name, resource, item.Key);
                LocalizationRecord entity = records.FirstOrDefault(a => a.Name == item.Key && a.CultureName == cultureName
                       && a.ResourceName == computedKey);

                if (entity == null)
                {
                    entity = new LocalizationRecord
                    {
                        Name = item.Key,
                        Value = item.Value,
                        CultureName = cultureName,
                        ResourceName = computedKey,
                    };

                    records.Add(entity);
                    isSuccess.Add(new KeyValuePair<string, string>(computedKey, item.Value));
                }
            }

            JsonReader.Write(records, computedPath);

            if (_options.CacheDependency == CacheOption.IMemoryCache)
            {
                foreach (KeyValuePair<string, string> item in isSuccess)
                {
                    _memoryCache.Set(item.Key, item.Value);
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> item in isSuccess)
                {
                    _distributedCache.SetString(item.Key, item.Value);
                }
            }
        }

        public void Update(string name, string value, string cultureName, string resourceName)
        {
            var resource = string.IsNullOrEmpty(resourceName) ? nameof(LocalizationResourceNames.SharedResource) : resourceName;

            string computedKey = string.Format(DefaultConfiguration.LocalizationCacheKeyTemplate, CultureInfo.CurrentUICulture.Name, resource, name);

            string computedPath = string.Format(JsonConfiguration.FileName(), CultureInfo.CurrentUICulture.Name, resource);

            List<LocalizationRecord> records = JsonReader.Read<List<LocalizationRecord>>(computedPath);

            LocalizationRecord entity = records.FirstOrDefault(a => a.Name == name && a.CultureName == cultureName
                        && a.ResourceName == computedKey);

            if (entity != null)
            {
                records.FirstOrDefault(a => a.Name == name && a.CultureName == cultureName
                        && a.ResourceName == computedKey).Value = value;

                JsonReader.Write(records, computedPath);
                if (_options.CacheDependency == CacheOption.IMemoryCache)
                {
                    _memoryCache.Set(computedKey, value);
                }
                else
                {
                    _distributedCache.SetString(computedKey, value);
                }
            }
        }

        public void Update(IEnumerable<KeyValuePair<string, string>> keyValue, string cultureName, string resourceName)
        {
            var resource = string.IsNullOrEmpty(resourceName) ? nameof(LocalizationResourceNames.SharedResource) : resourceName;

            string computedPath = string.Format(JsonConfiguration.FileName(), CultureInfo.CurrentUICulture.Name, resource);

            List<LocalizationRecord> records = JsonReader.Read<List<LocalizationRecord>>(computedPath);

            List<KeyValuePair<string, string>> isSuccess = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> item in keyValue)
            {
                string computedKey = string.Format(DefaultConfiguration.LocalizationCacheKeyTemplate, CultureInfo.CurrentUICulture.Name, resource, item.Key);
                LocalizationRecord entity = records.FirstOrDefault(a => a.Name == item.Key && a.CultureName == cultureName
                       && a.ResourceName == computedKey);

                if (entity == null)
                {
                    records.FirstOrDefault(a => a.Name == item.Key && a.CultureName == cultureName
                        && a.ResourceName == computedKey).Value = item.Value;

                    isSuccess.Add(new KeyValuePair<string, string>(computedKey, item.Value));
                }
            }

            JsonReader.Write(records, computedPath);

            if (_options.CacheDependency == CacheOption.IMemoryCache)
            {
                foreach (KeyValuePair<string, string> item in isSuccess)
                {
                    _memoryCache.Set(item.Key, item.Value);
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> item in isSuccess)
                {
                    _distributedCache.SetString(item.Key, item.Value);
                }
            }
        }
    }
}
