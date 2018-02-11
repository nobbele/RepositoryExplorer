using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using HtmlAgilityPack;

namespace GUI
{
    class CydiaAPI
    {
        private static string getjsonprice(string packageid) {
            string json = "";
            string url = @"https://cydia.saurik.com/api/ibbignerd?query=" + packageid;

            using (WebClient wc = new WebClient()) {
                json = wc.DownloadString(url);
            }

            return json;
        }
        public static string getprice(string packageid) {
            string json = getjsonprice(packageid);
            if (json != null) {
                dynamic jsoncode = JsonConvert.DeserializeObject(json);
                if (jsoncode != null) {
                    string price = (string)jsoncode.msrp;
                    return "$" + price;
                }
            }
            return "Free";
        }
    }
}
