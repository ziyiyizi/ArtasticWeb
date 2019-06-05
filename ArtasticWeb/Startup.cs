using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.Models;

namespace ArtasticWeb
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

            services.AddCors(options =>
                options.AddPolicy("allallow", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
            );

            var connection = Configuration.GetConnectionString("ArtasticConnection");
            services.AddDbContext<ArtasticContext>(options => options.UseMySql(connection));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("allallow");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "getPosts",
                    template: "getPosts/{controller=Artworks}/{action=GetPosts}"
                );
                routes.MapRoute(
                    name: "getlikelistandcomments",
                    template: "getlikelistandcomments/{controller=Artworks}/{action=GetLikeAndComment}"
                );
                routes.MapRoute(
                    name: "like",
                    template: "like/{controller=Artworks}/{action=Like}"
                );
                routes.MapRoute(
                    name: "comment",
                    template: "comment/{controller=Artworks}/{action=Comment}"
                );
                routes.MapRoute(
                    name: "getpost",
                    template: "getpost/{controller=Artworks}/{action=GetPost}"
                );
                routes.MapRoute(
                    name: "getsearch",
                    template: "getsearch/{controller=Artworks}/{action=GetSearch}"
                );
                routes.MapRoute(
                    name: "followmember",
                    template: "follow/{controller=User}/{action=Follow}"
                );
                routes.MapRoute(
                    name: "test",
                    template: "upload/test/{controller=Artworks}/{action=Upload}"
                );
                routes.MapRoute(
                    name: "getmemberdetail",
                    template: "getmemberdetail/{controller=User}/{action=GetMemberDetails}"
                );
            });

        }
    }
}
