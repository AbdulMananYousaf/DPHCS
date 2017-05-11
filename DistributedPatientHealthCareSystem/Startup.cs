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
using DistributedPatientHealthCareSystem.Data;
using DistributedPatientHealthCareSystem.Models;
using DistributedPatientHealthCareSystem.Services;
using DistributedPatientHealthCareSystem.DPHCSModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using DistributedPatientHealthCareSystem.Hubs;
using DistributedPatientHealthCareSystem.Helper;

namespace DistributedPatientHealthCareSystem
{
    public class Startup
    {
        public static IConnectionManager ConnectionManager;
       
       
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


            // Add framework services.
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<DPHCSContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DPHCSConnection")));


           
          
            services.AddIdentity<ApplicationUser, IdentityRole>(
                x =>
                {
                    x.Cookies.ApplicationCookie.LoginPath = new PathString("/UserAccount/Login");
                    x.Cookies.ApplicationCookie.LogoutPath = new PathString("/UserAccount/Logof");
                }
                )
                .AddEntityFrameworkStores<DPHCSContext>()
                .AddDefaultTokenProviders();
          
           
            services.AddMvc();

            //For RealTime App
            services.AddSignalR();

            //Services for Conver View to String
                services.AddTransient<ViewRenderService>();
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            /////for creating session and us app.usesesssion
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.CookieName = "DPHCS";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(15);
                options.CookieHttpOnly = true;
            });


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            RoleManager<IdentityRole> roleManager, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            ConnectionManager = serviceProvider.GetService<IConnectionManager>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSignalR();

            app.UseIdentity();

            app.UseSession();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            var _context = app.ApplicationServices.GetService<DPHCSContext>();

            //Drop all old session in database
            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE [UserConnection]");
            _context.SaveChanges();


            await RoleInitializer.Initialize(roleManager);

           
        }
    }
}
