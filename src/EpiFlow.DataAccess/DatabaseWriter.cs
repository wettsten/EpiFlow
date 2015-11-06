using Raven.Client;
using Raven.Client.Document;
using EpiFlow.Data.Raven;
using System.Linq;

namespace EpiFlow.DataAccess
{
    public class DatabaseWriter : IDatabaseWriter
    {
        private IDocumentStore _documentStore;

        public DatabaseWriter()
        {
            _documentStore = new DocumentStore { ConnectionStringName = "EpiFlowDB" };
            _documentStore.Initialize();
        }

        public void Dispose()
        {
            _documentStore.Dispose();
        }

        public void WriteEpisode(EpisodeConversion episode)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(episode);
                session.SaveChanges();
            }
        }
    }
}
