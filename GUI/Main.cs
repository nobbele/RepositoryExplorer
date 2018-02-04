using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using Renci.SshNet;

namespace GUI
{
    public partial class Main : Form
    {
        Dictionary<string, Package> selected;
        List<Package> toinstall;
        List<Repo> repos;
        Repo rep;
        Dictionary<string, CheckedListBox> boxes;
        public Main() {
            InitializeComponent();
            toinstall = new List<Package>();
        }

        private void Refresh_Click(object sender, EventArgs e) {
            RefreshProgress.SetProgressNoAnimation(0);
            try {
                AddRepo(EnterRepo.Text);
            } catch (System.Net.WebException) {
                RefreshProgress.SetProgressNoAnimation(100);
                MessageBox.Show("Make sure you entered a valid repo, must start with http:// or https://");
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
            RefreshBar.Value = 0;
            foreach (Repo r in reponames) {
                RefreshProgress.SetProgressNoAnimation(0);

                repos.Add(new Repo(r.url, RefreshProgress, r.srchdir, r.debloc));

                RefreshProgress.SetProgressNoAnimation(100);
                int newval = RefreshBar.Value + (100 / reponames.Count);
                if (newval > 100) RefreshBar.SetProgressNoAnimation(100);
                else RefreshBar.SetProgressNoAnimation(newval);
            }
            RefreshBar.SetProgressNoAnimation(100);
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

                if (!(temp == null)) Packages.Items.AddRange(temp.Items);
                else {
                    MessageBox.Show("Could not get value rep.name from boxes");
                    foreach (CheckedListBox t in boxes.Values) {
                        MessageBox.Show(t.ToString());
                    }
                }

                copyitemspart2();
            }
        }
        private void copyitemspart2() {
            for (int i = 0; i < Packages.Items.Count; i++) {
                Package pak = (Package)Packages.Items[i];
                if (pak.selected) {
                    int ind = Packages.Items.IndexOf(pak);
                    Packages.SetItemChecked(ind, true);
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
                            p.download(direc.Text);
                        }
                    }
                }
            }
            MessageBox.Show("Done!");
        }

        private void button3_Click(object sender, EventArgs e) {
            Package p = Packages.SelectedItem as Package;
            if (p != null) {
                p.download(direc.Text);
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
            } catch (System.ObjectDisposedException) {
                
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
                        Console.WriteLine(fullZipToPath);

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

            if (!File.Exists("tics/settings")) {
                string[] def = new string[3];
                File.WriteAllLines("tics/settings", def);
            }
            string[] data = File.ReadAllLines("tics/settings"); //get ssh settings
            host.Text = data[0];
            port.Text = data[1];
        }

        private void button4_Click(object sender, EventArgs e) {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.ShowDialog();
            direc.Text = f.SelectedPath;
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
        string join(List<FileInfo> s, string i) {
            string temp = "";
            foreach (FileInfo j in s) {
                temp += '"' + j.FullName + '"' + i;
            }
            return temp;
        }
        //Credits to u/josephwalden for creating the tic.exe program
        private void installelectra(List<FileInfo> debs) {
            string[] data = { host.Text, port.Text, password.Text };
            File.WriteAllLines("tics/settings", data);
            Process.Start("tics/tic.exe", "dont-update " + "install " + join(debs, " "));
            File.Delete("tics/settings");
            string[] safedata = { host.Text, port.Text, "" };
            File.WriteAllLines("tics/settings", safedata);
        }
        private void installelectrasingle(FileInfo deb) {
            string[] data = { host.Text, port.Text, password.Text };
            File.WriteAllLines("tics/settings", data);
            Process.Start("tics/tic.exe", "dont-update " + "install " + deb);
            File.Delete("tics/settings");
            string[] safedata = { host.Text, port.Text, "" };
            File.WriteAllLines("tics/settings", safedata);
        }
        private void installnormal(FileInfo deb) {
            int p = 21;
            int.TryParse(port.Text, out p);

            string path = "/tmp/" + deb.Name;
            using (var client = new SftpClient(host.Text, p, "root", password.Text)) {
                client.Connect();

                FileStream fileStream = new FileStream(deb.FullName, FileMode.Open);
                if (client.IsConnected) {
                    if (fileStream != null) {
                        client.UploadFile(fileStream, path, null);
                        client.Disconnect();
                        client.Dispose();
                    }
                }
            }
            
            using (var client = new SshClient(host.Text, p, "root", password.Text)) {
                client.Connect();
                
                var r = client.RunCommand("dpkg -i " + path);
                Console.WriteLine(r.Result);
                client.RunCommand("rm " + path);
                client.Disconnect();
            }
        }
        //Install all checked in packages
        private void button6_Click(object sender, EventArgs e) {
            DialogResult t =  MessageBox.Show("Are you using the Electra jailbreak?", "Electra?", MessageBoxButtons.YesNoCancel);
            bool electra = (t == DialogResult.Yes);
            List<FileInfo> debs = new List<FileInfo>();
            foreach (Repo re in repos) {
                if (re.sel.Count > 0) {
                    foreach (Package p in re.sel) {
                        if (p != null) {
                            Console.WriteLine(p);
                            string path = direc.Text + "/" + p.getdebname();
                            if (!File.Exists(path)) {
                                string url = (re.defaultsource ? p.debloc + "/" + p.filename : p.url);
                                p.download(direc.Text);
                            }
                            //FileInfo deb = new FileInfo(path);
                            debs.Add(new FileInfo(path));
                            
                        }
                    }
                }
            }
            if (electra) installelectra(debs);
            else {
                foreach (FileInfo deb in debs) {
                    installnormal(deb);
                }
            }
            MessageBox.Show("Done!");
        }

        private void button7_Click(object sender, EventArgs e) {
            Package p = Packages.SelectedItem as Package;
            DialogResult t = MessageBox.Show("Are you using the Electra jailbreak?", "Electra?", MessageBoxButtons.YesNoCancel);
            bool electra = (t == DialogResult.Yes);
            string path = direc.Text + "/" + p.getdebname();
            if (!File.Exists(path)) {
                string url = ((RepoBox.SelectedItem as Repo).defaultsource ? p.debloc + "/" + p.filename : p.url);
                p.download(direc.Text);
            }
            if (electra) installelectrasingle(new FileInfo(path));
            else {
                installnormal(new FileInfo(path));
            }
        }

        private void direc_TextChanged(object sender, EventArgs e) {

        }

        private void label13_Click(object sender, EventArgs e) {

        }

        private void host_TextChanged(object sender, EventArgs e) {
            string[] safedata = { host.Text, port.Text, "" };
            File.WriteAllLines("tics/settings", safedata);
        }

        private void port_TextChanged(object sender, EventArgs e) {
            string[] safedata = { host.Text, port.Text, "" };
            File.WriteAllLines("tics/settings", safedata);
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
