using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using POS_API.Data;
using POS_API.Repositories;
using POS_API.Services;
using POS_API.Utilities;
using POS_API.Utilities.SignalR.NotificationHubs;
using POS_API.Utilities.SignalR.SalesHubs;
using Utilities.LicenseValidation;
using Utilities.SystemUtil;

namespace POS_API
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
            //register services
            services.Scan(x => x.FromAssemblyOf<IService>()
            .AddClasses(@classes => @classes.AssignableTo<IService>())
            .AsImplementedInterfaces().WithTransientLifetime());
            
            //register repositories
            services.Scan(x => x.FromAssemblyOf<IRepository>().AddClasses(@classes => @classes.AssignableTo<IRepository>()).AsImplementedInterfaces().WithTransientLifetime());
            // Utilities Config
            services.AddUtilityServices();

            services.AddCors(setupAction: _ =>
            {
                //options.AddDefaultPolicy(configurePolicy: builder =>
                //{
                //    builder.WithOrigins("http://localhost:5015")
                //           .AllowAnyHeader()
                //           .AllowAnyMethod()
                //           .AllowCredentials();
                //});
            });
            services.AddControllers();
            services.AddMvc(setupAction: option => option.EnableEndpointRouting = false)
            .AddJsonOptions(configure: options => {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(root: Path.Combine(path1: Directory.GetCurrentDirectory(), path2: "wwwroot")));
            //Fetching Connection string from APPSETTINGS.JSON  
            var connectionString = Configuration.GetConnectionString(name: "Default");
            //Entity Framework
            services.AddDbContext<PosDB_Context>(optionsAction: options => options.UseSqlServer(connectionString: connectionString, opt => opt.EnableRetryOnFailure()), contextLifetime: ServiceLifetime.Transient);
              //services.AddDbContext<PosDB_Context>(optionsAction: options => options.UseSqlServer(connectionString: connectionString));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(setupAction: c =>
            {
                c.SwaggerDoc(name: "v1", info: new OpenApiInfo { Title = "My API", Version = "v1" });
                //c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "JWT Authorization header using the Bearer scheme."
                //});
                //c.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
                //{
                //    {
                //          new OpenApiSecurityScheme
                //           {
                //                Reference = new OpenApiReference
                //                {
                //                    Type = ReferenceType.SecurityScheme,
                //                    Id = "Bearer"
                //                }
                //           },
                //            new string[] {}
                //    }
                //});
                c.ResolveConflictingActions(x => x.First());
            });
            //services.AddRepositoryServices();

            //Add Services
            //services.AddServices();
            //Add AutoMapper

            


            //Authentication
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(configureOptions: options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.ASCII.GetBytes(s: Configuration.GetSection(key: "AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddDistributedMemoryCache();
            services.AddSession(configure: so =>
            {
                so.Cookie.Name = "user_session";
                so.IdleTimeout = TimeSpan.FromDays(value: 1);
                so.Cookie.HttpOnly = true;
                so.Cookie.IsEssential = true;
            });
            services.AddSignalR(configure: e =>
            {
                e.EnableDetailedErrors = true;
                //e.MaximumReceiveMessageSize = 102400000;
            });
            //.AddJsonProtocol(options => {
            //options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            //});
            services.AddControllersWithViews()
            .AddNewtonsoftJson(setupAction: options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //SystemUtilityService
            services.AddSingleton<ISystemUtility, SystemUtility>();
            //license validator service.
            services.AddSingleton<ILicenseValidator, LicenseValidator>();
            //InMemoryCacheService
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(pathFormat: "API-ERRORS.txt");
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(setupAction: c => {c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "My API V1");});
            app.UseStatusCodePages();
            app.UseAuthentication();
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(x => x.AllowAnyMethod()
                             .AllowAnyHeader()
                             .SetIsOriginAllowed(_ => true) //allow any origin
                             .AllowCredentials());
            //app.UseCors(x => x.WithOrigins("https://localhost:5000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseAuthentication();
            app.UseSession();
            app.UseEndpoints(configure: endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>(pattern: "/notificationHub"/*, options => { options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling; }*/);
                endpoints.MapHub<SalesHub>(pattern: "/salesHub");
            });
            app.UseMvc(configureRoutes: routes => {
                routes.MapRoute( name: "Areas", template: "{area:exists}/{controller=User}/{action=Register}/{id?}");
                routes.MapRoute( name: "default", template: "{controller=WeatherForecast}/{action=Get}/{id?}");
            });
        }
    }
}