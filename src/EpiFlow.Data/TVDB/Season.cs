using System.Collections.Generic;
using System.Xml.Serialization;

namespace EpiFlow.Data.TVDB
{
    public class Season : TreeViewItemBase
    {
        public int SeriesId { get; set; }

        public int SeasonNumber { get; set; }

        public List<Episode> Episodes { get; set; }

        [XmlIgnore]
        public string ItemString => string.Format("Season {0}", SeasonNumber);
    }
}
