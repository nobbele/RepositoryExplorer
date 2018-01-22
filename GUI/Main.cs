using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Threading.Tasks;

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
            AddRepo(EnterRepo.Text);
            RefreshProgress.SetProgressNoAnimation(50);
            ReloadRepos();
            RefreshProgress.SetProgressNoAnimation(100);
            /*RepoBox.Items.Clear();
            foreach (Repo r in repos) {
                Console.WriteLine(r.ToString());
                RepoBox.Items.Add(r);
            }*/
        }
        private void RefreshAll() {
            List<string> reponames = new List<string>();

            foreach(Repo r in repos) {
                reponames.Add(r.url);
            }
            repos.Clear();
            RefreshBar.Value = 0;
            foreach(string name in reponames) {
                RefreshProgress.SetProgressNoAnimation(0);
                repos.Add(new Repo(name, RefreshProgress));
                RefreshProgress.SetProgressNoAnimation(100);

                int newval = RefreshBar.Value + (100 / reponames.Count);
                if (newval > 100) RefreshBar.SetProgressNoAnimation(100);
                else RefreshBar.SetProgressNoAnimation(newval);
            }
            RefreshBar.SetProgressNoAnimation(100);
        }
        private void ReloadRepoBox() {
            RepoBox.Items.Clear();
            foreach(Repo r in repos) {
                RepoBox.Items.Add(r);
            }
        }
        private void copyitems() {
            if (rep.packages != null) {
                CheckedListBox temp;
                boxes.TryGetValue(rep.name, out temp);

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
            rep = repos[index];

            Reponame.Text = rep.name;
            Packages.Items.Clear();
            copyitems();
        }
        private void AddRepo(string url, string srchdir = "") {
            if (!url.EndsWith("/")) url += "/";
            Repo r = new Repo(url, RefreshProgress, srchdir);
            foreach(Repo rep in repos) {
                if(rep.name == r.name) {
                    return;
                }
            }
            if (r.packages != null && r.name != null) {
                repos.Add(r);
            }
            if (r != null && r.name != null) {
                string path = Environment.CurrentDirectory + "/" + r.name + "/";
                string dataname = path + "data.repodata";
                ObjectSaver.WriteToXmlFile(dataname, r);

                //Fix for coolstar's weird description
                string orig = File.ReadAllText(dataname);
                File.Delete(dataname);
                File.WriteAllText(dataname, orig);


                string zipname = r.name + ".repo";
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
            /*if (e.CurrentValue == CheckState.Unchecked) {
                if (selected.ContainsKey(packtochange.name))
                    selected.Remove(packtochange.name);
                packtochange.selected = true;
                selected.Add(packtochange.name, packtochange);
            } else {
                selected.Remove(packtochange.name);
                packtochange.selected = true;
            }*/
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

        private void RepoBox_SelectedValueChanged(object sender, EventArgs e) {
            ViewRepo(RepoBox.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e) {
            RefreshAll();
        }
        public delegate void AddPackage();
        public AddPackage addpackage;
        public void AddPackageMethod(Package pak) {

        }
        private void search_TextChanged(object sender, EventArgs e) {
            if (rep != null) {
                Packages.Items.Clear();
                foreach (Package pak in rep.packages.Values) {
                    if (pak != null && pak.ToString().ToLower().Contains(search.Text.ToLower())) {
                        Packages.Items.Add(pak);
                    }
                }
            }
        }
        // Download all
        private void button2_Click(object sender, EventArgs e) {
            foreach (Repo re in repos) {
                Downloadprogress.SetProgressNoAnimation(0);
                if (re.sel.Count > 0) {
                    int toadd = 100 / re.sel.Count;
                    foreach (Package p in re.sel) {
                        if (p != null) {
                            Console.WriteLine(p);
                            p.download(direc.Text);
                            int newval = Downloadprogress.Value + toadd;
                            if (newval > 100) newval = 100;
                            Downloadprogress.SetProgressNoAnimation(newval);
                        }
                    }
                }
            }
            Downloadprogress.SetProgressNoAnimation(100);
        }

        private void button3_Click(object sender, EventArgs e) {
            Package p = Packages.SelectedItem as Package;
            if (p != null) {
                p.download(direc.Text);
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
                pak = r.packages.Values.ToArray<Package>()[Packages.SelectedIndex];
            } catch (IndexOutOfRangeException ex) {
                pak = r.packages.Values.Last<Package>();
            }
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
        private void Adddefault(string toadd) {
            string lnk = toadd + "/dists/stable/";
            string srchdir = "main/binary-iphoneos-arm/";
            try {
                AddRepo(lnk, srchdir);
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }

        private void Defrep_Click(object sender, EventArgs e) {
            DefaultRepos popup = new DefaultRepos();
            popup.ShowDialog();
            MessageBox.Show("This will take a very very long time");
            string url = "";
            switch(popup.chosen) {
                case "bigboss":
                    url = "http://apt.thebigboss.org/repofiles/cydia";
                    break;
                case "modmyi":
                    url = "http://apt.modmyi.com";
                    break;
                case "saurik":
                    string lnk = "http://apt.saurik.com/dists/ios/";
                    string srchdir = "main/binary-iphoneos-arm/";
                    AddRepo(lnk, srchdir);
                    url = "";
                    break;
                default:
                    url = "";
                    break;
            }
            if (url != "") {
                Adddefault(url);
            }
            ReloadRepoBox();
        }
        private void ReloadRepos() {
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
        }
        private void Main_Load(object sender, EventArgs e) {
            /*this.Show();
            this.Select();*/
            SplashScreen splash = new SplashScreen();
            splash.Show();
            splash.Select();

            ReloadRepos();

            splash.Hide();
            splash.Close();
            this.Show();
            this.Select();
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
    }
}
