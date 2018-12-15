using System;
using Microsoft.Extensions.Localization;

namespace Localization.Core
{
    /// <summary>
    /// Represents a string that can be localized.
    /// </summary>
    [Serializable]
    public class LocalizableString : ILocalizableString
    {
        /// <summary>
        /// The localization to load navigation resources from.
        /// Can be Null for Database localization source <see cref="LocalizationSourceType"/>.
        /// </summary>
        /// <returns></returns>
        public virtual string ResourceLocation { get; set;}

        /// <summary>
        /// Unique name of the localization resource like, SharedResource,...
        /// </summary>
        public virtual string ResourceName { get; }

        /// <summary>
        /// Unique Name of the string to be localized.
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Needed for serialization.
        /// </summary>
        private LocalizableString()
        {
        }

        public LocalizableString(string name, string resourceName)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if(string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }
            
            Name = name;
            ResourceName = resourceName;
        }

        public string Localize(IStringLocalizerFactory factory)
        {
            return factory.Create(ResourceName, ResourceLocation).GetString(Name);
        }

        public override string ToString()
        {
            return $"[LocalizableString: {Name}, {ResourceName}]";
        }
    }
}