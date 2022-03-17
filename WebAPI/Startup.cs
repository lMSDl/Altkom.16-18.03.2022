using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;
using Models.Validation;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Filters;

namespace WebAPI
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
            services.AddControllers()
                .AddNewtonsoftJson(x => {
                    x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    x.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                    x.SerializerSettings.DateFormatString = "yy.MM+dd";
                    //x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                    //x.SerializerSettings.MaxDepth = 5;
                    x.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                })
                .AddXmlSerializerFormatters()
                //Rejestracja wyszstkich walidatorów z assembly zawieraj¹cego wskazany reprezentatywny walidator
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<ProductValidator>());
                //.AddFluentValidation();

            //services.AddTransient<IValidator<Product>, ProductValidator>();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            //});

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);

            services
                .AddSingleton<EntityFaker<User>, UserFaker>()
                .AddSingleton<EntityFaker<Product>, ProductFaker>()
                .AddSingleton<IUsersService, UsersService>()
                .AddSingleton<ICrudService<Product>>(serviceProvider => new CrudService<Product>(serviceProvider.GetService<EntityFaker<Product>>()));

            services.AddScoped<ConsoleLogFilter>()
                .AddSingleton(x => new LimitAsyncFilter(2));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseHsts();

            app.UseRouting();

            app.UseResponseCompression();

            //app.Use(async (context, next) =>
            //{

            //    await next();
            //    Console.WriteLine($"{DateTime.Now}: {context.Request.Host}{context.Request.Path} - StatusCode {context.Response.StatusCode}");
            //});

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
