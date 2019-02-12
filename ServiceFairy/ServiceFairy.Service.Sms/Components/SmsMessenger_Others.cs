using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ServiceFairy.Service.Sms.Components
{
    class SmsMessenger_Others : SmsMessenger
    {
        public SmsMessenger_Others(Service service)
            : base(service)
        {
        }

        private const string SmsUserName = "lmz";
        private const string SmsUserPassword = "CA5397AD84CE686284D8D9752245F676";

        public Dictionary<string, string> Send(string[] mobileNumbers, string msgContent)
        {
            StringBuilder sbUrl = new StringBuilder("http://58.64.162.28/ensms/servlet/WebSend");

            List<string> phoneNumbers = new List<string>();
            foreach (string pn in mobileNumbers)
            {
                string[] pnSegs = pn.Split(new char[] { ' ' });
                if (pnSegs.Length > 1)
                {
                    StringBuilder sbMobile = new StringBuilder();
                    for (int i = 1; i < pnSegs.Length; i++)
                    {
                        sbMobile.Append(pnSegs[i].Replace("(", String.Empty).Replace(")", String.Empty));
                    }

                    phoneNumbers.Add("+" + pnSegs[0] + sbMobile.ToString().TrimStart(new char[] { '0' }));
                }
            }

            string myMobiles = String.Join(",", phoneNumbers.ToArray());
            sbUrl.Append("?userId=").Append(HttpUtility.UrlDecode(SmsUserName))
                .Append("&password=").Append(SmsUserPassword)
                .Append("&content=").Append(HttpUtility.UrlDecode(msgContent))
                .Append("&mobile=").Append(HttpUtility.UrlDecode(myMobiles))
                .Append("&srcName=");

            string smsResult = SmsManager.GetHtmlFromUrl(sbUrl.ToString());
            this.AddSmsLog(myMobiles, msgContent, smsResult);

            return SmsManager.Parse(smsResult);
        }
    }
}
