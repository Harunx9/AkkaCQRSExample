using Akka.Actor;
using Akka.Event;
using Domain.Core.Persistance;
using Domain.Messages.Events;
using Domain.ReadModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.WriteModel
{
    public class TodoRepositoryActor : TypedActor,
        IHandle<TodoIsChangedEvent>,
        IHandle<TodoIsClosedEvent>,
        IHandle<TodoIsCreatedEvent>
    {
        private readonly IMongoConnection _db;
        private readonly ILoggingAdapter _logger;

        public TodoRepositoryActor(IMongoConnection db)
        {
            _db = db;
            _logger = Context.GetLogger();
            SubscribeEvents();
        }

        public void Handle(TodoIsCreatedEvent message)
        {
            var collection = _db.Database.GetCollection<TodoDetail>("todos");
            collection.InsertOne(new TodoDetail
            {
                UUID = message.UUID,
                Title = message.Title,
                Description = string.Empty,
                IsActive = true
            });
            _logger.Info($"Todo {message.UUID} is added");
        }

        public void Handle(TodoIsClosedEvent message)
        {
            var collection = _db.Database.GetCollection<TodoDetail>("todos");
            var item = collection.AsQueryable().First(x => x.UUID == message.UUID);
            item.IsActive = false;
            collection.ReplaceOne(x => x.UUID == message.UUID, item);
            _logger.Info($"Todo {message.UUID} is closed");
        }

        public void Handle(TodoIsChangedEvent message)
        {
            var collection = _db.Database.GetCollection<TodoDetail>("todos");
            var item = collection.AsQueryable().First(x => x.UUID == message.UUID);
            item.Title = message.Title;
            item.Description = message.Description;
            collection.ReplaceOne(x => x.UUID == message.UUID, item);
            _logger.Info($"Todo {message.UUID} is changed");
        }

        protected override void PostStop()
        {
            UnsubscribeEvents();
            base.PostStop();
        }

        public void SubscribeEvents()
        {
            Context.System.EventStream.Subscribe(Self, typeof(TodoIsChangedEvent));
            Context.System.EventStream.Subscribe(Self, typeof(TodoIsClosedEvent));
            Context.System.EventStream.Subscribe(Self, typeof(TodoIsCreatedEvent));
        }

        public void UnsubscribeEvents()
        {
            Context.System.EventStream.Unsubscribe(Self, typeof(TodoIsChangedEvent));
            Context.System.EventStream.Unsubscribe(Self, typeof(TodoIsClosedEvent));
            Context.System.EventStream.Unsubscribe(Self, typeof(TodoIsCreatedEvent));
        }
    }
}
