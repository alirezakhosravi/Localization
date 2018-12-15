using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Localization.EntityFramework;
using Localization.EntityFramework.Extentions;
using LocalizationTest.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Localization.Xml.Extentions;
using Localization.Json.Extentions;

namespace LocalizationTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            //var connection = @"Server=localhost;Database=EFGetStarted;user id=sa;password=123qweRt;ConnectRetryCount=0";
            //services.AddDbContext<Context>(options => 
            //{ 
            //    options.UseSqlServer(connection); 
            //});

            services.AddSingleton(typeof(ILogger), typeof(Logger<object>));

            //services.AddDbLocalization<Context>(options => 
            //{ 
            //});

            //services.AddXmlLocalization(options => { });

            services.AddJsonLocalization(options => { options.CacheDependency = Localization.Core.CacheOption.IMemoryCache; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
