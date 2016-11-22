using Akka.Actor;
using Domain.Core.Classes;
using Domain.Core.Persistance;
using Domain.Messages.Commands;
using Domain.WriteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DomainEntry : ReceiveActor
    {
        private IActorRef _factory;
        private IActorRef _repository;

        public DomainEntry()
        {
            ConfigureChildren();
            ConfigureMessages();
        }

        private void ConfigureChildren()
        {
            var cfg = Context.System.Settings.Config;
            var connection = cfg.GetString("akka.read-db.mongo-connection", "");
            _factory = Context.ActorOf(Props.Create<TodoFactoryActor>(), "TodoFactory");
            _repository = Context.ActorOf(Props.Create(() => new TodoRepositoryActor( 
                new MongoConnection(connection))), 
                "TodoRepository");
        }

        private void ConfigureMessages()
        {
            Receive<ITodoCommand>(x => _factory.Tell(x));
        }
    }
}
