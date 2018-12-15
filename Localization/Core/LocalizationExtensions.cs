using System.Globalization;

namespace Localization.Core
{
    public static class LocalizationExtensions
    {
        public static bool IsValidCultureName(this string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return false;
            }

            try
            {
                CultureInfo.GetCultureInfo(cultureName);
                return true;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }
    }
}