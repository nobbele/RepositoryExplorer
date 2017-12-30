using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace GUI
{
    public partial class Main : Form
    {
        Dictionary<string, Package> selected;
        List<Repo> repos;
        public Main() {
            InitializeComponent();
            repos = new List<Repo>();
            selected = new Dictionary<string, Package>();
            string[] repofiles = Directory.GetFiles(Environment.CurrentDirectory, "*.repo");

            foreach (string r in repofiles) {
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
                    } catch ( DirectoryNotFoundException ex ) {
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
                    continue;
                }
                string n = zf.Name;
                zf.IsStreamOwner = true;
                zf.Close();

                try {
                    string data = path + "/data.repodata";
                    Repo toadd = ObjectSaver.ReadFromXmlFile<Repo>(data);
                    repos.Add(toadd);
                } catch (FileNotFoundException ex) {
                    File.Delete(n);
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
            ReloadRepoBox();
        }

        private void Refresh_Click(object sender, EventArgs e) {
            RefreshProgress.SetProgressNoAnimation(0);
            AddRepo(EnterRepo.Text);
            RefreshProgress.SetProgressNoAnimation(100);
            RepoBox.Items.Clear();
            foreach (Repo r in repos) {
                Console.WriteLine(r.ToString());
                RepoBox.Items.Add(r);
            }
        }
        private void RefreshAll() {
            List<string> reponames = new List<string>();

            foreach(Repo r in repos) {
                reponames.Add(r.name);
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
        private void ViewRepo() {
            int index = RepoBox.SelectedIndex;
            if (index < 0) index = 0;
            Repo rep = repos[index];

            Reponame.Text = rep.name;
            Packages.Items.Clear();
            if (rep.packages != null) {
                foreach (Package pak in rep.packages.Values) {
                    Packages.Items.Add(pak);
                }
            }
        }
        private void AddRepo(string url) {
            if (!url.EndsWith("/")) url += "/";
            Repo r = new Repo(url, RefreshProgress);
            foreach(Repo rep in repos) {
                if(rep.name == r.name) {
                    return;
                }
            }
            if (r.packages != null && r.name != null) {
                repos.Add(r);
            }
        }

        private void Packages_ItemCheck_1(object sender, ItemCheckEventArgs e) {

            Package packtochange = (Package)Packages.Items[e.Index];
            if (e.CurrentValue == CheckState.Unchecked) selected.Add(packtochange.name, packtochange);
            else selected.Remove(packtochange.name);

            Package pak = repos[RepoBox.SelectedIndex].packages.Values.ToArray<Package>()[e.Index];
            name.Text = pak.name;
            packageid.Text = pak.package;
            section.Text = pak.section;
            md5.Text = pak.md5;
            size.Text = pak.size.ToString();
            description.Text = pak.description;
            depends.Text = pak.depends;
            URL.Text = pak.url;
            URL.LinkVisited = false;
        }
        protected override void OnClosed(EventArgs e) {
            foreach(Repo r in repos) {
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

                    Directory.Delete(r.name, true);
                }
            }
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
            ViewRepo();
        }

        private void button1_Click(object sender, EventArgs e) {
            RefreshAll();
        }
    }
}
