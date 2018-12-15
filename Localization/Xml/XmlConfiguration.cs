using Localization.Core;

namespace Localization.Xml
{
    internal static class XmlConfiguration
    {
        internal static string FileName() => $"{DefaultConfiguration.AppDomain()}{DefaultConfiguration.LocalizationPathTemplate}.json";
    }
}
