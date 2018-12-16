using System;
using Raveshmand.Localization.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Localization.MvcTest.Infrastructure
{
    public class BaseController : Controller
    {
        private readonly IStringLocalizerFactory _localizerFactory;

        public BaseController(IStringLocalizerFactory localizerFactory)
        {
            _localizerFactory = localizerFactory;
        }

        private IStringLocalizer StringLocalizer
        {
            get
            {
                if (_localizerFactory == null)
                    throw new InvalidOperationException(
                        "For use L(...) methods, should Inject IStringLocalizerFactory and pass it to WebApiController's constructor");

                return _localizerFactory.Create(LocalizationResourceName, LocalizationResourceLocation);
            }
        }

        /// <summary>
        /// The name of the resource to load strings from
        /// It must be set in order to use <see cref="L(string)"/>.
        /// </summary>
        protected string LocalizationResourceName { get; set; } = nameof(LocalizationResourceNames.SharedResource);

        /// <summary>
        /// The location to load resources from
        /// It must be set in order to use <see cref="L(string)"/>.
        /// </summary>
        protected string LocalizationResourceLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        protected string L(string name) => StringLocalizer.GetString(name);
    }
}
