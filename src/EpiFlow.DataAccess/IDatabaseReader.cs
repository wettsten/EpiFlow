using System;
using EpiFlow.Data.Raven;

namespace EpiFlow.DataAccess
{
    public interface IDatabaseReader : IDisposable
    {
        EpisodeConversion FindEpisodeByOriginalFilename(string originalFilename);

        EpisodeConversion FindEpisodeByNewFilename(string newFilename);

        EpisodeConversion FindEpisodeByOriginalAndNewFilenames(string originalFilename, string newFilename);
    }
}
