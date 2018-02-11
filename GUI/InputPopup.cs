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
    public partial class InputPopup : Form
    {
        public InputPopup(string question, bool ispassword=false) {
            InitializeComponent();
            label1.Text = question;
            answer.UseSystemPasswordChar = ispassword;
        }

        private void InputPopup_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
