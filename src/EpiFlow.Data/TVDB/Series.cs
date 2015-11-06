using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EpiFlow.Data.TVDB
{
    public class Series : TreeViewItemBase
    {
        public Series()
        {
            Seasons = new List<Season>();
        }

        [XmlElement("id")]
        public int Id { get; set; }

        public string IMDB_ID { get; set; }

        public string Overview { get; set; }

        public string SeriesName { get; set; }

        public string lastupdated { get; set; }

        [XmlIgnore]
        public List<Season> Seasons { get; set; }

        [XmlIgnore]
        public string ItemString => SeriesName;

        public static Series FromXml(XContainer doc)
        {
            var serializer = new XmlSerializer(typeof(Series));
            return (Series)serializer.Deserialize(doc.CreateReader());
        }
    }
}
