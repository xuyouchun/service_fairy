using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using Common.Contracts.Service;
using Common.Package;

namespace TestLog
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
        public static ReplyWrapper<TReply> SendRequest<TRequest, TReply>(string address, TRequest requestObj, string method, string desc = "", string sid = "")
        {
            string reqStr = Utility.JsonSerialize(new RequestWrapper<TRequest> { Method = method, Body = requestObj, SID = sid });

            string rspStr = SendRequest(address, reqStr);
            var w = Utility.JsonDeserialize<ReplyWrapper<TReply>>(rspStr);
            if (!_Success(w.StatusCode))
                throw new ServiceException(w.StatusDesc, (ServiceStatusCode)w.StatusCode, (ushort)w.StatusReason);

            return w;
        }

        public static string SendRequest(string address, string reqJson)
        {
            string url = "http://" + address + "/s";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";

            using (Stream stream = req.GetRequestStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(reqJson ?? "");
                stream.Write(bytes, 0, bytes.Length);
            }

            using (HttpWebResponse rsp = (HttpWebResponse)req.GetResponse())
            using (Stream rspStream = rsp.GetResponseStream())
            {
                StreamReader sr = new StreamReader(rspStream, Encoding.UTF8);
                string rspStr = sr.ReadToEnd();

                return rspStr;
            }
        }

        private static bool _Success(int statusCode)
        {
            return statusCode <= 299;
        }

        private static void _Record(string json, string desc)
        {
            File.AppendAllText(@"d:\System.User接口的请求应答参数.txt",
                desc + "\r\n-------------------------------------------------\r\n" + json + "\r\n\r\n\r\n");
        }

        public static void WriteLog(Exception error)
        {
            Console.WriteLine(error.ToString());
            LogManager.LogError(error);
        }

        public static void WriteLog(string s)
        {
            Console.WriteLine(s);
            LogManager.LogMessage(s);
        }
    }
}
