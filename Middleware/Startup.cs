using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Use2Middleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseRouting();

            app.UseMiddleware<Use1Middleware>();
            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("Begin Use1");
            //    await next();
            //    Console.WriteLine("End Use1");
            //});

            app.Map("/Hello", mapApp =>
            {
                mapApp.Use(async (context, next) =>
                {
                    Console.WriteLine("Begin HelloUse");
                    await next();
                    Console.WriteLine("End HelloUse");
                });

                mapApp.Run(async (context) =>
                {
                    Console.WriteLine("Begin HelloRun");
                    await context.Response.WriteAsync("Hello from Run!");
                    Console.WriteLine("End HelloRun");
                });
            });

            app.MapWhen(context => context.Request.Query.TryGetValue("name", out var result), mapApp =>
            {
                mapApp.Run(async (context) =>
                {
                    context.Request.Query.TryGetValue("name", out var result);
                    await context.Response.WriteAsync($"Hello {result}!");
                });
            });

            app.Use2Middleware();
            //app.UseMiddleware<Use2Middleware>();
            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("Begin Use2");
            //    await next();
            //    Console.WriteLine("End Use2");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("Bye", context => context.Response.WriteAsync("Bye from Endpoint")); ;
            });

            app.Run(async (context) =>
            {
                Console.WriteLine("Begin Run");
                await context.Response.WriteAsync("Bye from Run!");
                Console.WriteLine("End Run");
            });
        }
    }
}
