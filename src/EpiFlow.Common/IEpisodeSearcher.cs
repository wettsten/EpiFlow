using EpiFlow.Data.Raven;

namespace EpiFlow.Common
{
    public interface IEpisodeSearcher
    {
        bool SearchForEpisode(EpisodeConversion episode);
    }
}
