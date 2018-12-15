using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Localization.Core
{
    public class LocalizationRecord
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Localization key, Unique Name of the string to be localized, like Administration.User.Fields.UserName
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Localized value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Name of the culture, like "fa" or "fa-IR"
        /// </summary>
        public string CultureName { get; set; }

        /// <summary>
        /// Localization resource name, like SharedResource,LabelResource,MessageResource,...
        /// </summary>
        public string ResourceName { get; set; }
    }
}