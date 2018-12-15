namespace Localization.Core
{
    public enum LocalizationSourceType
    {
        ResourceFile,
        JsonFile,
        JsonEmbeddedFile,
        XmlFile,
        XmlEmbeddedFile,
        Database,
        DictionaryBased
    }

    public class LocalizationConfiguration
    {
        public LocalizationSourceType SourceType { get; set; } = LocalizationSourceType.ResourceFile;
    }
}