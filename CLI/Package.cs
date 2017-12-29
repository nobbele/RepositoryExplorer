
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;

namespace CLI
{
    class Package
    {
        public Dictionary<string, string> data;
        public string url;
        public string name;
        public string package;
        public string section;
        public string depends;
        public int size;
        public string md5;
        public string description;
        public Package(Dictionary<string, string> _data, string url) {

            this.data = _data;
            bool success = this.data.TryGetValue("Filename", out this.url);
            if (success)
                this.url = this.url.Replace("./", url.EndsWith('/') ? url : url + "/");
            else {
                this.url = "Invalid";
            }
            if (!this.data.TryGetValue("Name", out this.name)) {
                print("No name for a package from {0}", this.url);
                this.name = "Unknown name";
            }
            if (!this.data.TryGetValue("Package", out this.package)) print("No package id for {0}", this.name);
            if (!this.data.TryGetValue("Section", out this.section)) print("No section for {0}", this.name);
            if (!this.data.TryGetValue("Depends", out this.depends)) print("No depends for {0}", this.name);
            string _size = "";
            this.data.TryGetValue("Size", out _size);
            if (!int.TryParse(_size, out this.size))
                print("Invalid size for {0}", this.package);
            if (!this.data.TryGetValue("MD5sum", out this.md5)) print("No MD5sum for {0}", this.name);
            if (!this.data.TryGetValue("Description", out this.description)) print("No description for {0}", this.name);

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
    }
}