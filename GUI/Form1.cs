using CydiaForWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        Dictionary<string, Package> selected;
        List<Repo> repos;
        public Form1() {
            InitializeComponent();
            repos = new List<Repo>();
            selected = new Dictionary<string, Package>();
        }

        private void Refresh_Click(object sender, EventArgs e) {
            repos.Clear();
            AddRepo(EnterRepo.Text);
            RefreshProgress.Value = 100;
            
        }
        private void AddRepo(string url) {
            if (!url.EndsWith("/")) url += "/";
            
            repos.Add(new Repo(url, RefreshProgress));
            Reponame.Text = repos[0].name;
            Packages.Items.Clear();
            if (repos[0].packages != null) {
                foreach (Package pak in repos[0].packages.Values) {
                    Packages.Items.Add(pak);
                }
            }
        }

        private void Packages_ItemCheck_1(object sender, ItemCheckEventArgs e) {

            Package packtochange = (Package)Packages.Items[e.Index];
            if (e.CurrentValue == CheckState.Unchecked) selected.Add(packtochange.name, packtochange);
            else selected.Remove(packtochange.name);

            Package pak = repos[0].packages.Values.ToArray<Package>()[e.Index];
            name.Text = pak.name;
            packageid.Text = pak.package;
            section.Text = pak.section;
            md5.Text = pak.md5;
            size.Text = pak.size.ToString();
            description.Text = pak.description;
            depends.Text = pak.depends;
            URL.Text = pak.url;
            URL.LinkVisited = false;
        }
        protected override void OnClosed(EventArgs e) {
            foreach(Repo r in repos) {
                if (r != null) {
                    Directory.Delete(r.name, true);
                }
            }
        }

        private void URL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            LinkLabel l = sender as LinkLabel;
            try {
                Console.WriteLine("Opening link " + l.Text);
                System.Diagnostics.Process.Start(l.Text);
                l.LinkVisited = true;
            } catch (System.ComponentModel.Win32Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
