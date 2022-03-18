using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WindowsServices.Core
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string _fileName = "c:\\CustomService\\CoreService.txt";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
            File.AppendAllText(_fileName, $"{DateTime.Now.ToShortTimeString()}: {message}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
                WriteMessage("I am wokring...");
            }
        }
    }
}
