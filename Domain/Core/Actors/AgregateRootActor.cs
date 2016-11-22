using Akka.Actor;
using Akka.Persistence;
using Domain.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Actors
{
    public class AgregateRootActor<TState> : ReceivePersistentActor, IEventPublisher
        where TState : class, IAgregateState, new()
    {
        private TState _state;
        private TimeSpan _saveOnTimeout;

        protected string _persistanceId;
        public override string PersistenceId
        {
            get
            {
                return $"{_state.GetType().Name}_{_state.Id.ToString()}";
            }
        }

        public AgregateRootActor(
            Guid id,
            TimeSpan saveTimeout)
        {
            _saveOnTimeout = saveTimeout;
            _state = new TState { Id = id };

            ConfigureCommands();
            ConfigureEvents();
        }

        private void ConfigureEvents()
        {
            Recover<RecoveryCompleted>(@event => CompleteRecovery(@event));
            Recover<SnapshotOffer>(@event => LoadState(@event));
            Recover<IEvent>(@event => ApplyEvent(@event));
        }

        private void ConfigureCommands()
        {
            Command<ReceiveTimeout>(command => SaveState(command));
            Command<SaveSnapshotSuccess>(command => DestoryAgregate(command));
            Command<ICommand>(command => RecieveCommand(command));
        }

        private bool ApplyEvent(IEvent @event)
        {
            return _state.ApplyEvent(@event as IEvent);
        }

        private bool LoadState(SnapshotOffer @event)
        {
            var snapshot = ((SnapshotOffer)@event).Snapshot as TState;
            if (snapshot == null)
            {
                return false;
            }
            _state = snapshot;
            return true;
        }

        private bool CompleteRecovery(RecoveryCompleted @event)
        {
            Context.SetReceiveTimeout(_saveOnTimeout);
            return true;
        }

        private bool RecieveCommand(ICommand command)
        {
            Context.SetReceiveTimeout(_saveOnTimeout);
            return _state.HandleCommand((ICommand)command, this);
        }

        private bool DestoryAgregate(SaveSnapshotSuccess command)
        {
            Context.Stop(Self);
            return true;
        }

        private bool SaveState(ReceiveTimeout command)
        {
            SaveSnapshot(_state);
            return true;
        }

        public void Publish(IEvent @event)
        {
            Persist(@event, x =>
            {
                _state.ApplyEvent(@event);
                Context.System.EventStream.Publish(@event);
            });
        }
    }
}
