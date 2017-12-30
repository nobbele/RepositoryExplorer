using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GUI
{
    class Helper
    {
        public const bool debug = true;
        public static FileInfo Download(string file, string name) {
            string[] dir = name.Split('/');
            if(dir.Length > 1) Directory.CreateDirectory(dir[0]);
            using (var wc = new System.Net.WebClient()) {
                //try {
                try {
                    wc.DownloadFile(file, name);
                } catch (System.Net.WebException) {
                    
                } catch (System.ArgumentException ex) {

                }
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
            if (file.Exists) {
                using (FileStream fs = file.OpenRead()) {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);

                    while (fs.Read(b, 0, b.Length) > 0) {
                        str += temp.GetString(b);
                    }
                }
            }
            return str;
        }
        public static String ReadUrl(string url, string name) {
            return ReadFileInfo(Download(url + (url.EndsWith("/") ? name : "/" + name), name));
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
    }
}
