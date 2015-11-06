using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EpiFlow
{
    public static class SiteReader
    {
        public static List<TVSeries> GetSeries(string name)
        {
            XDocument doc = XDocument.Load(@"http://www.thetvdb.com/api/GetSeries.php?seriesname=" + name);
            List<TVSeries> shows = doc.Descendants("Series").Select(i => new TVSeries(
                        i.Element("SeriesName").Value,
                        (i.Element("Overview") == null) ? string.Empty : i.Element("Overview").Value,
                        i.Element("id").Value,
                        (i.Element("IMDB_ID") == null) ? string.Empty : i.Element("IMDB_ID").Value)).ToList();
            return shows;
        }

        public static List<TVEpisode> GetEpisodes(string seriesId)
        {
            XDocument doc = XDocument.Load(string.Format(@"http://thetvdb.com/api/82D3AAB05796490D/series/{0}/all/en.xml", seriesId));
            List<TVEpisode> episodes = doc.Descendants("Episode").Select(i => new TVEpisode(
                        i.Element("EpisodeName").Value,
                        i.Element("Overview").Value,
                        i.Element("id").Value,
                        int.Parse(i.Element("SeasonNumber").Value),
                        int.Parse(i.Element("EpisodeNumber").Value))).ToList();
            return episodes;
        }

        public static string ParseShowName(string fullName)
        {
            string[] x = fullName.Split('.', ' ', '-');
            StringBuilder sb = new StringBuilder();
            foreach (string str in x)
            {
                if ((str.Contains("0") || str.Contains("1") || str.Contains("2") || str.Contains("3") || str.Contains("4")
                    || str.Contains("5") || str.Contains("6") || str.Contains("7") || str.Contains("8") || str.Contains("9")) && sb.Length > 0)
                {
                    break;
                }
                else if (str.Length > 1)
                    sb.AppendFormat(" {0}", str);
            }
            if (sb[0] == ' ')
                sb.Remove(0, 1);
            return sb.ToString();
        }

        public static string ParseSeasonEpisode(string fullName)
        {
            string s00e00 = string.Empty;
            string[] x = fullName.Split('.', ' ', '-');
            StringBuilder sb = new StringBuilder();
            foreach (string str in x)
            {
                if ((str.Contains("0") || str.Contains("1") || str.Contains("2") || str.Contains("3") || str.Contains("4")
                    || str.Contains("5") || str.Contains("6") || str.Contains("7") || str.Contains("8") || str.Contains("9")) && sb.Length > 0)
                {
                    s00e00 = str;
                    break;
                }
                else if (str.Length > 1 && !str.ToLower().Equals("and"))
                    sb.AppendFormat(" {0}", str);
            }
            return s00e00;
        }

        public static TVSeries FindShow(string name, out string s00e00, out string series)
        {
            series = ParseShowName(name);
            s00e00 = ParseSeasonEpisode(name);

            List<TVSeries> shows = new List<TVSeries>();
            for (int j = 0; j < 5; j++)
            {
                string testShowName = series;
                switch (j)
                {
                    case 1:
                        testShowName = series.Substring(series.IndexOf(' ') + 1);
                        break;
                    case 2:
                        testShowName = series.Substring(0, series.LastIndexOf(' '));
                        break;
                    case 3:
                        testShowName = series.Substring(series.IndexOf(' ') + 1, series.LastIndexOf(' ') - series.IndexOf(' ') - 1);
                        break;
                    case 4:
                        testShowName = (series.Contains(" and ")) ? series.Remove(series.IndexOf(" and "), 4) : series;
                        break;
                    default:
                        testShowName = series;
                        break;
                }
                shows = GetSeries(testShowName);
                if (shows.Count > 0)
                    break;
            }
            if (shows.Count > 0)
                return shows[0];
            return null;
        }

        public static int ParseSeasonNumber(string s00e00)
        {
            int season = 0;
            switch (s00e00.Length)
            {
                case 3:
                    int.TryParse(s00e00.Substring(0, 1), out season);
                    break;
                case 4:
                    int.TryParse(s00e00.Substring(0, 2), out season);
                    break;
                case 6:
                    int.TryParse(s00e00.Substring(1, 2), out season);
                    break;
                default:
                    season = 0;
                    break;
            }
            return season;
        }

        public static int ParseEpisodeNumber(string s00e00)
        {
            int episode = 0;
            switch (s00e00.Length)
            {
                case 3:
                    int.TryParse(s00e00.Substring(1), out episode);
                    break;
                case 4:
                    int.TryParse(s00e00.Substring(2), out episode);
                    break;
                case 6:
                    int.TryParse(s00e00.Substring(4), out episode);
                    break;
                default:
                    episode = 0;
                    break;
            }
            return episode;
        }

        public static TVEpisode FindEpisode(string seriesId, string s00e00)
        {
            List<TVEpisode> episodes = new List<TVEpisode>();
            episodes = GetEpisodes(seriesId);

            int episodeNumber = ParseEpisodeNumber(s00e00);
            int seriesNumber = ParseSeasonNumber(s00e00);

            var eps = episodes.Where(i => i.Season == seriesNumber && i.Episode == episodeNumber);
            if (eps.Count() > 0)
                return eps.First();
            return null;
        }
    }
}
