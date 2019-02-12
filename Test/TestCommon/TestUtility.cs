using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using Common.Contracts.Service;
using Common.Communication.Wcf;
using ServiceFairy.Entities.Navigation;
using System.Net.Sockets;
using Common.Utility;

namespace TestCommon
{
    public static class TestUtility
    {
        /// <summary>
        /// 将对象进行Json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerialize(object obj)
        {
            if (obj == null)
                return "";

            return JsonUtility.SerializeToString(obj);
        }

        /// <summary>
        /// 将对象进行Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string s)
        {
            if (string.IsNullOrEmpty(s))
                return default(T);

            return JsonUtility.Deserialize<T>(s);
        }

        /// <summary>
        /// 以HTTP方式发送请求
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TReply"></typeparam>
        /// <param name="address"></param>
        /// <param name="requestObj"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static ReplyWrapper<TReply> SendHttpRequest<TRequest, TReply>(string address, TRequest requestObj, string method, string desc = "", string sid = "")
        {
            string reqStr = TestUtility.JsonSerialize(new RequestWrapper<TRequest> { Method = method, Body = requestObj, SID = sid });
            Console.WriteLine("Send:" + reqStr);

            string rspStr = SendHttpRequest(address, reqStr);
            Console.WriteLine("Receive:" + rspStr);

            var w = TestUtility.JsonDeserialize<ReplyWrapper<TReply>>(rspStr);
            if (!_Success(w.StatusCode))
                throw new ServiceException(w.StatusDesc, (ServiceStatusCode)w.StatusCode);

            return w;
        }

        /// <summary>
        /// 以HTTP方式发送请求
        /// </summary>
        /// <param name="address"></param>
        /// <param name="reqJson"></param>
        /// <returns></returns>
        public static string SendHttpRequest(string address, string reqJson)
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
                var statusCode = rsp.StatusCode;

                StreamReader sr = new StreamReader(rspStream, Encoding.UTF8);
                string rspStr = sr.ReadToEnd();

                return rspStr;
            }
        }

        /// <summary>
        /// 以Socket方式发送请求
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="tc"></param>
        /// <param name="requestObj"></param>
        /// <param name="method"></param>
        /// <param name="sid"></param>
        /// <param name="action"></param>
        public static void SendSocketRequest<TRequest>(this TcpClient tc, TRequest requestObj, string method, string sid = "", string action = "w")
        {
            string reqStr = TestUtility.JsonSerialize(new RequestWrapper<TRequest> { Method = method, Body = requestObj, SID = sid, Action = action });
            SendSocketRequest(tc, reqStr);
        }

        /// <summary>
        /// 以Socket方式发送请求
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="reqJson"></param>
        public static void SendSocketRequest(this TcpClient tc, string reqJson)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(reqJson);
            NetworkStream ns = tc.GetStream();

            ns.Write(bytes.Length);
            ns.Write(bytes);

            ns.Flush();
        }

        private static bool _Success(int statusCode)
        {
            return statusCode <= 299;
        }
    }
}
