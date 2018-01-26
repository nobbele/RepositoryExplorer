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
        private void addrep(string r, Icon img) {
            imageList.Images.Add(r, img);
            ListViewItem i = view.Items.Add(r);
            i.ImageKey = r;
        }
        ImageList imageList;
        public string chosen;
        public DefaultRepos() {
            InitializeComponent();

            chosen = "";

            imageList = new ImageList();

            view.LargeImageList = imageList;

            addrep("bigboss", Properties.Resources.bigboss);
            addrep("modmyi", Properties.Resources.modmyi);
            addrep("saurik", Properties.Resources.saurik);


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
