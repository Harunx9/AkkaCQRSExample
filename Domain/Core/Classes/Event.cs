using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Classes
{
    public interface IEvent
    {
    }

    public interface IApplyEvent<in T> where T : IEvent
    {
        void Apply(T @event);
    }

    public interface IEventPublisher
    {
        void Publish(IEvent @event);
    }
}
