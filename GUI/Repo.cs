using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CydiaForWindows
{
    [Serializable]
    class Repo
    {
        public Dictionary<string, Package> packages;
        public Dictionary<string, string> data;
        public string url;
        public string name;
        public Repo(string url, ProgressBar RefreshProgress) {
            RefreshProgress.Value = 0;
            this.url = url;

            // Downloads Release file from url, reads it and deletes it
            string releasefile = Helper.ReadUrl(url, "Release");
            File.Delete("Release");

            RefreshProgress.Value += 5; // 5

            // different OSes uses different endings so this splits each line into a element of an array
            string[] split = releasefile.Split(new String[] { "\r\n" }, StringSplitOptions.None);
            if (split.Length < 2) split = releasefile.Split('\n');
            if (split.Length < 2) split = releasefile.Split('\r');

            RefreshProgress.Value += 5; // 10

            // Intilize a dictionary for repo data 
            this.data = new Dictionary<string, string>();
            // This will go through each line and place x of "x: y" in key and y in value
            foreach (string data in split) {
                string[] keyval = data.Split(new String[] { ": " }, StringSplitOptions.None);
                if (keyval.Length < 2) break;
                this.data.Add(keyval[0], keyval[1]);
            }
            RefreshProgress.Value += 5; // 15

            this.data.TryGetValue("Label", out this.name); // Grabs repo name and puts it in this.name
            string dir = this.name; // Repo directory
            if (!(dir == null)) {

                DownloadPackages(dir); //Puts packages file in *dir*/Packages

                RefreshProgress.Value += 20; // 35

                // Reads the Packages file stored in repo directory
                string content = Helper.ReadFromPath(dir + (dir.EndsWith("/") ? "Packages" : "/Packages"));

                RefreshProgress.Value += 5; // 40

                // different OSes uses different endings so this splits each line into a element of an array
                string[] contentarr = content.Split(new String[] { "\r\n\r\n" }, StringSplitOptions.None);
                if (contentarr.Length < 2) contentarr = content.Split(new String[] { "\n\n" }, StringSplitOptions.None);
                if (contentarr.Length < 2) contentarr = content.Split(new String[] { "\r\r" }, StringSplitOptions.None);
                RefreshProgress.Value += 5; // 30
                                            // Initilize a dictionary with format name: packagedata
                this.packages = new Dictionary<string, Package>();
                foreach (string package in contentarr) {
                    if ((RefreshProgress.Value + (60 / contentarr.Length)) > 100) RefreshProgress.Value = 100;
                    else RefreshProgress.Value += (60 / contentarr.Length); // 100
                                                                                // Initilize a Dictionary dataname: data
                    Dictionary<string, string> packagedatadict = new Dictionary<string, string>();

                    // different OSes uses different endings so this splits each line into a element of an array
                    string[] packagedata = package.Split(new String[] { "\r\n" }, StringSplitOptions.None);
                    if (packagedata.Length < 2) packagedata = package.Split('\r');
                    if (packagedata.Length < 2) packagedata = package.Split('\n');
                    // This will go through each line and place x of "x: y" in key and y in value
                    foreach (string data in packagedata) {
                        string[] keyval = data.Split(new String[] { ": " }, StringSplitOptions.None);
                        if (keyval.Length < 2) break;
                        packagedatadict.Add(keyval[0], keyval[1]);
                    }
                    string name;
                    if (packagedatadict.TryGetValue("Name", out name)) {
                        try {
                            this.packages.Add(name, new Package(packagedatadict, this.url));
                        } catch (ArgumentException e) {
                            Console.WriteLine("Warning: {0}", e.Message);
                        }
                    } else {
                        // TODO 
                        // Check version and overwrite if newer
                        Console.WriteLine("Could not grab name from package, skipping...");
                        continue;
                    }
                }
            } else {
                MessageBox.Show("Error, repo not found, make sure it's entered correctly");
            }
        }
        private void DownloadPackages(string dir) {
            try {
                string dire = dir + (dir.EndsWith("/") ? "Packages" : "/Packages");
                string urle = this.url + (this.url.EndsWith("/") ? "Packages.bz2" : "/Packages.bz2");
                Helper.DecompressUrl(urle, dire);
            } catch (Exception ex) {
                Console.WriteLine("Failed to download. Error " + ex.ToString());
            }
        }
        public override string ToString() {
            string name;
            this.data.TryGetValue("Name", out name);
            return name;
        }
    }
}

