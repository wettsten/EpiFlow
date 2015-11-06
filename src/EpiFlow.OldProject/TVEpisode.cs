namespace EpiFlow
{
    public class TVEpisode
    {
        private string _name;
        private string _description;
        private string _iD;
        private int _season;
        private int _episode;

        public TVEpisode(string name, string description, string iD, int season, int episode)
        {
            // TODO: Complete member initialization
            this._name = name;
            this._description = description;
            this._iD = iD;
            this._season = season;
            this._episode = episode;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public int Season
        {
            get { return _season; }
            set { _season = value; }
        }

        public int Episode
        {
            get { return _episode; }
            set { _episode = value; }
        }
    }
}
