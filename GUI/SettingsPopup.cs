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
    public partial class SettingsPopup : Form
    {
        public string ip;
        public string port;
        public string password;
        public string debloc;
        public SettingsPopup(string ipset, string portset, string passwordset, string deblocset) {
            InitializeComponent();
            ipfield.Text = ipset;
            portfield.Text = portset;
            passfield.Text = passwordset;
            deblocation.Text = deblocset;
        }

        private void SettingsPopup_Load(object sender, EventArgs e) {
        }

        private void ipfield_TextChanged(object sender, EventArgs e) {
            ip = ipfield.Text;
        }

        private void portfield_TextChanged(object sender, EventArgs e) {
            port = portfield.Text;
        }

        private void passfield_TextChanged(object sender, EventArgs e) {
            password = passfield.Text;
        }

        private void deblocation_TextChanged(object sender, EventArgs e) {
            debloc = deblocation.Text;
        }

        private void button1_Click(object sender, EventArgs e) {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.ShowDialog();
            debloc = f.SelectedPath;
        }
    }
}
