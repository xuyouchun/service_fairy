using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

using Common.Package.Service;
using Common.Package;
using ServiceFairy.DbEntities.Sms;

namespace ServiceFairy.Service.Sms.Components
{
    class SmsManager : AppComponent
    {
        private readonly SmsMessenger_China smsChina;
        private readonly SmsMessenger_Others smsOthers;

        public SmsManager(Service service)
            : base(service)
        {
            this.myService = service;

            this.smsChina = new SmsMessenger_China(service);
            this.smsOthers = new SmsMessenger_Others(service);
        }

        private readonly Service myService;

        protected override void OnStart()
        {
            base.OnStart();

            //LogManager.LogMessage("Sms Start...");
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        public Dictionary<string,string> Send(string[] phoneNumbers, string content)
        {
            Dictionary<string, string> retDict = null;
            if (phoneNumbers.Length == 0)
                return null;

            string phoneNum = phoneNumbers[0];
            if (String.IsNullOrWhiteSpace(phoneNum))
                return null;

            string[] phoneSegs = phoneNum.Split(new char[] { ' ' });
            string countryCode = phoneSegs[0];

            switch (countryCode)
            {
                case "86":
                    retDict = smsChina.Send(phoneNumbers, content);
                    break;
                default:
                    retDict = smsOthers.Send(phoneNumbers, content);
                    break;
            }

            return retDict;
            
        }

        public static string GetHtmlFromUrl(string url)
        {
            string strRet = String.Empty;

            if (String.IsNullOrWhiteSpace(url))
            {
                return strRet;
            }

            string targetUrl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targetUrl);
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.UTF8);
                strRet = ser.ReadToEnd();
            }
            catch { }

            return strRet;
        }

        public static Dictionary<string, string> Parse(string strData)
        {
            if (strData == null)
                return null;

            string[] arrStrData = strData.Split(new char[] { '&' });
            if (arrStrData.Length == 0)
                return null;

            Dictionary<string, string> retDict = new Dictionary<string, string>();
            foreach (string pairStr in arrStrData)
            {
                string[] arrPairStr = pairStr.Split(new char[] { '=' });
                if (arrPairStr.Length == 2)
                {
                    retDict.Add(arrPairStr[0], arrPairStr[1]);
                }
            }

            return retDict;
        }
    }
}
