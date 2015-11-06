using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EpiFlow
{
    public class SourceFile
    {
        private FileInfo _file;
        private string _s00e00;
        private string _seriesSearch;
        private int _seasonNumber;
        private int _episodeNumber;
        private TVSeries _series;
        private TVEpisode _episode;
        private bool _lookup;

        public string Filename
        {
            get { return _file.Name; }
        }

        public string FullName
        {
            get { return _file.FullName; }
        }

        public TVSeries Series
        {
            get { return _series; }
        }

        public TVEpisode Episode
        {
            get { return _episode; }
        }

        public bool Lookup
        {
            get { return _lookup; }
        }

        public string SeriesSearch
        {
            get { return _seriesSearch; }
        }

        public SourceFile(string filename, bool lookup)
        {
            _file = new FileInfo(filename);
            _lookup = lookup;
            _seriesSearch = SiteReader.ParseShowName(_file.Name);
            _s00e00 = SiteReader.ParseSeasonEpisode(_file.Name);
            _seasonNumber = SiteReader.ParseSeasonNumber(_s00e00);
            _episodeNumber = SiteReader.ParseEpisodeNumber(_s00e00);
            _series = null;
            _episode = null;
            LoadSeries();
            if (_series != null)
                LoadEpisode();
        }

        private void LoadSeries()
        {
            List<TVSeries> shows = new List<TVSeries>();
            for (int j = 0; j < 5; j++)
            {
                string testShowName = _seriesSearch;
                switch (j)
                {
                    case 1:
                        testShowName = (_seriesSearch.Contains(" and ")) ? _seriesSearch.Replace(" and ", " & ") : _seriesSearch;
                        break;
                    case 2:
                        testShowName = _seriesSearch.Substring(_seriesSearch.IndexOf(' ') + 1);
                        break;
                    case 3:
                        testShowName = _seriesSearch.Substring(0, _seriesSearch.LastIndexOf(' '));
                        break;
                    case 4:
                        testShowName = _seriesSearch.Substring(_seriesSearch.IndexOf(' ') + 1, _seriesSearch.LastIndexOf(' ') - _seriesSearch.IndexOf(' ') - 1);
                        break;
                    default:
                        testShowName = _seriesSearch;
                        break;
                }
                shows = SiteReader.GetSeries(testShowName);
                if (shows.Count > 0)
                    break;
            }
            if (shows.Count > 0)
                _series = shows.First();
        }

        private void LoadEpisode()
        {
            List<TVEpisode> episodes = new List<TVEpisode>();
            episodes = SiteReader.GetEpisodes(_series.ID);

            var eps = episodes.Where(i => i.Season == _seasonNumber && i.Episode == _episodeNumber);
            if (eps.Count() > 0)
                _episode = eps.First();
        }

        public void SetSeries(TVSeries series)
        {
            _series = series;
            if (_series != null)
                LoadEpisode();
        }
    }
}
