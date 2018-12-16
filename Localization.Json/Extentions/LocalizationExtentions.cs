using System;
using Raveshmand.Localization.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Raveshmand.Localization.Json.Extentions
{
    public static class LocalizationExtentions
    {
        public static void AddJsonLocalization(this IServiceCollection services,
            Action<Core.LocalizationOptions> options)
        {
            services.Configure(options);
            services.Add(ServiceDescriptor.Singleton<IStringLocalizerFactory, JsonStringLocalizerFactory>());
            services.Add(ServiceDescriptor.Singleton<ILocalizerCrud, JsonLocalizationCrud>());
        }
    }
}
