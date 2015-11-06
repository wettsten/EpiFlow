namespace EpiFlow
{
    partial class NewFile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewFile));
            this.cbSeries = new System.Windows.Forms.ComboBox();
            this.tbSeries = new System.Windows.Forms.TextBox();
            this.lbEpisodes = new System.Windows.Forms.ListBox();
            this.lbSearch = new System.Windows.Forms.Label();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.tbEpisode = new System.Windows.Forms.TextBox();
            this.llIMDB = new System.Windows.Forms.LinkLabel();
            this.lbOldFile = new System.Windows.Forms.Label();
            this.tVShowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tVShowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cbSeries
            // 
            this.cbSeries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeries.FormattingEnabled = true;
            this.cbSeries.Location = new System.Drawing.Point(12, 53);
            this.cbSeries.Name = "cbSeries";
            this.cbSeries.Size = new System.Drawing.Size(572, 21);
            this.cbSeries.TabIndex = 1;
            this.cbSeries.SelectedIndexChanged += new System.EventHandler(this.cbSeries_SelectedIndexChanged);
            // 
            // tbSeries
            // 
            this.tbSeries.Location = new System.Drawing.Point(12, 97);
            this.tbSeries.Multiline = true;
            this.tbSeries.Name = "tbSeries";
            this.tbSeries.ReadOnly = true;
            this.tbSeries.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSeries.Size = new System.Drawing.Size(258, 113);
            this.tbSeries.TabIndex = 2;
            // 
            // lbEpisodes
            // 
            this.lbEpisodes.FormattingEnabled = true;
            this.lbEpisodes.Location = new System.Drawing.Point(278, 83);
            this.lbEpisodes.Name = "lbEpisodes";
            this.lbEpisodes.Size = new System.Drawing.Size(306, 212);
            this.lbEpisodes.TabIndex = 3;
            this.lbEpisodes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbEpisodes_MouseDoubleClick);
            this.lbEpisodes.SelectedIndexChanged += new System.EventHandler(this.lbEpisodes_SelectedIndexChanged);
            // 
            // lbSearch
            // 
            this.lbSearch.AutoSize = true;
            this.lbSearch.Location = new System.Drawing.Point(9, 29);
            this.lbSearch.Name = "lbSearch";
            this.lbSearch.Size = new System.Drawing.Size(62, 13);
            this.lbSearch.TabIndex = 4;
            this.lbSearch.Text = "Search for: ";
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(278, 26);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(225, 20);
            this.tbSearch.TabIndex = 5;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(509, 24);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Search";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnMove
            // 
            this.btnMove.Enabled = false;
            this.btnMove.Location = new System.Drawing.Point(278, 309);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(306, 23);
            this.btnMove.TabIndex = 7;
            this.btnMove.Text = "THAT\'S IT!";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // tbEpisode
            // 
            this.tbEpisode.Location = new System.Drawing.Point(12, 216);
            this.tbEpisode.Multiline = true;
            this.tbEpisode.Name = "tbEpisode";
            this.tbEpisode.ReadOnly = true;
            this.tbEpisode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEpisode.Size = new System.Drawing.Size(258, 116);
            this.tbEpisode.TabIndex = 8;
            // 
            // llIMDB
            // 
            this.llIMDB.AutoSize = true;
            this.llIMDB.Enabled = false;
            this.llIMDB.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llIMDB.Location = new System.Drawing.Point(13, 81);
            this.llIMDB.Name = "llIMDB";
            this.llIMDB.Size = new System.Drawing.Size(34, 13);
            this.llIMDB.TabIndex = 9;
            this.llIMDB.TabStop = true;
            this.llIMDB.Text = "IMDB";
            this.llIMDB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llIMDB_LinkClicked);
            // 
            // lbOldFile
            // 
            this.lbOldFile.AutoSize = true;
            this.lbOldFile.Location = new System.Drawing.Point(9, 9);
            this.lbOldFile.Name = "lbOldFile";
            this.lbOldFile.Size = new System.Drawing.Size(0, 13);
            this.lbOldFile.TabIndex = 10;
            // 
            // tVShowBindingSource
            // 
            this.tVShowBindingSource.DataSource = typeof(EpiFlow.TVSeries);
            // 
            // NewFile
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 344);
            this.Controls.Add(this.lbOldFile);
            this.Controls.Add(this.llIMDB);
            this.Controls.Add(this.tbEpisode);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.lbSearch);
            this.Controls.Add(this.lbEpisodes);
            this.Controls.Add(this.tbSeries);
            this.Controls.Add(this.cbSeries);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NewFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TV Tray";
            ((System.ComponentModel.ISupportInitialize)(this.tVShowBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSeries;
        private System.Windows.Forms.TextBox tbSeries;
        private System.Windows.Forms.ListBox lbEpisodes;
        private System.Windows.Forms.Label lbSearch;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.TextBox tbEpisode;
        private System.Windows.Forms.LinkLabel llIMDB;
        private System.Windows.Forms.Label lbOldFile;
        private System.Windows.Forms.BindingSource tVShowBindingSource;
    }
}

