using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Domain.App
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(host =>
            {
                host.SetServiceName("Akka System");
                host.SetDisplayName("Akka System Host");

                host.UseAssemblyInfoForServiceInfo();
                host.RunAsLocalSystem();
                host.StartAutomatically();
                host.Service<AkkaServiceHost>();
                host.EnableServiceRecovery(x => x.RestartService(1));
            });
        }
    }
}
