using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI
{
    class Helper
    {
        public const bool debug = false;
        public static FileInfo DownloadWithName(string file, string name, string device="iPhone6,1", string UDID= "867ddda319d4761bbce3d211c44f454d268d3271") {
            string[] dir = name.Split('/');
            if(dir.Length > 1) Directory.CreateDirectory(dir[0]);
            using (var wc = new System.Net.WebClient()) {
                wc.Headers.Add("X-Machine", device);
                wc.Headers.Add("X-Unique-Id", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                wc.Headers.Add("X-Firmware", "10.1.1");
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 10_2 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Mobile/14G60 Safari/602.1 Cydia/1.1.30 CyF/1349.70");
                wc.DownloadFile(file, name);
            }
            string localpath = Directory.GetCurrentDirectory() + "/" + name;
            return (new FileInfo(localpath));
        }
        public static FileInfo Download(string file, string device = "iPhone6,1", string UDID = "867ddda319d4761bbce3d211c44f454d268d3271") {
            string[] split = file.Split('/');
            string name = split[split.Length - 1];
            if (name == "") {
                name = split[split.Length - 2];
            }
            using (var wc = new System.Net.WebClient()) {
                wc.Headers.Add("X-Machine", device);
                wc.Headers.Add("X-Unique-Id", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                wc.Headers.Add("X-Firmware", "10.1.1");
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 10_2 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Mobile/14G60 Safari/602.1 Cydia/1.1.30 CyF/1349.70");
                wc.DownloadFile(file, name);
            }
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
            if (file.Exists) {
                byte[] b = File.ReadAllBytes(file.FullName);
                UTF8Encoding temp = new UTF8Encoding(true);
                str = temp.GetString(b);
            }
            return str;
        }
        public static String ReadUrl(string url, string name) {
            return ReadFileInfo(DownloadWithName(url + (url.EndsWith("/") ? name : "/" + name), name));
        }
        // Extracts the file contained within a GZip to the target dir.
        // A GZip can contain only one file, which by default is named the same as the GZip except
        // without the extension.
        //
        public static void ExtractGZipSample(string gzipFileName, string targetDir) {

            // Use a 4K buffer. Any larger is a waste.    
            byte[] dataBuffer = new byte[4096];

            using (System.IO.Stream fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read)) {
                using (GZipInputStream gzipStream = new GZipInputStream(fs)) {

                    // Change this to your needs
                    string fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

                    using (FileStream fsOut = File.Create(fnOut)) {
                        StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                    }
                }
            }
        }
        public static void DecompressUrl(string url, string localfile, string type="bz2") {
            /*
             * God dammit coolstar, you wasted like 5 hours from me cuz of the stupid non-compressed Packages file in your repo
             */
            string[] splt = localfile.Split('/');
            string dir = splt[0];
            Directory.CreateDirectory(dir);
            
            if (!url.StartsWith("http")) {
                url = "http://" + url;
                MessageBox.Show("Please include http:// or https:// in the beginning next time");
            }

            if (type == "bz2") {
                string bz2file = localfile + ".bz2";
                FileInfo packagesfile = Helper.DownloadWithName(url, bz2file);
                FileStream packagesbz2 = (FileStream)Helper.InfoToStream(packagesfile);

                FileStream packages = File.Create(localfile);
                ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(packagesbz2, packages, true);
            }
            if (type == "gz") {
                string gzfile = localfile + ".gz";
                Helper.DownloadWithName(url, gzfile);
                ExtractGZipSample(gzfile, dir);
            }
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
                string[] keyval = data.Split(new string[] { ": " }, StringSplitOptions.None);
                if (keyval.Length < 2) break;
                dict.Add(keyval[0], keyval[1]);
            }
            return dict;
        }
        public static string[] ParseFiles(string tosplit) {
            string[] split = tosplit.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (split.Length < 2) split = tosplit.Split('\n');
            if (split.Length < 2) split = tosplit.Split('\r');
            return split;
        }
        public static string[] TwiceParseFiles(string tosplit) {
            string[] split = tosplit.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
            if (split.Length < 2) split = tosplit.Split(new string[] { "\n\n" }, StringSplitOptions.None);
            if (split.Length < 2) split = tosplit.Split(new string[] { "\r\r" }, StringSplitOptions.None);
            return split;
        }
        public static void AddFileToZip(ZipOutputStream zStream, string relativePath, string file) {
            byte[] buffer = new byte[4096];
            string fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty) + Path.GetFileName(file);
            ZipEntry entry = new ZipEntry(fileRelativePath);

            entry.DateTime = DateTime.Now;
            zStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(file)) {
                int sourceBytes;

                do {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }
        public static void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset) {

            string[] files = Directory.GetFiles(path);

            foreach (string filename in files) {

                FileInfo fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename)) {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders) {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }
        public static bool Versioncheck(string newer, string older) {
            string[] newpak = newer.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            string[] oldpak = older.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            int newmajor = 0;
            int oldmajor = 0;
            int newminor = 0;
            int oldminor = 0;
            int newpatch = 0;
            int oldpatch = 0;

            for (int i = 0; i < newpak.Length; i++) {
                switch (i) {
                    case 0:
                        int.TryParse(newpak[i], out newmajor);
                        break;
                    case 1:
                        int.TryParse(newpak[i], out newminor);
                        break;
                    case 2:
                        int.TryParse(newpak[i], out newpatch);
                        break;
                    default:
                        break;
                }
            }
            for (int i = 0; i < oldpak.Length; i++) {
                switch (i) {
                    case 0:
                        int.TryParse(oldpak[i], out oldmajor);
                        break;
                    case 1:
                        int.TryParse(oldpak[i], out oldminor);
                        break;
                    case 2:
                        int.TryParse(oldpak[i], out oldpatch);
                        break;
                    default:
                        break;
                }
            }

            if (newmajor > oldmajor) {
                return true;
            } else if (newmajor == oldmajor) {
                // do more checking
                if (newminor > oldminor) {
                    return true;
                } else if (newminor == oldminor) {
                    // do more checking
                    if (newpatch > oldpatch) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
