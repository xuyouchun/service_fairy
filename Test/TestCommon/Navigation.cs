using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using ServiceFairy.Entities.Navigation;
using Common.Package.Service;
using Common.Contracts;
using ServiceFairy;

namespace TestCommon
{
    public static class Navigation
    {
        /// <summary>
        /// 以HTTP方式获取代理
        /// </summary>
        /// <param name="navigation">导航地址（HTTP方式）</param>
        /// <param name="type">要获取的代理通信方式（TCP、HTTP、SOCKET）</param>
        /// <param name="duplex">代理通信方向：单向或双向</param>
        /// <returns></returns>
        public static CommunicationOption GetProxyByHttp(string navigation, CommunicationType type, bool duplex)
        {
            Navigation_GetProxyList_Request req = new Navigation_GetProxyList_Request() {
                CommunicationType = type, MaxCount = 10, Direction = duplex ? CommunicationDirection.Bidirectional : CommunicationDirection.Unidirectional
            };

            ReplyWrapper<Navigation_GetProxyList_Reply> reply =
                TestUtility.SendHttpRequest<Navigation_GetProxyList_Request, Navigation_GetProxyList_Reply>(navigation, req, "System.Navigation/GetProxyList");

            return reply.Body.CommunicationOptions.FirstOrDefault();
        }
    }
}
