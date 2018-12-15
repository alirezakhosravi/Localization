using System;
using System.Globalization;
using Localization.Core;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Localization;

namespace Localization.MvcTest.Infrastructure
{
    public abstract class WebViewPageBase : WebViewPageBase<dynamic>
    {
    }

    public abstract class WebViewPageBase<TModel> : RazorPage<TModel>
    {
        [RazorInject] public IStringLocalizerFactory LocalizerFactory { get; set; }

        private IStringLocalizer HtmlLocalizer =>
            LocalizerFactory.Create(LocalizationResourceName, LocalizationResourceLocation);

        /// <summary>
        /// The name of the resource to load strings from
        /// It must be set in order to use <see cref="L(string)"/>.
        /// </summary>
        protected string LocalizationResourceName { get; set; } = nameof(LocalizationResourceNames.SharedResource);

        /// <summary>
        /// The location to load resources from
        /// It must be set in order to use <see cref="L(string)"/>.
        /// </summary>
        protected string LocalizationResourceLocation { get; set; }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        protected string L(string name)
        {
            return HtmlLocalizer.GetString(name);
        }
    }
}
