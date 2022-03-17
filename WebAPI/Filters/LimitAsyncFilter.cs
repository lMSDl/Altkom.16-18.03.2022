using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Filters
{
    public class LimitAsyncFilter : IAsyncActionFilter
    {
        private ConcurrentDictionary<HostString, Counter> _counter = new ConcurrentDictionary<HostString, Counter>();

        private readonly int _limit;
        //private int _counter;

        public LimitAsyncFilter(int limit)
        {
            _limit = limit;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //if (_counter >= _limit )
            if (_counter.TryGetValue(context.HttpContext.Request.Host, out var value))
            {
                if (value.Value >= _limit)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    return;
                }
            }
            else
            {
                value = new Counter();
                _counter.TryAdd(context.HttpContext.Request.Host, value);
            }
            
            Interlocked.Increment(ref value.Value);
                //Interlocked.Increment(ref _counter);
                try
                {
                    await next();
                    await Task.Delay(5000);
                }
                finally
                {
                    Interlocked.Decrement(ref value.Value);
                    //Interlocked.Decrement(ref _counter);
                }


        }

        private class Counter
        {
            public int Value;
        }
    }
}
