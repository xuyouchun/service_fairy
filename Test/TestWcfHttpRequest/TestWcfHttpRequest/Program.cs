using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Common.Utility;

namespace TestWcfHttpRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:80/s");
            req.Method = "POST";

            using (Stream reqStream = req.GetRequestStream())
            {
                RequestEntity reqEntity = new RequestEntity(1234, "my god!");

                string s = "{\"method\":\"TestService/MyMethod\",\"body\":";
                s += JsonUtility.Serialize(reqEntity);
                s += "}";

                byte[] buffer = Encoding.UTF8.GetBytes(s);
                reqStream.Write(buffer);
            }

            using (HttpWebResponse rsp = (HttpWebResponse)req.GetResponse())
            {
                using (Stream rspStream = rsp.GetResponseStream())
                {
                    ReplyEntity replyEntity = JsonUtility.Deserialize<ReplyEntity>(rspStream);

                    return;
                }
            }
        }
    }
}
