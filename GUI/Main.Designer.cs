namespace GUI
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.EnterRepo = new System.Windows.Forms.TextBox();
            this.Refresh = new System.Windows.Forms.Button();
            this.RefreshProgress = new System.Windows.Forms.ProgressBar();
            this.Packages = new System.Windows.Forms.CheckedListBox();
            this.name = new System.Windows.Forms.Label();
            this.packageid = new System.Windows.Forms.Label();
            this.section = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Info = new System.Windows.Forms.GroupBox();
            this.version = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.URL = new System.Windows.Forms.LinkLabel();
            this.label10 = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.Label();
            this.size = new System.Windows.Forms.Label();
            this.md5 = new System.Windows.Forms.Label();
            this.depends = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.RefreshBar = new System.Windows.Forms.ProgressBar();
            this.label11 = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.direc = new System.Windows.Forms.TextBox();
            this.defrep = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.RepoBox = new System.Windows.Forms.ListBox();
            this.RepInf = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.RepURL = new System.Windows.Forms.LinkLabel();
            this.RepNam = new System.Windows.Forms.Label();
            this.DepictionView = new System.Windows.Forms.WebBrowser();
            this.Depiction = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.host = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.Info.SuspendLayout();
            this.RepInf.SuspendLayout();
            this.Depiction.SuspendLayout();
            this.SuspendLayout();
            // 
            // EnterRepo
            // 
            this.EnterRepo.Location = new System.Drawing.Point(527, 630);
            this.EnterRepo.Name = "EnterRepo";
            this.EnterRepo.Size = new System.Drawing.Size(207, 20);
            this.EnterRepo.TabIndex = 1;
            this.EnterRepo.Text = "https://coolstar.org/publicrepo/";
            // 
            // Refresh
            // 
            this.Refresh.Location = new System.Drawing.Point(1062, 629);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(75, 20);
            this.Refresh.TabIndex = 2;
            this.Refresh.Text = "Add";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // RefreshProgress
            // 
            this.RefreshProgress.Location = new System.Drawing.Point(740, 629);
            this.RefreshProgress.MarqueeAnimationSpeed = 1;
            this.RefreshProgress.Name = "RefreshProgress";
            this.RefreshProgress.Size = new System.Drawing.Size(316, 20);
            this.RefreshProgress.TabIndex = 3;
            // 
            // Packages
            // 
            this.Packages.FormattingEnabled = true;
            this.Packages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Packages.Location = new System.Drawing.Point(389, 26);
            this.Packages.Name = "Packages";
            this.Packages.Size = new System.Drawing.Size(402, 544);
            this.Packages.TabIndex = 4;
            this.Packages.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Packages_ItemCheck_1);
            this.Packages.SelectedIndexChanged += new System.EventHandler(this.Packages_SelectedIndexChanged);
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Location = new System.Drawing.Point(50, 16);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(35, 13);
            this.name.TabIndex = 5;
            this.name.Text = "Name";
            // 
            // packageid
            // 
            this.packageid.AutoSize = true;
            this.packageid.Location = new System.Drawing.Point(30, 29);
            this.packageid.Name = "packageid";
            this.packageid.Size = new System.Drawing.Size(61, 13);
            this.packageid.TabIndex = 6;
            this.packageid.Text = "PackageID";
            // 
            // section
            // 
            this.section.AutoSize = true;
            this.section.Location = new System.Drawing.Point(58, 42);
            this.section.Name = "section";
            this.section.Size = new System.Drawing.Size(43, 13);
            this.section.TabIndex = 7;
            this.section.Text = "Section";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Section:";
            // 
            // Info
            // 
            this.Info.Controls.Add(this.version);
            this.Info.Controls.Add(this.label14);
            this.Info.Controls.Add(this.URL);
            this.Info.Controls.Add(this.label10);
            this.Info.Controls.Add(this.description);
            this.Info.Controls.Add(this.size);
            this.Info.Controls.Add(this.md5);
            this.Info.Controls.Add(this.depends);
            this.Info.Controls.Add(this.label7);
            this.Info.Controls.Add(this.label5);
            this.Info.Controls.Add(this.label6);
            this.Info.Controls.Add(this.label4);
            this.Info.Controls.Add(this.label1);
            this.Info.Controls.Add(this.section);
            this.Info.Controls.Add(this.label3);
            this.Info.Controls.Add(this.packageid);
            this.Info.Controls.Add(this.label2);
            this.Info.Controls.Add(this.name);
            this.Info.Location = new System.Drawing.Point(816, 26);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(352, 219);
            this.Info.TabIndex = 11;
            this.Info.TabStop = false;
            this.Info.Text = "Info";
            // 
            // version
            // 
            this.version.AutoSize = true;
            this.version.Location = new System.Drawing.Point(58, 107);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(42, 13);
            this.version.TabIndex = 21;
            this.version.Text = "Version";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 107);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "Version:";
            // 
            // URL
            // 
            this.URL.AutoSize = true;
            this.URL.Location = new System.Drawing.Point(42, 94);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(29, 13);
            this.URL.TabIndex = 19;
            this.URL.TabStop = true;
            this.URL.Text = "URL";
            this.URL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.URL_LinkClicked);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "URL:";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(75, 120);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(265, 96);
            this.description.TabIndex = 17;
            this.description.Text = "Description";
            // 
            // size
            // 
            this.size.AutoSize = true;
            this.size.Location = new System.Drawing.Point(42, 81);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(27, 13);
            this.size.TabIndex = 16;
            this.size.Text = "Size";
            // 
            // md5
            // 
            this.md5.AutoSize = true;
            this.md5.Location = new System.Drawing.Point(45, 68);
            this.md5.Name = "md5";
            this.md5.Size = new System.Drawing.Size(30, 13);
            this.md5.TabIndex = 15;
            this.md5.Text = "MD5";
            // 
            // depends
            // 
            this.depends.AutoSize = true;
            this.depends.Location = new System.Drawing.Point(65, 55);
            this.depends.Name = "depends";
            this.depends.Size = new System.Drawing.Size(50, 13);
            this.depends.TabIndex = 14;
            this.depends.Text = "Depends";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Description:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "MD5:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Depends:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(527, 614);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Repo";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(737, 613);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Repo load";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 629);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RefreshBar
            // 
            this.RefreshBar.Location = new System.Drawing.Point(93, 630);
            this.RefreshBar.MarqueeAnimationSpeed = 1;
            this.RefreshBar.Name = "RefreshBar";
            this.RefreshBar.Size = new System.Drawing.Size(428, 23);
            this.RefreshBar.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(93, 614);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Total Refresh";
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(437, 576);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(354, 20);
            this.search.TabIndex = 18;
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(390, 579);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Search";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1174, 126);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Download All Selected";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1305, 126);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(128, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "Download Current";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // direc
            // 
            this.direc.Location = new System.Drawing.Point(1174, 225);
            this.direc.Name = "direc";
            this.direc.Size = new System.Drawing.Size(178, 20);
            this.direc.TabIndex = 24;
            this.direc.Text = "debs";
            this.direc.TextChanged += new System.EventHandler(this.direc_TextChanged);
            // 
            // defrep
            // 
            this.defrep.Location = new System.Drawing.Point(1143, 629);
            this.defrep.Name = "defrep";
            this.defrep.Size = new System.Drawing.Size(112, 24);
            this.defrep.TabIndex = 25;
            this.defrep.Text = "Add Default Repo";
            this.defrep.UseVisualStyleBackColor = true;
            this.defrep.Click += new System.EventHandler(this.Defrep_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1358, 222);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 26;
            this.button4.Text = "Select folder";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(9, 45);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 27;
            this.button5.Text = "Delete Repo";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // RepoBox
            // 
            this.RepoBox.FormattingEnabled = true;
            this.RepoBox.Location = new System.Drawing.Point(12, 26);
            this.RepoBox.Name = "RepoBox";
            this.RepoBox.Size = new System.Drawing.Size(371, 576);
            this.RepoBox.TabIndex = 28;
            this.RepoBox.SelectedIndexChanged += new System.EventHandler(this.RepoBox_SelectedIndexChanged);
            // 
            // RepInf
            // 
            this.RepInf.Controls.Add(this.label16);
            this.RepInf.Controls.Add(this.label15);
            this.RepInf.Controls.Add(this.RepURL);
            this.RepInf.Controls.Add(this.RepNam);
            this.RepInf.Controls.Add(this.button5);
            this.RepInf.Location = new System.Drawing.Point(1174, 26);
            this.RepInf.Name = "RepInf";
            this.RepInf.Size = new System.Drawing.Size(259, 94);
            this.RepInf.TabIndex = 29;
            this.RepInf.TabStop = false;
            this.RepInf.Text = "Repo Info";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 13);
            this.label16.TabIndex = 31;
            this.label16.Text = "URL:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(38, 13);
            this.label15.TabIndex = 30;
            this.label15.Text = "Name:";
            // 
            // RepURL
            // 
            this.RepURL.AutoSize = true;
            this.RepURL.Location = new System.Drawing.Point(38, 29);
            this.RepURL.Name = "RepURL";
            this.RepURL.Size = new System.Drawing.Size(29, 13);
            this.RepURL.TabIndex = 29;
            this.RepURL.TabStop = true;
            this.RepURL.Text = "URL";
            this.RepURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RepURL_LinkClicked);
            // 
            // RepNam
            // 
            this.RepNam.AutoSize = true;
            this.RepNam.Location = new System.Drawing.Point(46, 16);
            this.RepNam.Name = "RepNam";
            this.RepNam.Size = new System.Drawing.Size(35, 13);
            this.RepNam.TabIndex = 28;
            this.RepNam.Text = "Name";
            // 
            // DepictionView
            // 
            this.DepictionView.AllowNavigation = false;
            this.DepictionView.AllowWebBrowserDrop = false;
            this.DepictionView.Location = new System.Drawing.Point(6, 19);
            this.DepictionView.MinimumSize = new System.Drawing.Size(20, 20);
            this.DepictionView.Name = "DepictionView";
            this.DepictionView.ScriptErrorsSuppressed = true;
            this.DepictionView.Size = new System.Drawing.Size(632, 342);
            this.DepictionView.TabIndex = 30;
            this.DepictionView.WebBrowserShortcutsEnabled = false;
            this.DepictionView.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.DepictionView_DocumentCompleted);
            // 
            // Depiction
            // 
            this.Depiction.Controls.Add(this.DepictionView);
            this.Depiction.Location = new System.Drawing.Point(816, 251);
            this.Depiction.Name = "Depiction";
            this.Depiction.Size = new System.Drawing.Size(644, 351);
            this.Depiction.TabIndex = 31;
            this.Depiction.TabStop = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1174, 155);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(125, 23);
            this.button6.TabIndex = 32;
            this.button6.Text = "Install All Selected";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1305, 155);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(128, 23);
            this.button7.TabIndex = 33;
            this.button7.Text = "Install Current";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // host
            // 
            this.host.Location = new System.Drawing.Point(1174, 199);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(141, 20);
            this.host.TabIndex = 34;
            this.host.TextChanged += new System.EventHandler(this.host_TextChanged);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(1321, 199);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(106, 20);
            this.password.TabIndex = 35;
            this.password.UseSystemPasswordChar = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1174, 183);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 36;
            this.label13.Text = "IP Address";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(1318, 183);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 13);
            this.label17.TabIndex = 37;
            this.label17.Text = "Password";
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(1433, 199);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(27, 20);
            this.port.TabIndex = 38;
            this.port.Text = "22";
            this.port.TextChanged += new System.EventHandler(this.port_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(1430, 183);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(26, 13);
            this.label18.TabIndex = 39;
            this.label18.Text = "Port";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1472, 660);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.port);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.password);
            this.Controls.Add(this.host);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.Depiction);
            this.Controls.Add(this.RepInf);
            this.Controls.Add(this.RepoBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.defrep);
            this.Controls.Add(this.direc);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.search);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.RefreshBar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Info);
            this.Controls.Add(this.Packages);
            this.Controls.Add(this.RefreshProgress);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.EnterRepo);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Name = "Main";
            this.Text = "RepositoryExplorer";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Info.ResumeLayout(false);
            this.Info.PerformLayout();
            this.RepInf.ResumeLayout(false);
            this.RepInf.PerformLayout();
            this.Depiction.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox EnterRepo;
        private System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.ProgressBar RefreshProgress;
        private System.Windows.Forms.CheckedListBox Packages;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Label packageid;
        private System.Windows.Forms.Label section;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox Info;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label description;
        private System.Windows.Forms.Label size;
        private System.Windows.Forms.Label md5;
        private System.Windows.Forms.Label depends;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel URL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar RefreshBar;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox search;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label version;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox direc;
        private System.Windows.Forms.Button defrep;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ListBox RepoBox;
        private System.Windows.Forms.GroupBox RepInf;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.LinkLabel RepURL;
        private System.Windows.Forms.Label RepNam;
        private System.Windows.Forms.WebBrowser DepictionView;
        private System.Windows.Forms.GroupBox Depiction;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox host;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label label18;
    }
}

