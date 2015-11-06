using NServiceBus;

namespace EpiFlow.Messages.Commands
{
    public interface ILookupEpisodeCommand : ICommand
    {
        string FilePath { get; set; }
        string OriginalFilename { get; set; }
    }
}
