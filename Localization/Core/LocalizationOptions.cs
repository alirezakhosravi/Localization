namespace Localization.Core
{
    public enum CacheOption
    {
        IMemoryCache,
        IDistributedCache
    }

    public class LocalizationOptions
    {
        /// <summary>
        /// Gets or sets the cache dependency.
        /// </summary>
        /// <value>The cache dependency.</value>
        public CacheOption CacheDependency { get; set; } = CacheOption.IMemoryCache;
    }
}