using Raven.Client;
using Raven.Client.Document;
using System.Linq;
using EpiFlow.Data.Raven;

namespace EpiFlow.DataAccess
{
    public class DatabaseReader : IDatabaseReader
    {
        private readonly IDocumentStore _documentStore;

        public DatabaseReader()
        {
            _documentStore = new DocumentStore { ConnectionStringName = "EpiFlowDB" };
            _documentStore.Initialize();
        }

        public void Dispose()
        {
            _documentStore.Dispose();
        }

        public EpisodeConversion FindEpisodeByOriginalFilename(string originalFilename)
        {
            EpisodeConversion episode = null;
            using (var session = _documentStore.OpenSession())
            {
                episode = session.Query<EpisodeConversion>().FirstOrDefault(i => i.OriginalFilename.Equals(originalFilename));
            }
            return episode;
        }

        public EpisodeConversion FindEpisodeByNewFilename(string newFilename)
        {
            EpisodeConversion episode = null;
            using (var session = _documentStore.OpenSession())
            {
                episode = session.Query<EpisodeConversion>().FirstOrDefault(i => i.NewFilename.Equals(newFilename));
            }
            return episode;
        }

        public EpisodeConversion FindEpisodeByOriginalAndNewFilenames(string originalFilename, string newFilename)
        {
            EpisodeConversion episode = null;
            using (var session = _documentStore.OpenSession())
            {
                episode = session
                    .Query<EpisodeConversion>()
                    .Where(i => i.OriginalFilename.Equals(originalFilename))
                    .FirstOrDefault(i => i.NewFilename.Equals(newFilename));
            }
            return episode;
        }
    }
}
