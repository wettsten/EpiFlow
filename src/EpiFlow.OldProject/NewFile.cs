using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Linq;

namespace EpiFlow
{
    public delegate void VoidDelegate();
    public delegate void StringDelegate(string str);

    public partial class NewFile : Form
    {
        private List<TVSeries> _shows;
        private List<TVEpisode> _eps;
        private SourceFile _src;

        public NewFile()
        {
            InitializeComponent();
        }

        public void LoadInfo(SourceFile src)
        {
            _src = src;
            lbOldFile.Text = _src.Filename;
            string strSearch = _src.SeriesSearch;
            if (_src.Series != null)
                strSearch = _src.Series.Name;
            FillComboBox(strSearch);
        }

        private void FillComboBox(string series)
        {
            btnMove.Enabled = !_src.Lookup;
            lbSearch.Text = "Search for: " + series;

            _shows = SiteReader.GetSeries(series);

            cbSeries.Items.Clear();
            _shows.ForEach(i => cbSeries.Items.Add(i.Name));

            if (cbSeries.Items.Count > 0)
                cbSeries.SelectedIndex = 0;
            else
            {
                MessageBox.Show(string.Format("No documented series matched the query string: {0}. Use manual search to find the correct series.", series), "No match found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            _src.SetSeries(_shows.Single(r => r.Name == cbSeries.Text));
            if (_src.Series == null)
                return;
            llIMDB.Tag = _src.Series.IMDB;
            llIMDB.Enabled = !string.IsNullOrEmpty(_src.Series.IMDB);
            tbSeries.Text = _src.Series.Description;

            _eps = SiteReader.GetEpisodes(_src.Series.ID);

            if (_eps.Count() == 0)
                return;

            lbEpisodes.Items.Clear();
            //_eps.ForEach(i => lbEpisodes.Items.Add(string.Format("{0}/{1}: {2}", i.Season, i.Episode, i.Name)));
            int index = -1, cnt = 0;
            foreach (TVEpisode ep in _eps)
            {
                lbEpisodes.Items.Add(string.Format("{0}/{1}: {2}", ep.Season, ep.Episode, ep.Name));
                if (_src.Episode != null && index == -1)
                {
                    if (_src.Episode.Season.Equals(ep.Season) && _src.Episode.Episode.Equals(ep.Episode))
                        index = cnt;
                    cnt++;
                }
            }

            if (_src.Episode != null)
            {
                lbEpisodes.SelectedIndex = index;
            }
            //    if (!_error && !_lookup)
            //    {
            //        string epis = (ep < 10) ? "0" + ep.ToString() : ep.ToString();
            //        if (MessageBox.Show(string.Format("Old name: '{0}'\nNew name: '{1} - {2}{3} - {4}'\nIs this correct?", _oldFile.Name, cbSeries.Text, s.ToString(), epis, currEp[0].Name), "Found a match!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //        {
            //            MoveFile(string.Format(@"{0} - {1}{2} - {3}", cbSeries.Text, s.ToString(), epis, currEp[0].Name));
            //        }
            //    }
            //}
            //if (lbEpisodes.SelectedIndex < 0 && !_lookup && !_onceThru)
            //{
            //    if (cbSeries.SelectedIndex < cbSeries.Items.Count - 1)
            //    {
            //        cbSeries.SelectedIndex = cbSeries.SelectedIndex + 1;
            //        return;
            //    }
            //    else if (cbSeries.SelectedIndex == cbSeries.Items.Count - 1)
            //        _onceThru = true;
            //    lbEpisodes.SelectedIndex = 0;
            //    _error = true;
            //}
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            FillComboBox(tbSearch.Text);
        }

        private void lbEpisodes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!_src.Lookup)
            {
                string name = lbEpisodes.SelectedItem.ToString();
                int s = int.Parse(name.Substring(0, name.IndexOf('/')));
                int ep = int.Parse(name.Substring(name.IndexOf('/') + 1, name.IndexOf(':') - name.IndexOf('/') - 1));
                name = name.Substring(name.IndexOf(' ') + 1);
                string newFile = "";
                if (_eps.Where(i => i.Season == s && i.Episode == ep && i.Name == name).Count() == 1)
                {
                    newFile = string.Format(@"{0} - {1}{2} - {3}", cbSeries.Text, s.ToString(), (ep < 10) ? "0" + ep.ToString() : ep.ToString(), name);
                    MoveFile(newFile);
                }
            }
        }

        private void MoveFile(string newFile)
        {
            string newName = "";
            FileInfo oldFile = new FileInfo(_src.FullName);
            while (true)
            {
                Rename rn = new Rename(newFile);
                if (rn.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(oldFile.DirectoryName + "\\" + rn.NewFile + oldFile.Extension))
                    {
                        if (MessageBox.Show(string.Format("The file: {0} already exists! Try another name.", rn.NewFile), "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                            break;
                    }
                    else
                    {
                        newName = oldFile.DirectoryName + "\\" + rn.NewFile + oldFile.Extension;
                        break;
                    }
                }
            }
            if (File.Exists(oldFile.FullName) && newName != "")
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
                        if (count == 600)
                            break;
                    }
                }
            }
            this.Close();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            string name = lbEpisodes.SelectedItem.ToString();
            int s = int.Parse(name.Substring(0, name.IndexOf('/')));
            int ep = int.Parse(name.Substring(name.IndexOf('/') + 1, name.IndexOf(':') - name.IndexOf('/') - 1));
            name = name.Substring(name.IndexOf(' ') + 1);
            string newFile = "";
            if (_eps.Where(i => i.Season == s && i.Episode == ep && i.Name == name).Count() == 1)
            {
                newFile = string.Format(@"{0} - {1}{2} - {3}", cbSeries.Text, s.ToString(), (ep < 10) ? "0" + ep.ToString() : ep.ToString(), name);
                MoveFile(newFile);
            }
        }

        private void lbEpisodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = lbEpisodes.SelectedItem.ToString();
            int s = int.Parse(name.Substring(0, name.IndexOf('/')));
            int ep = int.Parse(name.Substring(name.IndexOf('/') + 1, name.IndexOf(':') - name.IndexOf('/') - 1));
            name = name.Substring(name.IndexOf(' ') + 1);
            var x = _eps.Where(i => i.Season == s && i.Episode == ep && i.Name == name);
            if (x.Count() == 1)
                tbEpisode.Text = x.Single().Description;
        }

        private void llIMDB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(string.Format("http://www.imdb.com/title/{0}/", llIMDB.Tag.ToString()));
        }
    }
}
