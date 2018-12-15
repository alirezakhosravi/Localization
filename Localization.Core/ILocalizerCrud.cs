using System.Collections.Generic;

namespace Localization.Core
{
    public interface ILocalizerCrud
    {
        /// <summary>
        /// Insert the specified name, value, cultureName and resourceName.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="cultureName">Culture name.</param>
        /// <param name="resourceName">Resource name.</param>
        void Insert(string name, string value, string cultureName, string resourceName);

        /// <summary>
        /// Update the value by specified name, cultureName and resourceName.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="cultureName">Culture name.</param>
        /// <param name="resourceName">Resource name.</param>
        void Update(string name, string value, string cultureName, string resourceName);

        /// <summary>
        /// Delete by specified name, cultureName and resourceName.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="cultureName">Culture name.</param>
        /// <param name="resourceName">Resource name.</param>
        void Delete(string name, string cultureName, string resourceName);

        /// <summary>
        /// Insert renge of keyValue, cultureName and resourceName.
        /// </summary>
        /// <param name="keyValue">Key value.</param>
        /// <param name="cultureName">Culture name.</param>
        /// <param name="resourceName">Resource name.</param>
        void Insert(IEnumerable<KeyValuePair<string, string>> keyValue, string cultureName, string resourceName);

        /// <summary>
        /// Update renge of specified keyValue, cultureName and resourceName.
        /// </summary>
        /// <param name="keyValue">Key value.</param>
        /// <param name="cultureName">Culture name.</param>
        /// <param name="resourceName">Resource name.</param>
        void Update(IEnumerable<KeyValuePair<string, string>> keyValue, string cultureName, string resourceName);

        /// <summary>
        /// Delete renge of specified names, cultureName and resourceName.
        /// </summary>
        /// <param name="names">Names.</param>
        /// <param name="cultureName">Culture name.</param>
        /// <param name="resourceName">Resource name.</param>
        void Delete(IEnumerable<string> names, string cultureName, string resourceName);

        /// <summary>
        /// Exports the xml.
        /// </summary>
        /// <returns>The xml.</returns>
        /// <param name="cultureName">Culture name.</param>
        /// <param name="resourceName">Resource name.</param>
        string ExportXml(string cultureName, string resourceName);

        /// <summary>
        /// Exports the json.
        /// </summary>
        /// <returns>The json.</returns>
        /// <param name="cultureName">Culture name.</param>
        /// <param name="resourceName">Resource name.</param>
        string ExportJson(string cultureName, string resourceName);
    }
}
