using Raveshmand.Localization.Core;

namespace Raveshmand.Localization.Json
{
    internal static class JsonConfiguration
    {
        internal static string FileName() => $"{DefaultConfiguration.AppDomain()}{DefaultConfiguration.LocalizationPathTemplate}.json";
    }
}
