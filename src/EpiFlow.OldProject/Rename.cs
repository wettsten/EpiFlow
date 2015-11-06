using System;
using System.Windows.Forms;
using System.IO;

namespace EpiFlow
{
    public partial class Rename : Form
    {
        private string _newFile;

        public Rename(string newName)
        {
            InitializeComponent();

            char[] bad = Path.GetInvalidFileNameChars();
            foreach (char c in bad)
            {
                while (newName.Contains(c.ToString()))
                {
                    newName = newName.Replace(c, '-');
                }
            }
            tbFileName.Text = newName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _newFile = tbFileName.Text;
            char[] bad = Path.GetInvalidPathChars();
            foreach (char c in bad)
            {
                if (tbFileName.Text.Contains(c.ToString()))
                {
                    MessageBox.Show("The filename contains invalid characters. You must remove them before saving the file.", "Invalid Character(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public string NewFile
        {
            get { return _newFile; }
        }

        private void tbFileName_TextChanged(object sender, EventArgs e)
        {
            char[] bad = Path.GetInvalidPathChars();
            foreach (char c in bad)
            {
                if (tbFileName.Text.Contains(c.ToString()))
                {
                    MessageBox.Show("The filename contains invalid characters. You must remove them before saving the file.", "Invalid Character(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}
