using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NicoTube_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Length < 16)
                return;

            if (textBox1.Text.Substring(7, 16) == "www.nicovideo.jp")
            {
                //not logged in try login
                if (Properties.Settings.Default.NICOVIDEO_ID == string.Empty)
                {
                    Login nicolg = new Login();
                    nicolg.ShowDialog();

                    if (nicolg.id == null || nicolg.pw == null)
                        return;

                    Properties.Settings.Default.NICOVIDEO_ID = nicolg.id;


                    Properties.Settings.Default.NICOVIDEO_PW_ENCRYPT = EnDecryptor.EncryptString(EnDecryptor.ToSecureString(nicolg.id));

                    Properties.Settings.Default.Save();
                }

                //logged IN!

                string id = Properties.Settings.Default.NICOVIDEO_ID;
                string pass = EnDecryptor.ToInsecureString(EnDecryptor.DecryptString(Properties.Settings.Default.NICOVIDEO_PW_ENCRYPT));

                Uri u = new Uri(textBox1.Text);
                string videoid = u.Segments.Last();

                NicoVideoAPI nvapi = new NicoVideoAPI();
                nvapi.Login(id, pass);

                DownloadDialog dd = new DownloadDialog();
                dd.DOWNLOAD_URL = textBox1.Text;
                dd.cookie = nvapi.getCookie();
                dd.VIDEO_ID = videoid;

                dd.ShowDialog();

                dd.Dispose();
                

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.NICOVIDEO_ID = string.Empty;
            Properties.Settings.Default.NICOVIDEO_PW_ENCRYPT = string.Empty;

            Properties.Settings.Default.Save();

            MessageBox.Show("Cleared!");
        }



    }
}
