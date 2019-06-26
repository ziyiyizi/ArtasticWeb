using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Serv;
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

            //services.AddOptions();
            //services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            //services.AddSingleton<EmailService>();

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

            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (IServiceScope scope = app.ApplicationServices.CreateScope())
                    {
                        //do something 
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await Echo(context, webSocket);
                    }
                }
                else
                {
                    //Hand over to the next middleware
                    await next();
                }
            });

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
                    name: "likerequest",
                    template: "likerequest/{controller=Artworks}/{action=Like}"
                );
                routes.MapRoute(
                    name: "makecomment",
                    template: "makecomment/{controller=Artworks}/{action=Comment}"
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
                    template: "followmember/{controller=User}/{action=Follow}"
                );
                routes.MapRoute(
                    name: "test",
                    template: "upload/test/{controller=Artworks}/{action=Upload}"
                );
                routes.MapRoute(
                    name: "getmemberdetail",
                    template: "getmemberdetail/{controller=User}/{action=GetMemberDetails}"
                );
                routes.MapRoute(
                    name: "login",
                    template: "user/login/{controller=User}/{action=Login}"
                );
                routes.MapRoute(
                    name: "getrecommendtags",
                    template: "getrecommendtags/{controller=Artworks}/{action=GetRecommendTags}"
                );
                routes.MapRoute(
                    name: "getprofile",
                    template: "getprofile/{controller=User}/{action=GetUser}"
                );
                routes.MapRoute(
                    name: "uploadprofile",
                    template: "uploadprofile/{controller=User}/{action=UploadProfile}"
                );
                routes.MapRoute(
                    name: "getweekly",
                    template: "getweekly/{controller=Artworks}/{action=GetWeekly}"
                );
                routes.MapRoute(
                    name: "community",
                    template: "community/{controller=Home}/{action=Index}"
                );
                routes.MapRoute(
                    name: "signup",
                    template: "signup/{controller=User}/{action=Register}"
                );
                routes.MapRoute(
                    name: "post",
                    template: "post/{controller=Artworks}/{action=GetPost}"
                );
            });

        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

    }
}
