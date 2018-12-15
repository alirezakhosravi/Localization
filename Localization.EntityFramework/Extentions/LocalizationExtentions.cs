using System;
using Localization.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Localization.EntityFramework.Extentions
{
    public static class LocalizationExtentions
    {
        public static void AddDbLocalization<TContext>(this IServiceCollection services,
            Action<Core.LocalizationOptions> options)
            where TContext : DbContext
        {
            services.Configure(options);

            services.Add(ServiceDescriptor.Singleton<IStringLocalizerFactory, EFStringLocalizerFactory<TContext>>());
            services.Add(ServiceDescriptor.Singleton<ILocalizerCrud, EFLocalizationCrud<TContext>>());
        }

        public static void ApplyLocalizationRecordConfiguration(this ModelBuilder builder)
        {
            if(builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ApplyConfiguration(new LocalizationRecordConfiguration());
        }
    }
}
