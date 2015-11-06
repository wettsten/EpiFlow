using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EpiFlow.Data.Raven;
using EpiFlow.Data.TVDB;
using EpiFlow.DataAccess;

namespace EpiFlow.Common
{
    public class EpisodeSearcher : IEpisodeSearcher
    {
        private ISiteReader _siteReader;
        private EpisodeConversion _episode;

        public EpisodeSearcher(ISiteReader siteReader)
        {
            _siteReader = siteReader;
        }

        public bool SearchForEpisode(EpisodeConversion episode)
        {
            _episode = episode;
            ParseFilename();
            return SearchForSeries();
        }

        private void ParseFilename()
        {
            var x = _episode.OriginalFilename.Split('.', ' ', '-').Where(i => !string.IsNullOrWhiteSpace(i));
            var name = new List<string>();
            string s0e0Regex = @"[sS]\d{2}[Ee]\d{2}";
            int s0e0;
            foreach (string str in x)
            {
                if (Regex.Match(str, s0e0Regex).Success)
                {
                    _episode.SeasonNumber = int.Parse(str.Substring(1, 2));
                    _episode.EpisodeNumber = int.Parse(str.Substring(4, 2));
                    break;
                }
                else if (int.TryParse(str, out s0e0) && name.Count > 0)
                {
                    if (str.Length == 3)
                    {
                        _episode.SeasonNumber = int.Parse(str.Substring(0, 1));
                        _episode.EpisodeNumber = int.Parse(str.Substring(1, 2));
                    }
                    else if (str.Length == 4)
                    {
                        _episode.SeasonNumber = int.Parse(str.Substring(0, 2));
                        _episode.EpisodeNumber = int.Parse(str.Substring(2, 2));
                    }
                    break;
                }
                name.Add(str);
            }
            _episode.SearchSeriesName = string.Join(" ", name);
        }

        private bool SearchForSeries()
        {
            string seriesName = _episode.SearchSeriesName;
            var seriesFound = new List<Series>();
            for (int j = 0; j < 5; j++)
            {
                string testSeriesName = seriesName;
                switch (j)
                {
                    case 1:
                        testSeriesName = seriesName.Substring(seriesName.IndexOf(' ') + 1);
                        break;
                    case 2:
                        testSeriesName = seriesName.Substring(0, seriesName.LastIndexOf(' '));
                        break;
                    case 3:
                        testSeriesName = seriesName.Substring(seriesName.IndexOf(' ') + 1, seriesName.LastIndexOf(' ') - seriesName.IndexOf(' ') - 1);
                        break;
                    case 4:
                        testSeriesName = seriesName.Contains(" and ") ? seriesName.Replace(" and ", "") : seriesName;
                        break;
                }
                testSeriesName = testSeriesName.Trim();
                seriesFound = _siteReader.SearchSeries(testSeriesName, true);
                if (seriesFound.Any())
                {
                    foreach (var series in seriesFound)
                    {
                        var season = series.Seasons.FirstOrDefault(i => i.SeasonNumber == _episode.SeasonNumber);
                        var episode = season?.Episodes.FirstOrDefault(i => i.EpisodeNumber == _episode.EpisodeNumber);
                        if (episode != null)
                        {
                            _episode.FoundSeriesName = series.SeriesName;
                            _episode.FoundEpisodeName = episode.EpisodeName;
                            _episode.SeriesId = series.Id;
                            _episode.EpisodeId = episode.Id;
                            _episode.SearchSeriesName = testSeriesName;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
