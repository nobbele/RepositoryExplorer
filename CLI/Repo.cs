using System;
using System.Collections.Generic;

namespace CLI
{
    class Repo
    {
        public Dictionary<string, Package> packages;
        public Dictionary<string, string> data;
        public string url;
        public string name;
        public Repo(string url) {
            this.url = url;

            // Downloads Release file from url, reads it and deletes it
            string releasefile = Helper.ReadUrl(url, "Release");
            //File.Delete("Release");

            // different OSes uses different endings so this splits each line into a element of an array
            string[] split = releasefile.Split("\r\n");
            if (split.Length < 2) split = releasefile.Split("\n");
            if (split.Length < 2) split = releasefile.Split('\r');

            // Intilize a dictionary for repo data 
            this.data = new Dictionary<string, string>();
            // This will go through each line and place x of "x: y" in key and y in value
            foreach (string data in split) {
                string[] keyval = data.Split(": ");
                if (keyval.Length < 2) break;
                this.data.Add(keyval[0], keyval[1]);
            }

            this.data.TryGetValue("Label", out this.name); // Grabs repo name and puts it in this.name
            string dir = this.name; // Repo directory

            DownloadPackages(dir); //Puts packages file in *dir*/Packages

            // Reads the Packages file stored in repo directory
            string content = Helper.ReadFromPath(dir + (dir.EndsWith('/') ? "Packages" : "/Packages"));

            // different OSes uses different endings so this splits each line into a element of an array
            string[] contentarr = content.Split("\n\n");
            if (contentarr.Length < 2) contentarr = content.Split("\r\n\r\n");
            if (contentarr.Length < 2) contentarr = content.Split("\r\r");
            // Initilize a dictionary with format name: packagedata
            this.packages = new Dictionary<string, Package>();
            foreach (string package in contentarr) {
                // Initilize a Dictionary dataname: data
                Dictionary<string, string> packagedatadict = new Dictionary<string, string>();

                // different OSes uses different endings so this splits each line into a element of an array
                string[] packagedata = package.Split('\n');
                if (packagedata.Length < 2) packagedata = package.Split('\r');
                if (packagedata.Length < 2) packagedata = package.Split("\r\n");
                // This will go through each line and place x of "x: y" in key and y in value
                foreach (string data in packagedata) {
                    string[] keyval = data.Split(": ");
                    if (keyval.Length < 2) break;
                    packagedatadict.Add(keyval[0], keyval[1]);
                }
                string name;
                if (packagedatadict.TryGetValue("Package", out name)) {
                    if (!this.packages.TryAdd(name, new Package(packagedatadict, this.url)) && Helper.debug)
                        Console.WriteLine("Duplicate name, skipping...");
                } else {
                    // TODO 
                    // Check version and overwrite if newer
                    Console.WriteLine("Could not grab name from package, skipping...");
                    continue;
                }
            }
        }
        private void DownloadPackages(string dir) {
            try {
                string dire = dir + (dir.EndsWith('/') ? "Packages" : "/Packages");
                string urle = this.url + (this.url.EndsWith('/') ? "Packages.bz2" : "/Packages.bz2");
                Helper.DecompressUrl(urle, dire);
            } catch (Exception ex) {
                Console.WriteLine("Failed to download. Error " + ex.ToString());
            }
        }
    }
}
