using System.Collections.Generic;
using System.Globalization;
using Localization.Core;
using Microsoft.Extensions.Localization;

namespace Localization.Coreson
{
    public class BaseStringLocalization : IStringLocalizer
    {
        private readonly CultureInfo _culture;
        private readonly string _resourceName;

        public BaseStringLocalization(string resourceName)
        {
            _culture = CultureInfo.CurrentUICulture;
            _resourceName = resourceName;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new System.NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this;
        }

        public LocalizedString this[string name] => GetValue(name);

        public LocalizedString this[string name, params object[] arguments] => GetValue(name, arguments);

        protected LocalizedString GetValue(string name, params object[] arguments)
        {
            var value = GetString(name);

            return new LocalizedString(name, string.Format(value, arguments));
        }

        protected virtual string GetString(string name)
        {
            var resource = string.IsNullOrEmpty(_resourceName) ? nameof(LocalizationResourceNames.SharedResource) : _resourceName;
            string computedKey = string.Format(DefaultConfiguration.LocalizationCacheKeyTemplate, CultureInfo.CurrentUICulture.Name, resource, name);

            return computedKey;
        }
    }
}

