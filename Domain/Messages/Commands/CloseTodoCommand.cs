using Domain.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages.Commands
{
    public class CloseTodoCommand : ITodoCommand
    {
        public Guid UUID { get; private set; }
        public CloseTodoCommand(Guid uuid)
        {
            UUID = uuid;
        }
    }
}
