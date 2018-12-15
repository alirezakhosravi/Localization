using System.Collections.Generic;

namespace Localization.Core
{
    public interface ILocalizerCroud
    {
        void Insert(string name, string value, string cultureName, string resourceName);

        void Update(string name, string value, string cultureName, string resourceName);

        void Delete(string name, string cultureName, string resourceName);

        void Insert(IEnumerable<KeyValuePair<string, string>> keyValue, string cultureName, string resourceName);

        void Update(IEnumerable<KeyValuePair<string, string>> keyValue, string cultureName, string resourceName);

        void Delete(IEnumerable<string> names, string cultureName, string resourceName);

        string ExportXml(string cultureName, string resourceName);

        string ExportJson(string cultureName, string resourceName);
    }
}
