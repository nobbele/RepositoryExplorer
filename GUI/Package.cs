
using System;
using System.IO;
using System.Windows.Forms;

namespace GUI
{
    [Serializable]
    public class Package
    {
        public SerializableDictionaryString data;
        public string url, name, package, section, depends, filename, md5, version, description, depiction, todownload;
        public string debloc;
        public long size;

        public bool selected;
        public Package() {

        }
        public Package(SerializableDictionaryString _data, string url, string debloc="") {

            this.data = _data;
            
            bool success = this.data.TryGetValue("Filename", out this.url);
            this.url = this.url.Replace(url, "");
            this.url = this.url.Replace("./", "");
            url = url.Replace("./", "");
            if (success) {
                //this.url = (url.EndsWith("/") ? url : url + "/") + this.url;
                if (url.EndsWith("php") || url.EndsWith("/")) {
                    this.url = url + this.url;
                } else {
                    this.url = url + "/" + this.url;
                }
            } else {
                this.url = "Invalid";
            }
            this.debloc = debloc;
            if (this.debloc == "") this.debloc = this.url;

            if (!this.data.TryGetValue("Name", out this.name)) {
                print("No name for a package from {0}", this.url);
                this.name = "Unknown name";
            }
            if (!this.data.TryGetValue("Package", out this.package)) print("No package id for {0}", this.name);
            if (!this.data.TryGetValue("Section", out this.section)) print("No section for {0}", this.name);
            if (!this.data.TryGetValue("Depends", out this.depends)) print("No depends for {0}", this.name);
            if (!this.data.TryGetValue("Version", out this.version)) print("No version for {0}", this.name);
            if (!this.data.TryGetValue("Filename", out this.filename)) print("No filename for {0}", this.name);
            if (!this.data.TryGetValue("Depiction", out this.depiction)) print("No filename for {0}", this.name);
            string _size = "";
            this.data.TryGetValue("Size", out _size);
            if (!long.TryParse(_size, out this.size))
                print("Invalid size for {0}", this.package);
            if (!this.data.TryGetValue("MD5sum", out this.md5)) print("No MD5sum for {0}", this.name);
            if (!this.data.TryGetValue("Description", out this.description)) print("No description for {0}", this.name);
            this.todownload = this.debloc + "/" + this.filename;

        }
        public void print(object obj) {
            if(Helper.debug) {
                Console.WriteLine(obj);
            }
        }
        public void print(string obj, object arg1) {
            if (Helper.debug) {
                Console.WriteLine(obj, arg1);
            }
        }
        public override string ToString() {
            return this.name;
        }
        public override bool Equals(object obj) {
            Package p = obj as Package;
            if (p == null) return false;
            return (this.name == p.name);
        }
        public string getdebname() {
            return (this.package == null ? this.name : this.package) + ".deb";
        }
        public void download(string directory) {
            string file = directory + "/" + getdebname();
            try {
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            } catch (System.ArgumentException) {
                directory = "debs";
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                file = directory + "/" + (this.package == null ? this.name : this.package) + ".deb";
            }
            try {
                using (var wc = new System.Net.WebClient()) {
                    wc.DownloadFile(this.todownload, file);
                }
            } catch (System.Net.WebException e) {
                MessageBox.Show("Couldn't download package " + this.name + " (This will be shown on paid packages, if this package is not paid please contact 4pplecracker on twitter or reddit\n" + e.Message);
            }
        }
    }
}