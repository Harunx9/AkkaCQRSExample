using Akka.Actor;
using Akka.Event;
using Domain.Core.Actors;
using Domain.Core.Classes;
using Domain.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.WriteModel
{
    public class TodoFactoryActor : AgregateFactoryActor<TodoAgregateState, BaseIdentity>,
        IHandle<ITodoCommand>
    {
        public void Handle(ITodoCommand message)
        {
            var logger = Context.GetLogger();
            var child = CreateIfNotExist(new BaseIdentity(message.UUID), "Todo");
            child.Forward(message);
            logger.Info($"Message {message.GetType().Name} is forwarded to agrgate");
        }
    }
}
