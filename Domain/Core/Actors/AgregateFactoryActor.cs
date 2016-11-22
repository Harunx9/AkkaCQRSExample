using Akka.Actor;
using Domain.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Actors
{
    public interface IAgregateIdentity
    {
        Guid Id { get; }
    }

    public class BaseIdentity : IAgregateIdentity
    {
        public Guid Id { get; }

        public BaseIdentity(Guid id)
        {
            Id = id;
        }
    }

    public class AgregateFactoryActor<TState, TIndentity> : TypedActor
        where TState : class, IAgregateState, new()
        where TIndentity : IAgregateIdentity

    {
        public virtual IActorRef CreateIfNotExist(TIndentity identity, string agregateName)
        {
            string name = $"{agregateName}_{identity.Id.ToString()}";
            var child = Context.Child(name);
            if (child.IsNobody())
            {
                var props = CreateProps(identity);
                child = Context.ActorOf(props, name);
            }

            return child;
        }

        public virtual Props CreateProps(TIndentity identity)
        {
            return Props.Create<AgregateRootActor<TState>>(() => new AgregateRootActor<TState>(identity.Id, TimeSpan.FromSeconds(30d)));
        }
    }
}
