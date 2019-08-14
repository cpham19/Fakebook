using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fakebook.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;

namespace Fakebook
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policyBuilder => policyBuilder.RequireClaim("IsAdmin"));
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<TimelineService>();
            services.AddScoped<UserService>();
            services.AddScoped<IForumService, ForumService>();
            services.AddScoped<AccountService>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                 name: "login",
                 defaults: new { controller = "Account", action = "Login" },
                 template: "Login/");

                routes.MapRoute(
                    name: "register",
                    defaults: new { controller = "Account", action = "Register" },
                    template: "Register/");

                routes.MapRoute(
                    name: "search",
                    defaults: new { controller = "Search", action = "Index" },
                    template: "Search/{**name}");

                routes.MapRoute(
                  name: "search2",
                  defaults: new { controller = "Search", action = "Search" },
                  template: "Search/{**name}");

                routes.MapRoute(
                    name: "user",
                    defaults: new { controller = "User", action = "ViewUser" },
                    template: "User/{**name}");

                routes.MapRoute(
                    name: "edit",
                    defaults: new { controller = "Home", action = "Edit" },
                    template: "Edit/");

                //// Main Page where you see all forums
                //routes.MapRoute(
                //    name: "forum",
                //    template: "{controller=Forum}/{action=Index}/{id?}");

                //// View selected forum and its topics
                //routes.MapRoute(
                //    name: "ViewForum",
                //    defaults: new { controller = "Forum", action = "ViewForum" },
                //    template: "Forum/{id?}");

                //// View selected topic and its replies
                //routes.MapRoute(
                //    name: "ViewTopic",
                //    defaults: new { controller = "Forum", action = "ViewTopic", },
                //    template: "Forum/{id?}/{id2?}");

                //// Adding a new forum
                //routes.MapRoute(
                //    name: "AddForum",
                //    defaults: new { controller = "Forum", action = "AddForum" },
                //    template: "Forum/Add");

                //// Adding a new topic for selected forum
                //routes.MapRoute(
                //    name: "AddTopic",
                //    defaults: new { controller = "Forum", action = "AddTopic" },
                //    template: "Forum/{id?}/AddTopic");

                //// Adding a new reply for selected topic
                //routes.MapRoute(
                //    name: "AddReply",
                //    defaults: new { controller = "Forum", action = "AddReply" },
                //    template: "Forum/{id?}/{id2?}/AddReply");
            });

            // This suppose to add different environment files for Azure deployment but this doesnt work.
            //var builder = new ConfigurationBuilder()
            //.SetBasePath(env.ContentRootPath)
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //.AddEnvironmentVariables();
            //builder.Build();
        }
    }
}
