using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Net;

namespace GUI
{
    public partial class Main : Form
    {
        public static int reversion = 1;
        Dictionary<string, Package> selected;
        List<Package> toinstall;
        List<Repo> repos;
        Repo rep;
        Dictionary<string, CheckedListBox> boxes;
        string _ip;
        bool _electra;
        string _port;
        string _direc = "debs";
        public Main() {
            InitializeComponent();
            toinstall = new List<Package>();
        }

        private void Refresh_Click(object sender, EventArgs e) {
            RefreshProgress.SetProgressNoAnimation(0);
            try {
                AddRepo(EnterRepo.Text);
            } catch (System.Net.WebException ex) {
                RefreshProgress.SetProgressNoAnimation(100);
                MessageBox.Show("Make sure you entered a valid repo, must start with http:// or https://, Error " + ex);
                return;
            }
            RefreshProgress.SetProgressNoAnimation(50);
            ReloadRepos();
            RefreshProgress.SetProgressNoAnimation(100);
        }
        private void RefreshAll() {
            List<Repo> reponames = new List<Repo>();

            foreach (Repo r in repos) {
                reponames.Add(r);
            }
            repos.Clear();
            foreach (Repo r in reponames) {
                RefreshProgress.SetProgressNoAnimation(0);

                repos.Add(new Repo(r.url, RefreshProgress, r.srchdir, r.debloc));
                RefreshProgress.SetProgressNoAnimation(100);
            }
        }
        private void ReloadRepoBox() {
            RepoBox.BeginUpdate();
            RepoBox.Items.Clear();
            foreach (Repo r in repos) {
                RepoBox.Items.Add(r);
            }
            RepoBox.EndUpdate();
        }
        private void copyitems() {
            if (rep.packages != null) {
                boxes.TryGetValue(rep.name, out CheckedListBox temp);
                if (!(temp == null)) {
                    foreach (Package p in temp.Items) {
                        if (p != null && p.ToString().ToLower().Contains(search.Text.ToLower())) {
                            int i = Packages.Items.Add(p);
                            if (p.selected) {
                                Packages.SetItemChecked(i, true);
                            }
                        }
                    }
                } else {
                    MessageBox.Show("Could not get value rep.name from boxes");
                    foreach (CheckedListBox t in boxes.Values) {
                        MessageBox.Show(t.ToString());
                    }
                }
            }
        }
        private void ViewRepo(int ik) {
            int index = ik;
            if (index < 0) index = 0;
            if (index >= repos.Count) index = repos.Count;
            try {
                rep = repos[index];

                RepNam.Text = rep.name;
                RepURL.Text = rep.url;

                Packages.BeginUpdate();
                Packages.Items.Clear();
                copyitems();
                Packages.EndUpdate();
            } catch (System.NullReferenceException e) {
                MessageBox.Show(e.Message, (rep == null).ToString() + (rep != null ? rep.name : ""));
                Application.Exit();
            }
        }
        private void AddRepo(string url, string srchdir = "", bool defaultrep=false, string decloc="") {
            if (!url.EndsWith("/")) url += "/";
            Repo r = new Repo(url, RefreshProgress, srchdir, decloc);
            foreach (Repo rep in repos) {
                if (rep.name == r.name) {
                    return;
                }
            }
            if (r.packages != null && r.name != null) {
                r.defaultsource = defaultrep;
                repos.Add(r);
            }
            if (r != null && r.name != null) {
                string path = Environment.CurrentDirectory + "/" + r.name + "/";
                string dataname = path + "data.repodata";
                ObjectSaver.WriteToXmlFile(dataname, r);

                //Fix for coolstar's weird description
                string orig = File.ReadAllText(dataname);
                Helper.RemoveSpecialCharacters(orig);
                File.Delete(dataname);
                File.WriteAllText(dataname, orig);


                string zipname = r.name.Replace('/', '.') + ".repo";
                ZipOutputStream zip = new ZipOutputStream(File.Create(zipname));

                string folderName = r.name;

                int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

                Helper.CompressFolder(folderName, zip, folderOffset);

                zip.IsStreamOwner = true;
                zip.Close();
            }
        }

        private void Packages_ItemCheck_1(object sender, ItemCheckEventArgs e) {

            Package packtochange = (Package)Packages.Items[e.Index];
            if (!rep.sel.Contains(packtochange)) {
                rep.sel.Add(packtochange);
                packtochange.selected = true;
            } else {
                if (e.CurrentValue == CheckState.Checked) {
                    packtochange.selected = false;
                    rep.sel.Remove(packtochange);
                }
            }
            if (packtochange.selected) toinstall.Add(packtochange);
            else toinstall.Remove(packtochange);

            int selind = RepoBox.SelectedIndex;
            if (selind < 0) selind = 0;
            Repo r = repos[selind];
            Package pak = r.packages.Values.ToArray<Package>()[e.Index];
            name.Text = pak.name;
            packageid.Text = pak.package;
            section.Text = pak.section;
            md5.Text = pak.md5;
            size.Text = pak.size.ToString();
            description.Text = pak.description;
            depends.Text = pak.depends;
            URL.Text = pak.url;
            version.Text = pak.version;
            URL.LinkVisited = false;
        }

        private void URL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            LinkLabel l = sender as LinkLabel;
            if (l.Text != "URL") {
                try {
                    Console.WriteLine("Opening link " + l.Text);
                    System.Diagnostics.Process.Start(l.Text);
                    l.LinkVisited = true;
                } catch (System.ComponentModel.Win32Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            RefreshAll();
        }
        private void Search(Object myObject, EventArgs myEventArgs) {
            if (rep != null) {
                Packages.BeginUpdate(); 
                Packages.Items.Clear();
                foreach (Package pak in rep.packages.Values) {
                    if (pak != null && pak.ToString().ToLower().Contains(search.Text.ToLower())) {
                        int i = Packages.Items.Add(pak);
                        if (pak.selected) {
                            Packages.SetItemChecked(i, true);
                        }
                    }
                }
                Packages.EndUpdate();
            }
            searchdelay.Stop();
        }

        private Timer searchdelay;
        private void search_TextChanged(object sender, EventArgs e) {
            if (searchdelay != null) searchdelay.Stop();
            searchdelay = new Timer();
            searchdelay.Interval = 500;

            searchdelay.Tick += Search;

            searchdelay.Start();
        }
        // Download all
        private void button2_Click(object sender, EventArgs e) {
            foreach (Repo re in repos) {
                if (re.sel.Count > 0) {
                    int toadd = 100 / re.sel.Count;
                    foreach (Package p in re.sel) {
                        if (p != null) {
                            Console.WriteLine(p);
                            p.download(_direc);
                        }
                    }
                }
            }
            MessageBox.Show("Done!");
        }

        private void button3_Click(object sender, EventArgs e) {
            Package p = Packages.SelectedItem as Package;
            if (p != null) {
                p.download(_direc);
                MessageBox.Show("Done!");
            } else {
                MessageBox.Show("No package selected (you must double click on a package to select it");
            }
        }

        private void Packages_SelectedIndexChanged(object sender, EventArgs e) {
            //rep.selected.Add(Packages.SelectedItem as Package);
            int selind = RepoBox.SelectedIndex;
            if (selind < 0) selind = 0;
            Repo r = repos[selind];
            Package pak;
            try {
                
                pak = Packages.SelectedItem as Package;
                //pak = r.packages.TryGetValue(null, out null);
            } catch (IndexOutOfRangeException ex) {
                pak = r.packages.Values.Last<Package>();
            }
            if (pak == null) return;
            if (pak.depiction != null) {
                Uri loc = new Uri(pak.depiction);
                DepictionView.AllowNavigation = true;
                DepictionView.Navigate(loc);
            }
            name.Text = pak.name;
            packageid.Text = pak.package;
            section.Text = pak.section;
            md5.Text = pak.md5;
            size.Text = pak.size.ToString();
            description.Text = pak.description;
            depends.Text = pak.depends;
            URL.Text = (r.defaultsource ? pak.debloc + "/" + pak.filename : pak.url);
            version.Text = pak.version;
            price.Text = CydiaAPI.getprice(pak.package);
            button2.Enabled = price.Text == "Free";
            button3.Enabled = price.Text == "Free";
            button6.Enabled = price.Text == "Free";
            button7.Enabled = price.Text == "Free";
            URL.LinkVisited = false;
        }
        private void Adddefault(string toadd, string decloc, string srchdir, string dist) {
            string lnk = toadd + dist;
            try {
                AddRepo(lnk, srchdir, true, decloc);
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }
        private void Defrep_Click(object sender, EventArgs e) {
            RefreshProgress.SetProgressNoAnimation(0);
            DefaultRepos popup = new DefaultRepos();
            try {
                popup.ShowDialog();
                if (!popup.chosen.MultiOrEquals("", null)) {
                    MessageBox.Show("This will take a very very long time");
                    string url = "";
                    string decloc = "";
                    string srchdir = "main/binary-iphoneos-arm/";
                    string dist = "/dists/stable/";
                    switch (popup.chosen) {
                        case "bigboss":
                            url = "http://apt.thebigboss.org/repofiles/cydia";
                            decloc = url;
                            break;
                        case "modmyi":
                            url = "http://apt.modmyi.com";
                            decloc = url;
                            break;
                        case "saurik":
                            url = "http://apt.saurik.com/dists/ios";
                            srchdir = "/main/binary-iphoneos-arm/";
                            dist = "";
                            decloc = "http://apt.saurik.com/";
                            break;
                        default:
                            url = "";
                            break;
                    }
                    if (url != "") {
                        Adddefault(url, decloc, srchdir, dist);
                    }
                    ReloadRepos();
                    RefreshProgress.SetProgressNoAnimation(100);
                }
            } catch (System.ObjectDisposedException ex) {
                MessageBox.Show("{0} disposed at Defrep_click()", ex.ObjectName);
            }
        }
        private void ReloadRepos() {
            SplashScreen splash = new SplashScreen();
            splash.Show();
            splash.Select();

            RepoBox.ClearSelected();
            Packages.ClearSelected();

            rep = new Repo();
            boxes = new Dictionary<string, CheckedListBox>();
            repos = new List<Repo>();

            selected = new Dictionary<string, Package>();
            string[] repofiles = Directory.GetFiles(Environment.CurrentDirectory, "*.repo");
            Parallel.ForEach(repofiles, (r) => {
                bool cont = false;
                FileStream fs = File.OpenRead(r);
                ZipFile zf = new ZipFile(fs);

                string path = zf.Name.Replace(".repo", "");

                foreach (ZipEntry zipEntry in zf) {
                    try {
                        string name = zipEntry.Name;

                        byte[] buffer = new byte[4096];
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        String fullZipToPath = Path.Combine(path, name);

                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        using (FileStream streamWriter = File.Create(fullZipToPath)) {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    } catch (DirectoryNotFoundException ex) {
                        Console.WriteLine(ex.Message);
                        cont = true;
                        break;
                    } catch (FileNotFoundException ex) {
                        Console.WriteLine(ex.Message);
                        cont = true;
                        break;
                    }
                }
                if (cont) {
                    return;
                }
                string n = zf.Name;
                zf.IsStreamOwner = true;
                zf.Close();

                try {
                    string data = path + "/data.repodata";
                    Repo toadd = ObjectSaver.ReadFromXmlFile<Repo>(data);
                    if (toadd.version < reversion) {
                        toadd = new Repo(toadd.url, new ProgressBar(), toadd.srchdir, toadd.debloc);
                    } else if(toadd.version > reversion) {
                        
                        DialogResult answer = MessageBox.Show("Colliding versions", toadd.name + " has been installed using a higher version of RepositoryExplorer. Do you want to update the repo to the new version? Answering no might lead to unexpected behaviour", MessageBoxButtons.YesNo);
                        if (answer == DialogResult.Yes) {
                            toadd = new Repo(toadd.url, new ProgressBar(), toadd.srchdir, toadd.debloc);
                        }
                    }
                    toadd.selected = new List<Package>();
                    toadd.sel = new List<Package>();
                    repos.Add(toadd);

                    CheckedListBox b = new CheckedListBox();
                    foreach (Package pak in toadd.packages.Values) {
                        b.Items.Add(pak);
                    }
                    boxes.Add(toadd.name, b);
                } catch (FileNotFoundException ex) {
                    File.Delete(n);
                    Console.WriteLine(ex.Message);
                    return;
                }
                Directory.Delete(path, true);
            });
            ReloadRepoBox();

            splash.Hide();
            splash.Close();
            this.Show();
            this.Select();
        }
        private void SetReg() {
            int BrowserVer, RegVal;

            // get the installed IE version
            using (WebBrowser Wb = new WebBrowser())
                BrowserVer = Wb.Version.Major;

            // set the appropriate IE version
            if (BrowserVer >= 11)
                RegVal = 11001;
            else if (BrowserVer == 10)
                RegVal = 10001;
            else if (BrowserVer == 9)
                RegVal = 9999;
            else if (BrowserVer == 8)
                RegVal = 8888;
            else
                RegVal = 7000;

            // set the actual key
            using (RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
                if (Key.GetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe") == null)
                    Key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);
        }
        private void Main_Load(object sender, EventArgs e) {
            SetReg();

            ReloadRepos();

            string[] data = Installer.getsettings();
            _ip = data[0];
            _port = data[1];
        }

        private void button4_Click(object sender, EventArgs e) {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.ShowDialog();
            _direc = f.SelectedPath;
        }

        private void button5_Click(object sender, EventArgs e) {
            if (rep != null) {
                File.Delete(rep.name + ".repo");
                ReloadRepos();
            }
        }

        private void RepoBox_SelectedIndexChanged(object sender, EventArgs e) {
            ViewRepo(RepoBox.SelectedIndex);
        }

        private void RepURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            LinkLabel l = sender as LinkLabel;
            Console.WriteLine(l.Text);
            if (l.Text != "URL") {
                try {
                    Console.WriteLine("Opening link " + l.Text);
                    System.Diagnostics.Process.Start(l.Text);
                    l.LinkVisited = true;
                } catch (System.ComponentModel.Win32Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void DepictionView_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            DepictionView.AllowNavigation = false;
        }
        private string inputpopup(string question, bool ispassword = false) {
            InputPopup popup = new InputPopup(question, ispassword);
            popup.ShowDialog();
            string answer = popup.answer.Text;
            Console.WriteLine(answer);
            return answer;
        }     
        //Install all checked in packages
        private void button6_Click(object sender, EventArgs e) {
            string _password = inputpopup("Root password?", true);
            List<FileInfo> debs = new List<FileInfo>();
            foreach (Repo re in repos) {
                if (re.sel.Count > 0) {
                    foreach (Package p in re.sel) {
                        if (p != null) {
                            Console.WriteLine(p);
                            string path = _direc + "/" + p.getdebname();
                            if (!File.Exists(path)) {
                                string url = (re.defaultsource ? p.debloc + "/" + p.filename : p.url);
                                p.download(_direc);
                            }
                            //FileInfo deb = new FileInfo(path);
                            debs.Add(new FileInfo(path));
                            
                        }
                    }
                }
            }
            if (_electra) Installer.installelectra(debs, _ip, _port, _password);
            else {
                foreach (FileInfo deb in debs) {
                    Installer.installnormal(deb, _ip, _port, _password);
                }
            }
            MessageBox.Show("Done!");
        }

        private void button7_Click(object sender, EventArgs e) {
            string _password = inputpopup("Root password?", true);
            Package p;
            try {
                p = Packages.SelectedItem as Package;
                string path = _direc + "/" + p.getdebname();
                if (!File.Exists(path)) {
                    string url = ((RepoBox.SelectedItem as Repo).defaultsource ? p.debloc + "/" + p.filename : p.url);
                    p.download(_direc);
                }
                if (_electra) Installer.installelectrasingle(path, _ip, _port, _password);
                else {
                    Installer.installnormal(new FileInfo(path), _ip, _port, _password);
                }
            } catch(System.NullReferenceException) {
                MessageBox.Show("Please select a package");
                return;
            }
        }

        private void settings_Click(object sender, EventArgs e) {
            SettingsPopup popup = new SettingsPopup(_ip, _port, _electra, _direc);
            popup.ShowDialog();
            _ip = popup.ip;
            _port = popup.port;
            _direc = popup.debloc;
            _electra = popup.electrabool;
        }

        private void direc_TextChanged(object sender, EventArgs e) {

        }

        private void RefreshBar_Click(object sender, EventArgs e) {

        }

        private void Depiction_Enter(object sender, EventArgs e) {

        }

        private void label11_Click(object sender, EventArgs e) {

        }

        private void label7_Click(object sender, EventArgs e) {

        }

        private void description_Click(object sender, EventArgs e) {

        }
    }
    public static class ExtensionMethods
    {
        public static void SetProgressNoAnimation(this ProgressBar pb, int value) {
            // To get around the progressive animation, we need to move the 
            // progress bar backwards.
            if (value == pb.Maximum) {
                // Special case as value can't be set greater than Maximum.
                pb.Maximum = value + 1;     // Temporarily Increase Maximum
                pb.Value = value + 1;       // Move past
                pb.Maximum = value;         // Reset maximum
            } else {
                pb.Value = value + 1;       // Move past
            }
            pb.Value = value;               // Move to correct value
        }
        public static bool MultiAndEquals(this object me, params object[] args) {
            for (int i = 0; i < args.Length; i++) {
                if (me != args[i]) {
                    return false;
                }
            }
            return true;
        }
        public static bool MultiOrEquals(this object me, params object[] args) {
            for (int i = 0; i < args.Length; i++) {
                if (me == args[i]) {
                    return true;
                }
            }
            return false;
        }
    }
}
