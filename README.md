# ASP.net Core Development Localization

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
