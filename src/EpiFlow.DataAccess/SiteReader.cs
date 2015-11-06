using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using EpiFlow.Data.TVDB;

namespace EpiFlow.DataAccess
{
    public class SiteReader : ISiteReader
    {
        private string _urlBase = string.Format(@"http://thetvdb.com/api/{0}/", ConfigurationManager.AppSettings["apikey"]);
        private string _searchUrl = @"http://www.thetvdb.com/api/GetSeries.php?seriesname=";

        public Episode GetEpisode(int id)
        {
            var doc = GetData(string.Format(@"{0}episodes/{1}/en.xml", _urlBase, id));
            if (doc == null) { return null; }
            return doc.Descendants("Episode").Select(i => Episode.FromXml(i)).FirstOrDefault();
        }

        public Episode GetSeasonEpisode(int seriesId, int seasonNumber, int episodeNumber)
        {
            var doc = GetData(string.Format(@"{0}series/{1}/default/{2}/{3}/en.xml", _urlBase, seriesId, seasonNumber, episodeNumber));
            if (doc == null) { return null; }
            return doc.Descendants("Episode").Select(i => Episode.FromXml(i)).FirstOrDefault();
        }

        public Series GetSeries(int id)
        {
            var doc = GetData(string.Format(@"{0}series/{1}/en.xml", _urlBase, id));
            if (doc == null) { return null; }
            return doc.Descendants("Series").Select(i => Series.FromXml(i)).FirstOrDefault();
        }

        public Series GetSeriesAll(int id)
        {
            var doc = GetData(string.Format(@"{0}series/{1}/all", _urlBase, id));
            if (doc == null) { return null; }
            var series = doc.Descendants("Series").Select(i => Series.FromXml(i)).FirstOrDefault();
            var episodes = doc.Descendants("Episode").Select(i => Episode.FromXml(i));
            foreach (var group in episodes.GroupBy(i => i.SeasonNumber))
            {
                series.Seasons.Add(new Season
                {
                    SeriesId = series.Id,
                    SeasonNumber = group.Key,
                    Episodes = group.ToList()
                });
            }
            return series;
        }

        public List<Series> SearchSeries(string searchString, bool withDetails)
        {
            var doc = GetData(_searchUrl + searchString);
            if (doc == null) { return null; }
            var ids = doc.Descendants("seriesid");
            return (withDetails)
                ? ids.Select(i => GetSeriesAll(int.Parse(i.Value))).ToList()
                : ids.Select(i => GetSeries(int.Parse(i.Value))).ToList();
        }

        private XDocument GetData(string url)
        {
            XDocument doc;
            try
            {
                doc = XDocument.Load(url);
            }
            catch (XmlException xEx)
            {
                return null;
            }
            catch (WebException wEx)
            {
                return null;
            }
            return doc;
        }
    }
}
