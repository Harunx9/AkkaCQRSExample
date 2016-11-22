using System;
using Akka.Actor;
using Topshelf;
using Serilog;
using Domain;

namespace Domain.App
{
    internal class AkkaServiceHost : ServiceControl
    {
        private ActorSystem _system;

        public bool Start(HostControl hostControl)
        {
            _system = Akka.Actor.ActorSystem.Create("RemoteSystem");
            _system.ActorOf<DomainEntry>("domain");
            ConfigureLogger();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _system.Terminate();
            return true;
        }

        private void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
               .WriteTo.ColoredConsole(outputTemplate: "{Timestamp:HH:mm}; [{Level}]; [{SourceContext}]; ({Thread}); {Message}{NewLine}{Exception}")
               .CreateLogger();
        }

    }
}