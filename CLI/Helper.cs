using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CLI
{
    class Helper
    {
        public const bool debug = true;
        public static FileInfo Download(string file, string name) {
            string[] dir = name.Split('/');
            if(dir.Length > 1) Directory.CreateDirectory(dir[0]);
            using (var wc = new System.Net.WebClient()) {
                //try {
                wc.DownloadFile(file, name);
                /*} catch (System.Net.WebException e) {
                    Console.WriteLine("Error occured: " + e.Message);
                    Console.WriteLine("Can't continue, exiting in 3 seconds");
                    System.Threading.Thread.Sleep(3000);
                    Environment.Exit(-1);
                }*/
            }
            string localpath = Directory.GetCurrentDirectory() + "/" + name;
            return (new FileInfo(localpath));
        }
        public static FileInfo Download(string file) {
            string[] split = file.Split('/');
            string name = split[split.Length - 1];
            if (name == "") {
                name = split[split.Length - 2];
            }
            using (var wc = new System.Net.WebClient())
                wc.DownloadFile(file, name);
            string localpath = Directory.GetCurrentDirectory() + "/" + name;
            return (new FileInfo(localpath));
        }
        public static Stream InfoToStream(FileInfo file) {
            Stream s;
            s = file.OpenRead();
            return s;
        }
        public static String ReadFileInfo(FileInfo file) {
            string str = "";
            using (FileStream fs = file.OpenRead()) {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0) {
                    str += temp.GetString(b);
                }
            }
            return str;
        }
        public static String ReadUrl(string url, string name) {
            return ReadFileInfo(Download(url + (url.EndsWith('/') ? name : "/" + name), name));
        }
        public static void DecompressUrl(string url, string localfile) {
            /*
             * God dammit coolstar, you wasted like 5 hours from me cuz of the stupid non-compressed Packages file in your repo
             */
            string dir = localfile.Split('/')[0];
            Directory.CreateDirectory(dir);
            string bz2file = localfile + ".bz2";
            
            FileInfo packagesfile = Helper.Download(url, bz2file);
            FileStream packagesbz2 = (FileStream)Helper.InfoToStream(packagesfile);

            FileStream packages = File.Create(localfile);
            ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(packagesbz2, packages, true);
        }
        public static string RemoveSpecialCharacters(string str) {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
        public static string ReadFromPath(string url) {
            FileInfo file = new FileInfo(url);
            string content = "";
            if (file.Exists)
                content = ReadFileInfo(file);
            return content;
        }
        public static Dictionary<string, string> ParseFiles(string[] file) {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string data in file) {
                string[] keyval = data.Split(": ");
                if (keyval.Length < 2) break;
                dict.Add(keyval[0], keyval[1]);
            }
            return dict;
        }
        public static string[] ParseFiles(string tosplit) {
            string[] split = tosplit.Split("\r\n");
            if (split.Length < 2) split = tosplit.Split("\n");
            if (split.Length < 2) split = tosplit.Split('\r');
            return split;
        }
        public static string[] TwiceParseFiles(string tosplit) {
            string[] split = tosplit.Split("\r\n\r\n");
            if (split.Length < 2) split = tosplit.Split("\n\n");
            if (split.Length < 2) split = tosplit.Split("\r\r");
            return split;
        }
    }
}
