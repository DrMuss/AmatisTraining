using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AT.Data;
using AT.Data.Identity;
using AT.Services;
using AT.Services.Messaging;
using AT.Services.Stripe;
using AT.Services.Agylia;
using AT.Services.Recapture;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using AT.Helpers;
using Joonasw.AspNetCore.SecurityHeaders;
using Microsoft.AspNetCore.Rewrite;

namespace AT
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configure the Email, Stripe and Agylia config
            services.Configure<EmailConfig>(options => Configuration.GetSection("EmailSettings").Bind(options));
            services.Configure<StripeConfig>(options => Configuration.GetSection("Stripe").Bind(options));
            services.Configure<AgyliaConfig>(options => Configuration.GetSection("Agylia").Bind(options));
            services.Configure<RecaptchaConfig>(options => Configuration.GetSection("GoogleReCaptcha").Bind(options));




            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole> (options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.Configure<RouteOptions>(options => {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });

            services.AddMvc();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml",
                    "image/jpg",
                    "image/png",
                    "image/gif",
                    "image/jpeg"
        };
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            // Add Database Initializer
            services.AddScoped<IDbInitializer, DbInitializer>();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer, ILoggerFactory loggerFactory)
        {

            app.UseResponseCompression();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseHttpsEnforcement();

                app.UseHsts(new HstsOptions
                {
                    Duration = new TimeSpan(30, 0, 0, 0, 0),
                    IncludeSubDomains = false,
                    Preload = false
                });

                //only comment out in development.  Make sure it is uncommented on live
                app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

                //error pages
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
            }


            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24 * 300;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;

                    if (!StringValues.IsNullOrEmpty(ctx.Context.Request.Headers[HeaderNames.AcceptEncoding]))
                        ctx.Context.Response.Headers.Append(HeaderNames.Vary, HeaderNames.AcceptEncoding);
                }
            });
        

            app.UseIdentity();
            dbInitializer.Initialize();
            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
