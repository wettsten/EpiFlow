using NServiceBus;
using System;
using System.IO;
using System.Threading;
using EpiFlow.DataAccess;
using EpiFlow.Messages.Commands;
using System.Configuration;

namespace EpiFlow.FileWatcher.FileWatching
{
    public class NewFileHandler : IWantToRunWhenBusStartsAndStops
    {
        private FileSystemWatcher _fsWatcher;
        private IBus _bus;
        private IDatabaseReader _databaseReader;

        public NewFileHandler(IBus bus, IDatabaseReader databaseReader)
        {
            _bus = bus;
            _databaseReader = databaseReader;
            _fsWatcher = new FileSystemWatcher (ConfigurationManager.AppSettings["pathToWatch"])
            {
                EnableRaisingEvents = true,
                Filter = ConfigurationManager.AppSettings["fileFilter"],
                IncludeSubdirectories = false
            };
        }
        private void _fsWatcher_Created(object sender, FileSystemEventArgs e)
        {
            ProcessFile(e.FullPath);
        }

        public void Start()
        {
            _fsWatcher.Created += _fsWatcher_Created;
            if (ConfigurationManager.AppSettings["scanOnStartup"].ToLower().Equals("true"))
            {
                ScanFolder();
            }
        }

        public void Stop()
        {
            _fsWatcher.Created -= _fsWatcher_Created;
        }

        private void ProcessFile(string filename)
        {
            var fiNewFile = new FileInfo(filename);
            if (fiNewFile.Length == 0)
            {
                return;
            }

            // Check if file is ready
            while (!IsFileReady(fiNewFile.FullName))
            {
                Thread.Sleep(100);
            }

            // Check if file conversion is known
            var episode = _databaseReader.FindEpisodeByOriginalFilename(fiNewFile.Name);
            if (episode == null)
            {
                // Check if file is already final name
                episode = _databaseReader.FindEpisodeByNewFilename(fiNewFile.Name);
                if (episode == null)
                {
                    _bus.Send<ILookupEpisodeCommand>(i =>
                    {
                        i.FilePath = fiNewFile.DirectoryName;
                        i.OriginalFilename = fiNewFile.Name;
                    });
                }
            }
            else
            {
                _bus.Send<IChangeFilenameCommand>(i =>
                {
                    i.FilePath = fiNewFile.DirectoryName;
                    i.OriginalFilename = fiNewFile.Name;
                    i.NewFilename = episode.NewFilename;
                });
            }
        }

        private void ScanFolder()
        {
            var dir = new DirectoryInfo(ConfigurationManager.AppSettings["pathToWatch"]);
            foreach (var file in dir.GetFiles(ConfigurationManager.AppSettings["fileFilter"], SearchOption.TopDirectoryOnly))
            {
                ProcessFile(file.FullName);
            }
        }

        private bool IsFileReady(string filename)
        {
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
