using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware
{
    public class Use2Middleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Begin Use2");
            await next(context);
            Console.WriteLine("End Use2");
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder Use2Middleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Use2Middleware>();
        }
    }
}
