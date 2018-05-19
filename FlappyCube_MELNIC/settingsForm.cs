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

namespace FlappyCube_MELNIC
{
    public partial class settingsForm : Form
    {
        public settingsForm()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] settings = new string[3];

            settings[0] = Convert.ToString(trackBar1.Value);
            settings[1] = Convert.ToString(trackBar2.Value);
            settings[2] = Convert.ToString(checkBox1.Checked);

            File.WriteAllLines("settings", settings);
        }
    }
}
