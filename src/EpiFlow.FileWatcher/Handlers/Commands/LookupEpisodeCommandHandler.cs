using NServiceBus;
using System.Configuration;
using EpiFlow.Common;
using EpiFlow.Data.Raven;
using EpiFlow.Messages.Commands;
using EpiFlow.Messages.Events;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace EpiFlow.FileWatcher.Handlers.Commands
{
    public class LookupEpisodeCommandHandler : IHandleMessages<ILookupEpisodeCommand>
    {
        private IBus _bus;
        private IEpisodeSearcher _episodeSearcher;

        public LookupEpisodeCommandHandler(IBus bus, IEpisodeSearcher episodeSearcher)
        {
            _bus = bus;
            _episodeSearcher = episodeSearcher;
        }

        public void Handle(ILookupEpisodeCommand message)
        {
            var episode = new EpisodeConversion
            {
                OriginalFilename = message.OriginalFilename,
                ManualOverride = false
            };
            bool found = _episodeSearcher.SearchForEpisode(episode);
            if (found)
            {
                SetNewFilename(episode);
                _bus.Send<IChangeFilenameCommand>(i =>
                {
                    i.NewFilename = episode.NewFilename;
                    i.OriginalFilename = message.OriginalFilename;
                    i.FilePath = message.FilePath;
                });
                _bus.Publish<IEpisodeFoundEvent>(i =>
                {
                    i.Episode = episode;
                });
            }           
        }

        private void SetNewFilename(EpisodeConversion episode)
        {
            string mask = ConfigurationManager.AppSettings["filenameMask"];
            episode.NewFilename = mask
                .Replace("{seriesName}", episode.FoundSeriesName)
                .Replace("{seasonNumber}", episode.SeasonNumber.ToString())
                .Replace("{episodeNumber}", episode.EpisodeNumber.ToString("00"))
                .Replace("{episodeName}", episode.FoundEpisodeName);
            string extension = episode.OriginalFilename.Substring(episode.OriginalFilename.LastIndexOf('.'));
            episode.NewFilename += extension;
            
            foreach (var c in Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars()))
            {
                episode.NewFilename.Replace(c.ToString(), string.Empty);
            }
        }
    }
}
