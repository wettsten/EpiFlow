using EpiFlow.Common;
using EpiFlow.Data.Raven;
using EpiFlow.DataAccess;
using EpiFlow.Messages.Commands;
using NServiceBus;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EpiFlow.ManualUI.Handlers.Commands
{
    public class ManuallyNameEpisodeCommandHandler : IHandleMessages<IManuallyNameEpisodeCommand>
    {
        private IContainer _container;
        private IDatabaseReader _dbReader;
        private IEpisodeSearcher _episodeSearcher;
        private EpisodeConversion _episode;
        private ManualResetEvent _mre;
        private TimeSpan _mreTimeout;

        public ManuallyNameEpisodeCommandHandler(IDatabaseReader dbReader, IEpisodeSearcher episodeSearcher, IContainer container)
        {
            _dbReader = dbReader;
            _episodeSearcher = episodeSearcher;
            _mre = new ManualResetEvent(false);
            _mreTimeout = new TimeSpan(0, int.Parse(ConfigurationManager.AppSettings["windowTimeoutMinutes"]), 0);
            _container = container;
        }

        [STAThread]
        public void Handle(IManuallyNameEpisodeCommand message)
        {
            _mre.Reset();
            string filename = Path.GetFileName(message.Filename).Remove(Path.GetFileName(message.Filename).LastIndexOf('.'));
            GetEpisode(filename);
            switch (message.Action)
            {
                case "lookup":
                    //send to display
                    ShowLookupWindow();
                    _mre.WaitOne(_mreTimeout);
                    break;
                case "name":
                    break;
            }

        }

        private bool GetEpisode(string filename)
        {
            _episode = _dbReader.FindEpisodeByOriginalFilename(filename);
            if (_episode == null)
            {
                _episode = new EpisodeConversion
                {
                    OriginalFilename = filename
                };
                return _episodeSearcher.SearchForEpisode(_episode);
            }
            return true;
        }
        public void ShowLookupWindow()
        {
            var d = Application.Current.Dispatcher;
            if (d.CheckAccess())
                OnChangedInMainThread();
            else
                d.BeginInvoke((Action)OnChangedInMainThread);
        }

        private void OnChangedInMainThread()
        {
            var lookup = new LookupWindow(_container.GetInstance<ISiteReader>());
            lookup.LoadData(_episode);
            lookup.ShowDialog();
            _mre.Set();
        }
    }
}
