using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class DefaultRepos : Form
    {
        private Image fromfile(string name) {
            string file = Environment.CurrentDirectory + "/resources/" + name + ".ico";
            return Image.FromFile(file);
        }
        public string chosen;
        public DefaultRepos() {
            InitializeComponent();

            chosen = "";

            var imageList = new ImageList();

            view.LargeImageList = imageList;

            List<string> repos = new List<string>();
            repos.Add("bigboss");
            repos.Add("modmyi");


            foreach (string r in repos) {
                imageList.Images.Add(r, fromfile(r));
            }
            foreach (string r in repos) {
                ListViewItem i = view.Items.Add(r);
                i.ImageKey = r;
            }
        }

        private void add_Click(object sender, EventArgs e) {
            this.chosen = view.FocusedItem.Text;
            this.Close();
        }

        private void view_SelectedIndexChanged(object sender, EventArgs e) {
            Console.WriteLine("changed");
        }
    }
}
