using System;
using EpiFlow.Data.Raven;

namespace EpiFlow.DataAccess
{
    public interface IDatabaseWriter : IDisposable
    {
        void WriteEpisode(EpisodeConversion episode);
    }
}
