using EpiFlow.Common;
using EpiFlow.Data.Raven;
using EpiFlow.DataAccess;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EpiFlow.ManualUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IContainer _container;
        private IEpisodeSearcher _episodeSearcher;
        private IDatabaseReader _dbReader;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(IContainer container)
        {
            InitializeComponent();
            _container = container;
            _episodeSearcher = _container.GetInstance<IEpisodeSearcher>();
            _dbReader = _container.GetInstance<IDatabaseReader>();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var lookup = new LookupWindow(_container.GetInstance<ISiteReader>());
            lookup.LoadData(GetEpisode("NCIS - 1305 - Lockdown"));
            lookup.ShowDialog();
        }

        private EpisodeConversion GetEpisode(string filename)
        {
            var episode = _dbReader.FindEpisodeByOriginalFilename(filename);
            if (episode == null)
            {
                episode = new EpisodeConversion
                {
                    OriginalFilename = filename
                };
                _episodeSearcher.SearchForEpisode(episode);
                return episode;
            }
            return episode;
        }
    }
}
