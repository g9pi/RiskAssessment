using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RiskAssessment.Repositorys;
using RiskAssessment.Repositorys.IRepositorys;
using RiskAssessment.Service;
using RiskAssessment.Service.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RiskAssessment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("MILDB");
            OledbConnectionString = Configuration.GetConnectionString("NETSQL");
            EpicorConnectionString = Configuration.GetConnectionString("EPICOR");
        }

        public IConfiguration Configuration { get; }
        public static string ConnectionString { get; private set; }
        public static string OledbConnectionString { get; private set; }
        public static string EpicorConnectionString { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();

            services.AddTransient<IExternalService, ExternalService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IStorageService, StorageService>();
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(config =>
                {
                    config.Cookie.Name = "RAAHelloCookie";
                    config.LoginPath = "/Login";
                    config.Cookie.IsEssential = true;
                    config.Cookie.HttpOnly = true;
                    config.SlidingExpiration = true;
                    config.ExpireTimeSpan = TimeSpan.FromDays(30);
                    config.Cookie.SameSite = SameSiteMode.Strict;
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "Logs/log"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
          

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
