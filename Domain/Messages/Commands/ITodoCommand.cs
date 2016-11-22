using Domain.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages.Commands
{
    public interface ITodoCommand : ICommand
    {
        Guid UUID { get; }
    }
}
