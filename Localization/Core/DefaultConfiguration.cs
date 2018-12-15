using System;
namespace Localization.Core
{
    public static class DefaultConfiguration
    {
        internal static string AppDomain() => $"{AppContext.BaseDirectory}/Localization/";
        internal const string LocalizationCacheKeyTemplate = "CULTURE_{0}_RESOURCE_{1}_NAME_{2}_CACHE_KEY";
        internal const string LocalizationPathTemplate = "CULTURE_{0}_RESOURCE_{1}";
    }
}
