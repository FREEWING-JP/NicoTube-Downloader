using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NicoTube_Downloader
{
    public partial class Login : Form
    {

        public string id;
        public string pw;

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NicoVideoAPI nvapi = new NicoVideoAPI();

            if(!nvapi.Login(textBox1.Text, textBox2.Text))
            {
                MessageBox.Show("Invaild ID or Password. Please try again.");
                return;
            }

            id = textBox1.Text;
            pw = textBox2.Text;

            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty)
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty)
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }
    }
}
