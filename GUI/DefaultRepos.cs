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
            try {
                return Image.FromFile(file);
            } catch (System.IO.FileNotFoundException) {
                MessageBox.Show("Couldn't find " + file + ", please redownload the program");
                return null;
            }
        }
        public string chosen;
        public DefaultRepos() {
            InitializeComponent();

            chosen = "";

            var imageList = new ImageList();

            view.LargeImageList = imageList;

            List<string> names = new List<string>();

            names.Add("bigboss");
            names.Add("modmyi");

            List<string> repos = new List<string>();

            foreach (string r in names) {
                Image img = fromfile(r);
                if (img != null) {
                    imageList.Images.Add(r, img);
                    ListViewItem i = view.Items.Add(r);
                    i.ImageKey = r;
                }
            }
            if (view.Items.Count < 1) {
                this.Close();
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
