using Domain.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages.Commands
{
    public class CreateTodoCommand : ITodoCommand
    {
        public Guid UUID { get; private set; }
        public string Title { get; private set; }
        public CreateTodoCommand(Guid uuid, string title)
        {
            UUID = uuid;
            Title = title;
        }
    }
}
