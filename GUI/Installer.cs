using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    static class Installer
    {
        //Credits to u/josephwalden for creating the tic.exe program
        private static void setsettings(string[] settings) {
            File.WriteAllLines("tics/settings", settings);
        }
        private static string join(List<FileInfo> s, string i) {
            string temp = "";
            foreach (FileInfo j in s) {
                temp += '"' + j.FullName + '"' + i;
            }
            return temp;
        }
        private static void electra(string deb, string[] data) {
            setsettings(data);
            Process.Start("tics/tic.exe", "dont-update " + "install " + deb);
            File.Delete("tics/settings");
            string[] safedata = { data[0], data[1], "" };
            setsettings(safedata);
        }
        public static void installelectra(List<FileInfo> debs, string host, string port, string password) {
            string[] data = { host, port, password };
            electra(join(debs, " "), data);
        }
        public static void installelectrasingle(FileInfo deb, string host, string port, string password) {
            string[] data = { host, port, password };
            electra(deb.FullName, data);
        }
        public static string[] getsettings() {
            if (!File.Exists("tics/settings")) {
                string[] def = new string[3];
                File.WriteAllLines("tics/settings", def);
            }
            return File.ReadAllLines("tics/settings"); //get ssh settings
        }
        public static void installnormal(FileInfo deb, string host, string port, string password) {
            int p = 22;
            int.TryParse(port, out p);

            string path = "/tmp/" + deb.Name;
            using (var client = new SftpClient(host, p, "root", password)) {
                client.Connect();

                FileStream fileStream = new FileStream(deb.FullName, FileMode.Open);
                if (client.IsConnected) {
                    if (fileStream != null) {
                        client.UploadFile(fileStream, path, null);
                        client.Disconnect();
                        client.Dispose();
                    }
                }
            }

            using (var client = new SshClient(host, p, "root", password)) {
                client.Connect();

                var r = client.RunCommand("dpkg -i " + path);
                Console.WriteLine(r.Result);
                client.RunCommand("rm " + path);
                client.Disconnect();
            }
        }
    }
}
