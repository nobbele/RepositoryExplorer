using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace GUI
{
    class CydiaAPI
    {
        private static string gethtml(string packageid, string device, string UDID) {
            string html = "";
            string url = @"https://cydia.saurik.com/api/commercial?package=" + packageid;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Headers.Add("X-Machine", device);
            request.Headers.Add("X-Cydia-Id", UDID);
            request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 10_2 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Mobile/14G60 Safari/602.1 Cydia/1.1.30 CyF/1349.70";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                using (Stream stream = response.GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        html = reader.ReadToEnd();
                    }
                }
            }
            return html;
        }
        public static string getprice(string packageid, string device = "iPhone6,1", string UDID = "867ddda319d4761bbce3d211c44f454d268d3271") {
            string html = gethtml(packageid, device, UDID);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var pricelabels = doc.DocumentNode
            .Descendants("label")
            .Where(d =>
                d.Attributes.Contains("class")
                &&
                d.Attributes["class"].Value.Contains("price")
            );
            foreach(HtmlNode pricelabel in pricelabels) {
                Console.WriteLine(pricelabel.OuterHtml);
                return pricelabel.InnerText;
            }
            return "Free";
        }
    }
}
