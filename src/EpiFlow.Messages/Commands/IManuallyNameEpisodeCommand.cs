using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpiFlow.Messages.Commands
{
    public interface IManuallyNameEpisodeCommand : ICommand
    {
        string Filename { get; set; }

        string Action { get; set; }
    }
}
