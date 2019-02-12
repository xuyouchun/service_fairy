using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;

namespace FakeMobileClient
{
    public static class Utility
    {
        public static string JsonSerialize(object obj)
        {
            if (obj == null)
                return "";

            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(ms, obj);
            ms.Flush();

            return Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
        }

        public static T JsonDeserialize<T>(string s)
        {
            if (string.IsNullOrEmpty(s))
                return default(T);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(s));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            return (T)ser.ReadObject(ms);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TReply"></typeparam>
        /// <param name="address"></param>
        /// <param name="requestObj"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static ReplyWrapper<TReply> SendRequest<TRequest, TReply>(string address, TRequest requestObj, string method)
        {
            string url = "http://" + address + "/s";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            string reqStr = Utility.JsonSerialize(new RequestWrapper<TRequest> { Method = method, Body = requestObj });
            using (Stream stream = req.GetRequestStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(reqStr ?? "");
                stream.Write(bytes, 0, bytes.Length);
            }

            using (HttpWebResponse rsp = (HttpWebResponse)req.GetResponse())
            using (Stream rspStream = rsp.GetResponseStream())
            {
                StreamReader sr = new StreamReader(rspStream, Encoding.UTF8);
                string rspStr = sr.ReadToEnd();

                var w = Utility.JsonDeserialize<ReplyWrapper<TReply>>(rspStr);
                if (w.StatusCode != 200 && w.StatusCode != 0)
                    throw new ApplicationException(w.StatusDesc);

                return w;
            }
        }
    }
}
