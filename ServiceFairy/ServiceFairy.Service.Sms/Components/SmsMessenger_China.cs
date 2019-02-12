using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ServiceFairy.Service.Sms.Components
{
    class SmsMessenger_China : SmsMessenger
    {
        public SmsMessenger_China(Service service)
            : base(service)
        {
        }

        private const string SmsUserName = "zswx-0736";
        private const string SmsUserPassword = "12qw34er@2012";

        public Dictionary<string, string> Send(string[] mobileNumbers, string msgContent)
        {
            StringBuilder sbUrl = new StringBuilder("http://sdk.send1.net/Service.asmx/SendSMS");

            List<string> phoneNumbers = new List<string>();
            foreach (string pn in mobileNumbers)
            {
                string[] pnSegs = pn.Split(new char[] { ' ' });
                if (pnSegs.Length == 2)
                {
                    phoneNumbers.Add(pnSegs[1]);
                }
            }

            string myMobiles = String.Join(",", phoneNumbers.ToArray());
            sbUrl.Append("?regnum=").Append(HttpUtility.UrlDecode(SmsUserName))
                .Append("&pwd=").Append(HttpUtility.UrlDecode(SmsUserPassword))
                .Append("&content=").Append(HttpUtility.UrlDecode(msgContent))
                .Append("&phone=").Append(HttpUtility.UrlDecode(myMobiles))
                .Append("&time=");

            string smsResult = SmsManager.GetHtmlFromUrl(sbUrl.ToString());
            this.AddSmsLog(myMobiles, msgContent, smsResult);

            return SmsManager.Parse(smsResult);
        }
        
    }
}
