using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsServices.Microsoft
{
    public class CustomService : ServiceBase
    {
        private string _fileName = "c:\\CustomService\\MicrosoftService.txt";

        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
            File.AppendAllText(_fileName, $"{DateTime.Now.ToShortTimeString()}: {message}");
        }

        private CancellationTokenSource CancellationTokenSource { get; set; }

        public void Start(string[] args)
        {
            OnStart(args);
        }
        protected override void OnStart(string[] args)
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

            base.OnStart(args);
        }
        public void Stop(string[] args)
        {
            OnStop();
        }
        protected override void OnStop()
        {
            WriteMessage("Stopping...");
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            base.OnStop();
        }

    }
}
