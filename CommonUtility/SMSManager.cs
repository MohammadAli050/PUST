using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class SMSManager
    {
        private static string userName = "bup789";
        private static string password = "01769021586";
        private static string sender = "BUP";

        //private static string userName1 = "edusoft";
        //private static string password1 = "123456";
        //private static string sender1 = "UIU";

        //method for short message or for single sms
        public async static void SendShortSMS(string phoneNo, string roll, string msg, Action<string[]> action)
        {
            string[] dt = new string[3];
            try
            {
                var client = new WebClient();
                phoneNo = phoneNo.Replace("+", "00");
                dt[0] = roll;
                dt[1] = msg;
                dt[2] = await client.DownloadStringTaskAsync("http://app.planetgroupbd.com/api/sendsms/plain?user=" +
                    userName + "&password=" + password + "&sender=BUP" +
                    "&SMSText=" + System.Web.HttpUtility.UrlEncode(msg) + "&GSM=" + phoneNo);

                action(dt);
            }
            catch
            {
                dt[0] = roll;
                dt[1] = msg;
                dt[2] = "";

                action(dt);
            }
        }

        //method for long message, which is usually more then one sms.
        public async static void SendLongSMS(string phoneNo, string roll, string msg, int studentId, int acaCalId, Action<string[]> action)
        {
            string phoneNo1 = phoneNo;
            string[] dt = new string[6];
            try
            {
                var client = new WebClient();
                phoneNo = phoneNo.Replace("+", "00");
                dt[0] = roll;
                dt[1] = msg;
                dt[2] = phoneNo1;
                dt[3] = Convert.ToString(studentId);
                dt[4] = Convert.ToString(acaCalId);
                dt[5] = await client.DownloadStringTaskAsync("http://api.bulksms.icombd.com/api/v3/sendsms/plain?user=" + userName + "&password=" + password
                    + "&sender=UIU" + "&SMSText=" + System.Web.HttpUtility.UrlEncode(msg) + "&GSM=" + phoneNo + "&type=longSMS");

                action(dt);
            }
            catch
            {
                dt[0] = roll;
                dt[1] = msg;
                dt[2] = phoneNo1;
                dt[3] = Convert.ToString(studentId);
                dt[4] = Convert.ToString(acaCalId);
                dt[5] = "";

                action(dt);
            }
            //return dt;
        }

        public static string send(string phoneNo, string message)
        {


            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://app.planetgroupbd.com/api/sendsms/plain?user="
                + userName + "&password=" + password + "&sender=BUP"
                + "&SMSText=" + System.Web.HttpUtility.UrlEncode(message) + "&GSM=" + phoneNo);

            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            myResp.Close();
            return responseString;
        }

        public static string send2(string phoneNo, string message, string clientRefNo)
        {

            string sid = "edusoft";
            string user = "edusoft";
            string pass = "123456";
            string HtmlResult = null;
            phoneNo = phoneNo.Replace("+", "");
            string URI = "http://sms.sslwireless.com/pushapi/dynamic/server.php";
            string myParameters = "user=" + user + "&pass=" + pass + "&sms[0][0]=" + phoneNo + "&sms[0][1]=" + System.Web.HttpUtility.UrlEncode(message) + "&sms[0][2]=" + clientRefNo + "&sid=" + sid;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(URI, myParameters);
                //Console.Write(HtmlResult); 
            }
            return HtmlResult;
        }

        public async static void GetBalance(Action<string> action)
        {
            string balance = "";
            try
            {
                var client = new WebClient();
                balance = await client.DownloadStringTaskAsync("http://api.bulksms.icombd.com/api/command?username=" + userName + "&password=" + password + "&cmd=CREDITS");
                action(balance);
            }
            catch
            {

                action(balance);
            }
        }


    }
}
