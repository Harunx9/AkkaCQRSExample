using Domain.Core.Classes;
using Domain.Messages.Commands;
using Domain.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.WriteModel
{
    public enum TodoState
    {
        ACTIVE,
        CLOSED
    }

    public class TodoAgregateState : AgregateState,
        ICommandHandler<ChangeTodoStateCommand>,
        ICommandHandler<CloseTodoCommand>,
        ICommandHandler<CreateTodoCommand>,
        IApplyEvent<TodoIsChangedEvent>,
        IApplyEvent<TodoIsClosedEvent>,
        IApplyEvent<TodoIsCreatedEvent>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TodoState State { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public void Handle(ChangeTodoStateCommand command, IEventPublisher publisher)
        {
            if (State != TodoState.CLOSED)
                publisher.Publish(new TodoIsChangedEvent(command.UUID, command.Title, command.Description));
        }

        public void Handle(CloseTodoCommand command, IEventPublisher publisher)
        {
            if (State != TodoState.CLOSED)
                publisher.Publish(new TodoIsClosedEvent(command.UUID));
        }

        public void Handle(CreateTodoCommand command, IEventPublisher publisher)
        {
            if (State != TodoState.CLOSED)
                publisher.Publish(new TodoIsCreatedEvent(command.UUID, command.Title));
        }

        public void Apply(TodoIsChangedEvent @event)
        {
            Title = @event.Title;
            Description = @event.Description;
        }

        public void Apply(TodoIsClosedEvent @event)
        {
            State = TodoState.CLOSED;
        }

        public void Apply(TodoIsCreatedEvent @event)
        {
            Title = @event.Title;
        }
    }
}
