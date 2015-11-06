using System.Collections.Generic;
using EpiFlow.Data.TVDB;

namespace EpiFlow.DataAccess
{
    public interface ISiteReader
    {
        List<Series> SearchSeries(string searchString, bool withDetails);
        Series GetSeries(int id);
        Series GetSeriesAll(int id);
        Episode GetEpisode(int id);
        Episode GetSeasonEpisode(int seriesId, int seasonNumber, int episodeNumber);
    }
}
