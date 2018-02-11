using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GUI
{
    [Serializable]
    public class Repo
    {
        //[XmlIgnore]
        public List<Package> selected;
        //[XmlIgnore]
        public List<Package> sel;

        public int version;
        public SerializableDictionary<string, Package> packages;
        public SerializableDictionaryString data;
        public string url;
        public string name;
        public bool defaultsource;
        public string debloc;
        public string srchdir;
        public Repo() {
            
        }
        public Repo(string url, ProgressBar RefreshProgress, string srchdir = "", string debloc="") {
            this.version = Main.reversion;
            this.debloc = debloc;
            this.srchdir = srchdir;
            if (this.debloc == "") this.debloc = url;
            selected = new List<Package>();
            sel = new List<Package>();
            RefreshProgress.SetProgressNoAnimation(0);
            this.url = url;
            this.name = url.Replace("http://", "").Replace("https://", "");
            string dir = this.name;
            try {
                // Downloads Release file from url, reads it and deletes it
                string releasefile = Helper.ReadUrl(url, "Release");



                RefreshProgress.SetProgressNoAnimation(5); // 5

                // different OSes uses different endings so this splits each line into a element of an array
                string[] split = releasefile.Split(new String[] { "\r\n" }, StringSplitOptions.None);
                if (split.Length < 2) split = releasefile.Split('\n');
                if (split.Length < 2) split = releasefile.Split('\r');

                RefreshProgress.SetProgressNoAnimation(5); // 10

                // Intilize a dictionary for repo data 
                this.data = new SerializableDictionaryString();
                // This will go through each line and place x of "x: y" in key and y in value
                foreach (string data in split) {
                    string[] keyval = data.Split(new String[] { ": " }, StringSplitOptions.None);
                    if (keyval.Length < 2) break;
                    this.data.Add(keyval[0], keyval[1]);
                }
                RefreshProgress.SetProgressNoAnimation(5); // 15

                this.data.TryGetValue("Label", out this.name); // Grabs repo name and puts it in this.name
                this.name = this.name.Replace('/', '.');
                dir = this.name; // Repo directory
                if (dir == null) return;
                string to = dir + (dir.EndsWith("/") ? "Release" : "/Release");
                if (File.Exists(to)) {
                    File.Delete(to);
                }
                try {
                    File.Move("Release", to);
                } catch (DirectoryNotFoundException e) {
                    Directory.CreateDirectory(dir);
                    File.Move("Release", to);
                }
            } catch (System.Net.WebException) {
                MessageBox.Show("No Release file");
            }
            if (!(dir == null)) {
                Console.WriteLine("Downloading packages...");
                DownloadPackages(dir, srchdir); //Puts packages file in *dir*/Packages
                Console.WriteLine("Downloaded!");
                RefreshProgress.SetProgressNoAnimation(20); // 35
                Console.WriteLine("Reading packages...");
                // Reads the Packages file stored in repo directory
                string content = Helper.ReadFromPath(dir + (dir.EndsWith("/") ? "Packages" : "/Packages"));
                Console.WriteLine("Done!");
                RefreshProgress.SetProgressNoAnimation(5); // 40

                // different OSes uses different endings so this splits each line into a element of an array
                string[] contentarr = content.Split(new String[] { "\r\n\r\n" }, StringSplitOptions.None);
                if (contentarr.Length < 2) contentarr = content.Split(new String[] { "\n\n" }, StringSplitOptions.None);
                if (contentarr.Length < 2) contentarr = content.Split(new String[] { "\r\r" }, StringSplitOptions.None);
                RefreshProgress.SetProgressNoAnimation(5); // 30
                                            // Initilize a dictionary with format name: packagedata
                this.packages = new SerializableDictionary<string, Package>();
                foreach (string package in contentarr) {
                    if ((RefreshProgress.Value + (60 / contentarr.Length)) > 100) RefreshProgress.SetProgressNoAnimation(100);
                    else RefreshProgress.SetProgressNoAnimation((60 / contentarr.Length)); // 100
                                                                            // Initilize a Dictionary dataname: data
                    SerializableDictionaryString packagedatadict = new SerializableDictionaryString();

                    // different OSes uses different endings so this splits each line into a element of an array
                    string[] packagedata = package.Split(new String[] { "\r\n" }, StringSplitOptions.None);
                    if (packagedata.Length < 2) packagedata = package.Split('\r');
                    if (packagedata.Length < 2) packagedata = package.Split('\n');
                    // This will go through each line and place x of "x: y" in key and y in value
                    foreach (string data in packagedata) {
                        if (data != null && data != "") {
                            string[] keyval = data.Split(new String[] { ": " }, StringSplitOptions.None);
                            if (keyval.Length < 2) continue;
                            try {
                                packagedatadict.Add(keyval[0], keyval[1]);
                            } catch (System.ArgumentException e) {
                                packagedatadict.Remove(keyval[0]);
                                packagedatadict.Add(keyval[0], keyval[1]);
                            }
                        }
                    }
                    string name = "";
                    if (packagedatadict.TryGetValue("Name", out name)) {
                        name = name.Replace('/', '.');
                        try {
                            this.packages.Add(name, new Package(packagedatadict, this.url, this.debloc));
                        } catch (ArgumentException e) {
                            string ver = "";
                            packagedatadict.TryGetValue("Version", out ver);
                            Package p;
                            this.packages.TryGetValue(name, out p);

                            bool isnewer = Helper.Versioncheck(ver, p.version);
                            if (isnewer) {
                                this.packages.Remove(name);
                                this.packages.Add(name, new Package(packagedatadict, this.url, this.debloc));
                            }
                        }
                    } else {
                        // TODO 
                        // Check version and overwrite if newer
                        string pak = "";
                        packagedatadict.TryGetValue("Package", out pak);
                        Console.WriteLine("Could not grab name from {0}, skipping...", pak);

                        continue;
                    }
                }
            } else {
                MessageBox.Show("Error, repo not found, make sure it's entered correctly");
            }
        }
        private void DownloadPackages(string dir, string srchdir = "") {
            string dire = dir + (dir.EndsWith("/") ? "Packages" : "/Packages");
            if (!File.Exists(dire)) {
                // Add fall back to Packages.gz if bz2 doesn't work
                // Makes repo like beta.unlimapps.com and repo.lockhtml.com work
                string urlto = this.url + (this.url.EndsWith("/") ? srchdir : "/" + srchdir + (srchdir.EndsWith("/") ? "" : "/"));
                string urle = urlto + "Packages.bz2";
                try {
                    Helper.DecompressUrl(urle, dire);
                    File.Delete(dir + (dir.EndsWith("/") ? "Packages.bz2" : "/Packages.bz2"));
                } catch (FileNotFoundException) {
                    urle = urlto + "Packages.gz";
                    Helper.DecompressUrl(urle, dire, "gz");
                    File.Delete(dir + (dir.EndsWith("/") ? "Packages.gz" : "/Packages.gz"));
                }   
            }
        }
        public override string ToString() {
            return this.name + "   " + this.packages.Count + " packages";
        }
    }
}

