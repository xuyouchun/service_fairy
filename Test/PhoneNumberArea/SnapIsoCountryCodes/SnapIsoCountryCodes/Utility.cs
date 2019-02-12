using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Common.Mobile;
using Common.Utility;

namespace SnapIsoCountryCodes
{
    static class Utility
    {
        public static string GetFromUrl(string url, Encoding encoding)
        {
            WebClient wc = new WebClient();
            return encoding.GetString(wc.DownloadData(url));
        }

        public static string GetOpCode(this OperationDetail od)
        {
            return StringUtility.GetFirstNotNullOrWhiteSpaceString(od.Brand, od.OpName);
        }
    }
}
