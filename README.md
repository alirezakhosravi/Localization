# ASP.net Core Localization
Creating a multilingual website with ASP.NET Core will allow your site to reach a wider audience. ASP.NET Core provides services and middleware for localizing into different languages and cultures.

Feel free to ask any question: alirezakhosravi [at] live.com

### Configuration Xml Localizarion
On ``Startup.cs`` you must add ``services.AddXmlLocalization(option=> { });``

```
services.AddXmlLocalization(option => { });
```

### Configuration Json Localizarion
On ``Startup.cs`` you must add ``services.AddJsonLocalization(option=> { });``

```
services.AddJsonLocalization(option => { });
```

### Configuration EntityFramework Localizarion
On ``Startup.cs`` you must add ``services.AddDbLocalization<Context>(option=> { });``

```
var connection = @"ConnectionString";
            services.AddDbContext<Context>
                (options => options.UseSqlServer(connection));
services.AddDbLocalization<Context>(option => { });
```

and on ``Context.cs`` in ``OnModelCreating`` function add ``modelBuilder.ApplyLocalizationRecordConfiguration();``
```
using Raveshmand.Localization.EntityFramework.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Localization.MvcTest.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyLocalizationRecordConfiguration();
        }
    }
}
```

### Set Caching
Deault caching in this solution used ``IMemoryCache``, for change this interface to ``IDistributedCache`` you can changed this interface on option.
```
services.AddDbLocalization<Context>(option => { option.CacheDependency = CacheOption.IDistributedCache; });
```

### Use localization on controller
```
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
```

```
    public class HomeController : BaseController
    {
        public HomeController(IStringLocalizerFactory localizerFactory): base(localizerFactory)
        {
        }

        public IActionResult Index()
        {
            string value = L("Test01");
            ...
        }
        ...
    }
```

### Crud action on localization in application
```
    public class HomeController : BaseController
    {
        private readonly ILocalizerCrud _localizerCrud;
        public HomeController(ILocalizerCrud localizerCrud, IStringLocalizerFactory localizerFactory): base(localizerFactory)
        {
            _localizerCrud = localizerCrud;
        }

        public IActionResult Index()
        {
            ...
            _localizerCrud.Insert("Test01", "Test Value", CultureInfo.CurrentCulture.Name, LocalizationResourceName);
            ...
        }

    }
```

### Use localization on view
```
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
```
```
@inherits Localization.MvcTest.Infrastructure.WebViewPageBase

@{
    ViewData["Title"] = L("HomePage");
}

...

<div>
    @L("Test01")
</div>
```
