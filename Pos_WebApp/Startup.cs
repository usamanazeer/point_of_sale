using System;
using System.Collections.Generic;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Pos_WebApp.Attributes;
using Pos_WebApp.Services;
using Pos_WebApp.Utilities.ClientManagers;
using Utilities.LicenseValidation;
using Utilities.SystemUtil;

namespace Pos_WebApp
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
            services.Scan(x => x.FromAssemblyOf<IService>().AddClasses(@classes => @classes.AssignableTo<IService>()).AsImplementedInterfaces().WithTransientLifetime());
            
            services.AddTransient<IClientManager, ClientManager>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //services.AddControllers(config =>
            //{
            //    config.Filters.Add<UserAuthentication>();
            //});
            //services.AddMvc();
            services.AddMvc(setupAction: options =>
            {
                options.Filters.Add(item: new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add<UserAuthentication>();
                options.EnableEndpointRouting = false;
            });
            
            //license validator service.
            services.AddSingleton<ILicenseValidator, LicenseValidator>();

            //SystemUtilityService
            services.AddSingleton<ISystemUtility, SystemUtility>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddUtilityServices();

            //services.AddServices();

            services.AddSession(configure: so =>
            {
                so.IdleTimeout = TimeSpan.FromDays(1);
            });




            //Response Compression Start
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
            services.AddResponseCompression(options =>
            {
                IEnumerable<string> mimeTypes = new[]
                 {
                     // General
                     "text/plain",
                     "text/html",
                     "text/css",
                     "font/woff2",
                     "application/javascript",
                     "image/x-icon",
                     "image/png",
                     "application/json"
                 };

                options.EnableForHttps = true;
                options.MimeTypes = mimeTypes;
                //options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });
            //Response Compression End


            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //File.WriteAllText("DeploymentInfo.txt","Enivironment: Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //File.WriteAllText("DeploymentInfo.txt", "Enivironment: Production");
                app.UseExceptionHandler(errorHandlingPath: "/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            
            app.UseHttpsRedirection();
            app.UseResponseCaching();
            app.UseResponseCompression();
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    OnPrepareResponse = ctx =>
                    {
                        string durationInSeconds = Configuration.GetSection("AppSettings:CacheMaxAge").Value;
                        ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                            "public,max-age=" + durationInSeconds;
                    }
                }
                );

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(configure: endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseMvc(configureRoutes: routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=User}/{action=Login}/{id?}"
                );
            });
        }
    }
}