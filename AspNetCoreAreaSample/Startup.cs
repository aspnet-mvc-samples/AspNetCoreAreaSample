using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreAreaSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // ドメインによるエリア決定
            app.MapWhen(
                (HttpContext context) =>
                {
                    return context.Request.Host.Value.StartsWith("area1.");
                },
                (IApplicationBuilder builder) =>
                {
                    builder.UseMvc(routes =>
                    {
                        routes.MapRoute(
                            name: "area1route",
                            template: "{controller=Home}/{action=Index}/{id?}",
                            defaults: new { area = "area1" }
                        );
                    });
                }
            );
            app.MapWhen(
                (HttpContext context) =>
                {
                    return context.Request.Host.Value.StartsWith("area2.");
                },
                (IApplicationBuilder builder) =>
                {
                    builder.UseMvc(routes =>
                    {
                        routes.MapRoute(
                            name: "area2route",
                            template: "{controller=Home}/{action=Index}/{id?}",
                            defaults: new { area = "area2" }
                        );
                    });
                }
            );

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "area-route",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
