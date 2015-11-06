using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EpiFlow
{
    public partial class SearchForm : Form
    {
        private bool _running;
        private Queue<string> _q;

        public SearchForm()
        {
            InitializeComponent();
            _q = new Queue<string>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                DoWork(args[1], true);
            }
            else if (args.Length == 3)
                AddToQueue(args[1]);
        }

        private void Fin()
        {
            if (InvokeRequired)
            {
                Invoke(new VoidDelegate(Fin));
                return;
            }
            this.Close();
        }

        public void AddToQueue(string path)
        {
            _q.Enqueue(path);
            if (!_running)
            {
                Thread pq = new Thread(new ThreadStart(ProcessQueue));
                pq.Start();
            }
            this.WindowState = FormWindowState.Minimized;
        }

        private void ProcessQueue()
        {
            while (_q.Count > 0)
            {
                _running = true;

                FileInfo oldFile = new FileInfo(_q.Peek());
                if (!File.Exists(oldFile.FullName))
                {
                    _q.Dequeue();
                    break;
                }
                DoWork(oldFile.FullName, false);
                _q.Dequeue();
            }
            _running = false;
            Fin();
        }

        public static void DoWork(string fullName, bool lookup)
        {
            SourceFile src = new SourceFile(fullName, lookup);
            NewFile nf = new NewFile();
            nf.LoadInfo(src);
            if (src.Series != null && src.Episode != null && !lookup)
            {
                string strEpisode = (src.Episode.Episode < 10) ? "0" + src.Episode.Episode.ToString() : src.Episode.Episode.ToString();
                string strSeason = src.Episode.Season.ToString();
                if (MessageBox.Show(string.Format("Old name: '{0}'\nNew name: '{1} - {2}{3} - {4}'\nIs this correct?", fullName, src.Series.Name, strSeason, strEpisode, src.Episode.Name), "Found a match!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FileInfo oldFile = new FileInfo(fullName);
                    while (true)
                    {
                        Rename rn = new Rename(string.Format(@"{0} - {1}{2} - {3}", src.Series.Name, strSeason, strEpisode, src.Episode.Name));
                        DialogResult res = rn.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            if (File.Exists(oldFile.DirectoryName + "\\" + rn.NewFile + oldFile.Extension))
                            {
                                if (MessageBox.Show(string.Format("The file: {0} already exists! Try another name.", rn.NewFile), "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                                    break;
                            }
                            else
                            {
                                string newName = oldFile.DirectoryName + "\\" + rn.NewFile + oldFile.Extension;
                                if (File.Exists(oldFile.FullName))
                                {
                                    int count = 0;
                                    while (true)
                                    {
                                        try
                                        {
                                            oldFile.MoveTo(newName);
                                            break;
                                        }
                                        catch
                                        {
                                            Thread.Sleep(1000);
                                            count++;
                                            if (count == 60)
                                            {
                                                MessageBox.Show(string.Format("File cannot be moved after {0} attempts! Program will now terminate.", count), "File cannot be moved!", MessageBoxButtons.OK);
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                    MessageBox.Show("Original file cannot be found! Program will now terminate.", "File not found!", MessageBoxButtons.OK);
                                return;
                            }
                        }
                        else if (res == DialogResult.Cancel)
                            break;
                    }
                }
            }
            nf.ShowDialog();
        }
    }
}
