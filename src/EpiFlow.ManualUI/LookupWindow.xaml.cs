using EpiFlow.Data.Raven;
using EpiFlow.Data.TVDB;
using EpiFlow.DataAccess;
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
using System.Windows.Shapes;

namespace EpiFlow.ManualUI
{
    /// <summary>
    /// Interaction logic for LookupWindow.xaml
    /// </summary>
    public partial class LookupWindow : Window
    {
        private ISiteReader _siteReader;

        public static readonly DependencyProperty SelectedSeriesProperty = DependencyProperty.Register("SelectedSeries", typeof(Series), typeof(LookupWindow), new UIPropertyMetadata(new Series()));

        public Series SelectedSeries
        {
            get { return (Series)GetValue(SelectedSeriesProperty); }
            set { SetValue(SelectedSeriesProperty, value); }
        }

        public static readonly DependencyProperty SelectedEpisodeProperty = DependencyProperty.Register("SelectedEpisode", typeof(Episode), typeof(LookupWindow), new UIPropertyMetadata(new Episode()));

        public Episode SelectedEpisode
        {
            get { return (Episode)GetValue(SelectedEpisodeProperty); }
            set { SetValue(SelectedEpisodeProperty, value); }
        }

        public static readonly DependencyProperty AllSeriesProperty = DependencyProperty.Register("AllSeries", typeof(List<Series>), typeof(LookupWindow), new UIPropertyMetadata(new List<Series>()));

        public List<Series> AllSeries
        {
            get { return (List<Series>)GetValue(AllSeriesProperty); }
            set { SetValue(AllSeriesProperty, value); }
        }

        public LookupWindow(ISiteReader siteReader)
        {
            InitializeComponent();
            _siteReader = siteReader;
        }

        public void LoadData(EpisodeConversion ep)
        {
            searchText.Text = ep.SearchSeriesName;
            AllSeries = _siteReader.SearchSeries(searchText.Text, true);
            var series = AllSeries.FirstOrDefault(i => i.Id.Equals(ep.SeriesId));
            if (series != null)
            {
                series.IsExpanded = true;
                var season = AllSeries
                    .SelectMany(i => i.Seasons.Where(k => k.SeasonNumber.Equals(ep.SeasonNumber)))
                    .FirstOrDefault();
                if (season != null)
                {
                    season.IsExpanded = true;
                    var episode = AllSeries
                        .SelectMany(i => i.Seasons.SelectMany(j => j.Episodes.Where(k => k.Id.Equals(ep.EpisodeId)))).FirstOrDefault();
                    if (episode != null)
                    {
                        episode.IsSelected = true;
                    }
                }
                else
                {
                    series.IsSelected = true;
                }
            }
            SetSeriesAndEpisode();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            AllSeries = _siteReader.SearchSeries(searchText.Text, true);
            SetSeriesAndEpisode();
        }

        private void Series_Click(object sender, RoutedEventArgs e)
        {
            var series = (sender as Button)?.DataContext as Series;
            series.IsSelected = true;
            SetSeriesAndEpisode();
        }

        private void Season_Click(object sender, RoutedEventArgs e)
        {
            var season = (sender as FrameworkElement)?.DataContext as Season;
            season.IsSelected = true;
            SetSeriesAndEpisode();
        }

        private void Episode_Click(object sender, RoutedEventArgs e)
        {
            var episode = (sender as FrameworkElement)?.DataContext as Episode;
            episode.IsSelected = true;
            SetSeriesAndEpisode();
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //TODO handle if keyboard is used to select different item -> point textblocks at selected items in AllSeries instead of their own properties
        }

        private void SetSeriesAndEpisode()
        {
            SelectedSeries = AllSeries.FirstOrDefault(i => i.IsSelected);
            SelectedEpisode = null;
            if (SelectedSeries == null)
            {
                SelectedSeries = AllSeries.FirstOrDefault(i => i.Seasons.FirstOrDefault(j => j.IsSelected) != null);
                if (SelectedSeries == null)
                {
                    SelectedSeries = AllSeries.FirstOrDefault(i => i.Seasons.FirstOrDefault(j => j.Episodes.FirstOrDefault(k => k.IsSelected) != null) != null);
                    SelectedEpisode = AllSeries.SelectMany(i => i.Seasons.SelectMany(j => j.Episodes.Where(k => k.IsSelected))).FirstOrDefault();
                }
            }
        }
    }
}
