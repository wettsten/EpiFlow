namespace EpiFlow
{
    public class TVSeries
    {
        private string _name;
        private string _description;
        private string _iD;
        private string _iMDB;

        public TVSeries(string name, string description, string iD, string iMDB)
        {
            // TODO: Complete member initialization
            _name = name;
            _description = description;
            _iD = iD;
            _iMDB = iMDB;
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

        public string IMDB
        {
            get { return _iMDB; }
            set { _iMDB = value; }
        }
    }
}
