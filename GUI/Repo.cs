using System;
using System.IO;
using System.Windows.Forms;

namespace GUI
{
    [Serializable]
    public class Repo
    {
        public SerializableDictionary<string, Package> packages;
        public SerializableDictionaryString data;
        public string url;
        public string name;
        public Repo() {
            
        }
        public Repo(string url, ProgressBar RefreshProgress) {
            RefreshProgress.SetProgressNoAnimation(0);
            this.url = url;

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
            string dir = this.name; // Repo directory
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
            if (!(dir == null)) {

                DownloadPackages(dir); //Puts packages file in *dir*/Packages

                RefreshProgress.SetProgressNoAnimation(20); // 35

                // Reads the Packages file stored in repo directory
                string content = Helper.ReadFromPath(dir + (dir.EndsWith("/") ? "Packages" : "/Packages"));

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
                if (!File.Exists(dire)) {
                    string urle = this.url + (this.url.EndsWith("/") ? "Packages.bz2" : "/Packages.bz2");
                    Helper.DecompressUrl(urle, dire);
                    File.Delete(dir + (dir.EndsWith("/") ? "Packages.bz2" : "/Packages.bz2"));
                }
            } catch (Exception ex) {
                Console.WriteLine("Failed to download. Error " + ex.ToString());
            }
        }
        public override string ToString() {
            return this.name;
        }
    }
    public static class ExtensionMethods
    {
        /// <summary>
        /// Sets the progress bar value, without using 'Windows Aero' animation.
        /// This is to work around a known WinForms issue where the progress bar 
        /// is slow to update. 
        /// </summary>
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
    }
}

