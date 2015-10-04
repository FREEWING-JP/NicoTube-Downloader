using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NicoTube_Downloader
{
    public partial class DownloadDialog : Form
    {
        public string DOWNLOAD_URL;
        public CookieContainer cookie;
        public string VIDEO_ID;

        string FILE_NAME;

        public DownloadDialog()
        {
            InitializeComponent();
        }

        HttpWebRequest webRequest;
        bool closeit;

        private void DownloadDialog_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
                Application.DoEvents();
            }

            SaveFileDialog sfd = new SaveFileDialog();

            string videoid = VIDEO_ID;

            sfd.FileName = videoid + ".mp4";

            sfd.Filter =
                "mp4(*.mp4)|*.mp4|すべてのファイル(*.*)|*.*";

            //ダイアログを表示する
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            //ダウンロードするファイル
            string url = DOWNLOAD_URL;
            //保存先のファイル名
            FILE_NAME = sfd.FileName;

            //WebRequestを作成 
            webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

            webRequest.CookieContainer = cookie;
            webRequest.Referer = "http://www.nicovideo.jp/watch/" + videoid;


            webRequest.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);


            ProgressAnimation.Start();
           

        }

        void FinishWebRequest(IAsyncResult result)
        {
            ProgressAnimation.Stop();

            WebResponse response = webRequest.EndGetResponse(result); 

            //応答データを受信するためのStreamを取得
            System.IO.Stream strm = response.GetResponseStream();

            //ファイルに書き込むためのFileStreamを作成
            System.IO.FileStream fs = new System.IO.FileStream(
                FILE_NAME, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            //応答データをファイルに書き込む
            byte[] readData = new byte[1024];
            for (; ; )
            {
                //データを読み込む
                int readSize = strm.Read(readData, 0, readData.Length);
                if (readSize == 0)
                {
                    //すべてのデータを読み込んだ時
                    break;
                }
                //読み込んだデータをファイルに書き込む
                fs.Write(readData, 0, readSize);
            }

            //閉じる
            fs.Close();
            strm.Close();

            MessageBox.Show("Complete!");

            closeit = true;
        }

        int a;

        private void timer1_Tick(object sender, EventArgs e)
        {

            a++;
            label1.Text = (label1.Text += ".");

            if(a > 5)
            {
                a = 0;
                label1.Text = "Downloading";
            }

        }

        private void Asynclose_Tick(object sender, EventArgs e)
        {
            if (closeit)
            {
                closeit = false;
                Asynclose.Stop();
                this.Close();
            }
        }
    }
}
