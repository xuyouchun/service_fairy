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

namespace TestKeepConnection
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
        public static ReplyWrapper<TReply> SendHttpRequest<TRequest, TReply>(string address, TRequest requestObj, string method, string desc = "", string sid = "")
        {
            string reqStr = Utility.JsonSerialize(new RequestWrapper<TRequest> { Method = method, Body = requestObj, SID = sid });

            string rspStr = SendHttpRequest(address, reqStr);
            var w = Utility.JsonDeserialize<ReplyWrapper<TReply>>(rspStr);
            if (!_Success(w.StatusCode))
                throw new ServiceException(w.StatusDesc, (ServiceStatusCode)w.StatusCode, (ushort)w.StatusReason);

            return w;
        }

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
                StreamReader sr = new StreamReader(rspStream, Encoding.UTF8);
                string rspStr = sr.ReadToEnd();

                return rspStr;
            }
        }

        public static void SendSocketRequest<TRequest>(this TcpClient tc, TRequest requestObj, string method, string sid = "", string action = "w")
        {
            string reqStr = Utility.JsonSerialize(new RequestWrapper<TRequest> { Method = method, Body = requestObj, SID = sid, Action = action });
            SendSocketRequest(tc, reqStr);
        }

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

        private static void _Record(string json, string desc)
        {
            File.AppendAllText(@"d:\System.User接口的请求应答参数.txt",
                desc + "\r\n-------------------------------------------------\r\n" + json + "\r\n\r\n\r\n");
        }


        // 获取代理列表
        public static CommunicationOption GetProxyList(string navAddress, CommunicationType communicationType, bool supportDuplex = false)
        {
            Navigation_GetProxyList_Request req = new Navigation_GetProxyList_Request() {
                CommunicationType = communicationType, MaxCount = 10, Direction = supportDuplex ? CommunicationDirection.Bidirectional : CommunicationDirection.Unidirectional
            };

            ReplyWrapper<Navigation_GetProxyList_Reply> reply
                = Utility.SendHttpRequest<Navigation_GetProxyList_Request, Navigation_GetProxyList_Reply>(navAddress, req, "System.Navigation/GetProxyList", "获取代理列表");

            CommunicationOption[] proxyList = reply.Body.CommunicationOptions;
            if (proxyList.Length == 0)
            {
                throw new Exception("无可用代理");
            }

            return proxyList[0];
        }
    }
}
