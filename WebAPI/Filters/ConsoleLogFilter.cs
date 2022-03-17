using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Filters
{
    public class ConsoleLogFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"{DateTime.Now}: {context.HttpContext.Request.Host}{context.HttpContext.Request.Path} - StatusCode {context.HttpContext.Response.StatusCode}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
