using Akka.Actor;
using Autofac;
using Autofac.Integration.Mvc;
using Domain.Core.Persistance;
using Domain.ReadModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gui
{
    public interface IDomainBus
    {
        void Send<T>(T command);
    }

    public class DomainBus : IDomainBus
    {
        private readonly ActorSelection _domain;
        private readonly ActorSystem _system;

        public DomainBus()
        {
            _system = ActorSystem.Create("LocalSystem");
            var config = _system.Settings.Config;
            _domain = _system.ActorSelection(config.GetString("akka.actor.address.entry"));
        }

        public void Send<T>(T command)
        {
            _domain.Tell(command, ActorRefs.Nobody);
        }
    }

    public static class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<MongoConnection>().As<IMongoConnection>().WithParameter("connectionString", ConfigurationManager.ConnectionStrings["mongoDb"].ConnectionString);
            builder.RegisterType<TodoReadSideService>().As<ITodoReadSideService>();

            builder.RegisterInstance<IDomainBus>(new DomainBus());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}