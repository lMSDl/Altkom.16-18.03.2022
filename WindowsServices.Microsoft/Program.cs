using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace WindowsServices.Microsoft
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new CustomService();

            if (Environment.UserInteractive)//args.Any() && args[0] == "debug")
            {
                service.Start(args);
                Thread.Sleep(15000);
                service.Stop();

            }
            else
            ServiceBase.Run(service);
        }
    }
}
