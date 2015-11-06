using NServiceBus;

namespace EpiFlow.Messages.Commands
{
    public interface IChangeFilenameCommand : ICommand
    {
        string FilePath { get; set; }
        string OriginalFilename { get; set; }
        string NewFilename { get; set; }
    }
}
