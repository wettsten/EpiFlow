using NServiceBus;
using EpiFlow.DataAccess;
using EpiFlow.Messages.Events;

namespace EpiFlow.FileWatcher.Handlers.Events
{
    public class EpisodeFoundEventHandler : IHandleMessages<IEpisodeFoundEvent>
    {
        private IDatabaseWriter _databaseWriter;
        private IDatabaseReader _databaseReader;

        public EpisodeFoundEventHandler(IDatabaseWriter databaseWriter, IDatabaseReader databaseReader)
        {
            _databaseWriter = databaseWriter;
            _databaseReader = databaseReader;
        }

        public void Handle(IEpisodeFoundEvent message)
        {
            var episode = _databaseReader.FindEpisodeByOriginalAndNewFilenames(message.Episode.OriginalFilename, message.Episode.NewFilename);
            if (episode == null)
            {
                _databaseWriter.WriteEpisode(message.Episode);
            }
        }
    }
}
