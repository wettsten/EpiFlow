using NServiceBus;
using System.IO;
using EpiFlow.Messages.Commands;

namespace EpiFlow.FileWatcher.Handlers.Commands
{
    public class ChangeFilenameCommandHandler : IHandleMessages<IChangeFilenameCommand>
    {
        public void Handle(IChangeFilenameCommand message)
        {
            if (
                File.Exists(Path.Combine(message.FilePath, message.OriginalFilename))
                && !File.Exists(Path.Combine(message.FilePath, message.NewFilename))
                && !string.IsNullOrEmpty(message.NewFilename))
            {
                File.Move(Path.Combine(message.FilePath, message.OriginalFilename), Path.Combine(message.FilePath, message.NewFilename));
            }
        }
    }
}
