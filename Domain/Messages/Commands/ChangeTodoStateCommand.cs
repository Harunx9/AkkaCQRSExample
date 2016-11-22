using Domain.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages.Commands
{
    public class ChangeTodoStateCommand : ITodoCommand
    {
        public Guid UUID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public ChangeTodoStateCommand(Guid uuid, string title, string description)
        {
            UUID = uuid;
            Title = title;
            Description = description;
        }
    }
}
