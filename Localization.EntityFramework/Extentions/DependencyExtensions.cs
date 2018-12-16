using System;
using Microsoft.Extensions.DependencyInjection;

namespace Raveshmand.Localization.EntityFramework.Extentions
{
    public static class DependencyExtensions
    {
        /// <summary>
        /// Creates an IServiceScope which contains an IServiceProvider used to resolve dependencies from a newly created scope and then runs an associated callback.
        /// </summary>
        public static void RunScopedService<T, TS>(this IServiceProvider serviceProvider, Action<TS, T> callback)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TS>();

                callback(context, serviceScope.ServiceProvider.GetRequiredService<T>());
                if (context is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates an IServiceScope which contains an IServiceProvider used to resolve dependencies from a newly created scope and then runs an associated callback.
        /// </summary>
        public static void RunScopedService<TS>(this IServiceProvider serviceProvider, Action<TS> callback)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TS>();
                callback(context);
                if (context is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates an IServiceScope which contains an IServiceProvider used to resolve dependencies from a newly created scope and then runs an associated callback.
        /// </summary>
        public static T RunScopedService<T, TS>(this IServiceProvider serviceProvider, Func<TS, T> callback)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TS>();
                return callback(context);
            }
        }

    }
}
