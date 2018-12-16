using Raveshmand.Localization.Core;

namespace Raveshmand.Localization.Xml
{
    internal static class XmlConfiguration
    {
        internal static string FileName() => $"{DefaultConfiguration.AppDomain()}{DefaultConfiguration.LocalizationPathTemplate}.json";
    }
}
