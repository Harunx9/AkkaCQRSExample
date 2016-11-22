using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Classes
{
    public interface IAgregateState
    {
        Guid Id { get; set; }
        bool IsActive { get; set; }
        bool ApplyEvent(IEvent @event);
        bool HandleCommand(ICommand command, IEventPublisher publisher);
    }

    public abstract class AgregateState : IAgregateState
    {
        private Guid _id;
        public Guid Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == Guid.Empty)
                {
                    _id = value;
                }
            }
        }

        public AgregateState()
        {
            IsActive = true;
        }

        public bool IsActive { get; set; }

        public bool ApplyEvent(IEvent @event)
        {
            var eventHandler = GetType().GetMethod("Apply", new[] { @event.GetType() });

            if (eventHandler == null)
            {
                return false;
            }

            eventHandler.Invoke(this, new object[] { @event });
            return true;
        }

        public bool HandleCommand(ICommand command, IEventPublisher publisher)
        {
            var commandHandler = GetType().GetMethod("Handle", new[] { command.GetType(), typeof(IEventPublisher) });

            if (commandHandler == null)
            {
                return false;
            }

            commandHandler.Invoke(this, new object[] { command, publisher });
            return true;
        }
    }
}
