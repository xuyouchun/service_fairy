using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using System.Threading;

namespace MobileAreaTestData
{
    public static class GenerateTestDataUtility
    {
        public static void Run()
        {
            string[] ops = "134,135,136,137,138,139,150,151,152,157,158,159,182,187,188,147,130,131,132,155,156,185,186,133,153,180,189,145,183".Split(',');

            Random r = new Random();

            string path = @"D:\Work\Data\PhoneNumberAreaData\2012-05-09\PhoneNumberArea.txt";
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                foreach (string op in ops)
                {
                    for (int k = 0; k < 10000; k++)
                    {
                        //string part = r.Next(10000).ToString().PadLeft(4, '0');
                        string part = k.ToString().PadLeft(4, '0');
                        string mobile = op + part + "0000";

                        string operation, province, city;
                        if (_GetAreaInfo(mobile, out operation, out province, out city))
                        {
                            string s = string.Format("{0} {1} {2} {3}", mobile, operation, province, city);
                            Console.WriteLine(s);
                            sw.WriteLine(s);
                        }
                    }
                }
            }
        }

        static bool _GetAreaInfo(string mobile, out string operation, out string province, out string city)
        {
            operation = province = city = "";

            for (int tryTimes = 1; ; tryTimes++)
            {
                try
                {
                    string xml = _GetAreaInfo(mobile);
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xml);

                    if (xDoc.DocumentElement.ChildNodes.Count == 0)
                        return false;

                    operation = xDoc.DocumentElement.SelectSingleNode("/alldata/data/type").InnerText.Split(' ')[0].Trim();
                    if (operation.StartsWith("中国"))
                        operation = operation.Substring(2);
                    province = xDoc.DocumentElement.SelectSingleNode("/alldata/data/prov").InnerText.Split('(')[0].Trim();
                    city = xDoc.DocumentElement.SelectSingleNode("/alldata/data/city").InnerText.Split('(')[0].Trim();
                    return true;
                }
                catch (WebException ex)
                {
                    if (tryTimes >= 3)
                        return false;

                    Console.WriteLine(ex.ToString());
                    Thread.Sleep(10 * 000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
        }

        static string _GetAreaInfo(string mobile)
        {
            string url = string.Format("http://opendata.baidu.com//api.php?query={0}&co=&resource_id=6004&t=1324009053904&ie=utf8&oe=gbk&cb=bd__cbs__se2a2a&format=xml&tn=baidu", mobile);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Referer = "http://www.baidu.com/";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; BRI/2; InfoPath.3; CIBA; .NET4.0C; .NET4.0E)";

            using (HttpWebResponse rsp = (HttpWebResponse)req.GetResponse())
            using (Stream stream = rsp.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream, Encoding.Default);
                return sr.ReadToEnd();
            }
        }
    }
}
