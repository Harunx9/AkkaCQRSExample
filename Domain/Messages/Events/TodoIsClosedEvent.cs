using Domain.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages.Events
{
    public class TodoIsClosedEvent : IEvent
    {
        public Guid UUID { get; private set; }
        public TodoIsClosedEvent(Guid uuid)
        {
            UUID = uuid;
        }
    }
}
