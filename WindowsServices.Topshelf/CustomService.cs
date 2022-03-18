using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace WindowsServices.Topshelf
{
    public class CustomService : ServiceControl
    {
        private string _fileName = "c:\\CustomService\\TopshelfService.txt";

        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
            File.AppendAllText(_fileName, $"{DateTime.Now.ToShortTimeString()}: {message}");
        }

        private CancellationTokenSource CancellationTokenSource { get; set; }

        public bool Start(HostControl hostControl)
        {
            WriteMessage("Starting...");

            CancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                while (!CancellationTokenSource.Token.IsCancellationRequested)
                {
                    WriteMessage("I am wokring...");
                    await Task.Delay(5000);
                }
            }, CancellationTokenSource.Token);

            return DateTime.Now.Second % 2 == 0;
        }

        public bool Stop(HostControl hostControl)
        {
            WriteMessage("Stopping...");
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();

            return true;
        }
    }
}
