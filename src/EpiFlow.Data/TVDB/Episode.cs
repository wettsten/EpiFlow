using System.Xml.Linq;
using System.Xml.Serialization;

namespace EpiFlow.Data.TVDB
{
    public class Episode : TreeViewItemBase
    {
        [XmlElement("id")]
        public int Id { get; set; }

        public string EpisodeName { get; set; }

        public string Overview { get; set; }

        public int SeasonNumber { get; set; }

        [XmlElement("seriesid")]
        public int SeriesId { get; set; }

        public int EpisodeNumber { get; set; }

        [XmlIgnore]
        public string ItemString => string.Format("{0} - {1}", EpisodeNumber, EpisodeName);

        public static Episode FromXml(XContainer doc)
        {
            var serializer = new XmlSerializer(typeof(Episode));
            return (Episode)serializer.Deserialize(doc.CreateReader());
        }
    }
}
