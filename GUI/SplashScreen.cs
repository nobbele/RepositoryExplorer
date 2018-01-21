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
    public partial class SplashScreen : Form
    {
        public SplashScreen() {
            InitializeComponent();
        }
        public string[] tips = { "The more repos you have installed\nThe longer the load will take!", "Fuck Microsoft", "Default repos are incredibly slow" };
        private void SplashScreen_Load(object sender, EventArgs e) {
            int max = tips.Length;
            Tip.Text = tips[(new Random().Next(max))];
        }
    }
}
