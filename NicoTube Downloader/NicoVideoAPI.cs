using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace NicoTube_Downloader
{
    class NicoVideoAPI
    {
        private readonly string LOGIN_FORM_URL = "https://secure.nicovideo.jp/secure/login?";
        private CookieContainer cookie;

        public bool Login(string id, string password)
        {
            Hashtable vals = new Hashtable();
            vals["mail_tel"] = id;
            vals["password"] = password;

            var st = Post(LOGIN_FORM_URL, vals, Encoding.ASCII, out cookie);
            var sr = new StreamReader(st, Encoding.UTF8);

            //失敗の判断はメッセージの有無で決める
            return !sr.ReadToEnd().Contains("ロック");
        }

        public CookieContainer getCookie()
        {
            return cookie;
        }

        public static Stream Post(string url, Hashtable param, Encoding enc, out CookieContainer cc)
        {
            string query = "";
            foreach (string k in param.Keys)
                query += String.Format("{0}={1}&", k, param[k]);
            byte[] data = enc.GetBytes(query);

            query += "show_button_twitter=1&site=niconico&show_button_facebook=1";

            cc = new CookieContainer();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            req.CookieContainer = cc;

            //POST送信
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            return req.GetResponse().GetResponseStream();
        }

        public string GetDownloadURL(string videoid)
        {
            Stream stm;

            stm = Get("http://flapi.nicovideo.jp/api/getflv/" + videoid , cookie);

            var str = new StreamReader(stm);
            string ret = str.ReadToEnd();
           
            

            return DeSeriallizeVideoInfo(ret);
        }

        public static Stream Get(string uri, CookieContainer cc)
        {
            var req = (HttpWebRequest)WebRequest.Create(uri);
            req.CookieContainer = cc;
            return req.GetResponse().GetResponseStream();
        }

        public static string DeSeriallizeVideoInfo(string str)
        {
            try
            {
                if (str.Contains("&url=") && str.Contains("&ms"))
                {
                    return HttpUtility.UrlDecode(str.Substring(str.IndexOf("&url=") + 5, str.IndexOf("&ms") - (str.IndexOf("&url=") + 5)));
                }
            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }

    }
}
