namespace EpiFlow.Data.Raven
{
    public class EpisodeConversion
    {
        public string OriginalFilename { get; set; }

        public string NewFilename { get; set; }

        public bool ManualOverride { get; set; }

        public string SearchSeriesName { get; set; }

        public int SeriesId { get; set; }

        public int SeasonNumber { get; set; }

        public int EpisodeNumber { get; set; }

        public int EpisodeId { get; set; }

        public string FoundSeriesName { get; set; }

        public string FoundEpisodeName { get; set; }
    }
}
