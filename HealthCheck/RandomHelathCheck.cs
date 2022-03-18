using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck
{
    public class RandomHelathCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var second = DateTime.Now.Second;

            if (second % 2 == 0)
            {
                return Task.FromResult(HealthCheckResult.Healthy("I am OK!", new Dictionary<string, object>{{ "numberOfClients", new Random().Next() }}));
            }
            if (second % 3 == 0)
                return Task.FromResult(HealthCheckResult.Degraded("HELP ME!"));

            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}
