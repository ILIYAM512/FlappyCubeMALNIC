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
    public partial class LeaderBoardForm : Form
    {
        int _currentscore = 0;
        public LeaderBoardForm(int currentscore)
        {
            InitializeComponent();
            _currentscore = currentscore;
            try
            {
                textBox1.Text = File.ReadAllText("Leaders.FlappyCube");
            }
            catch (FileNotFoundException)
            {
                File.Create("Leaders.lol");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.AppendText(textBox2.Text + ":" + _currentscore.ToString() + Environment.NewLine);
        }

        private void LeaderBoardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText("Leaders.FlappyCube", textBox1.Text);
        }
    }
}
