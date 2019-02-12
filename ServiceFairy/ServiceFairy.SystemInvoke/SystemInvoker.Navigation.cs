using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Navigation;
using Common.Communication.Wcf;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private NavigationInvoker _navigation;

        /// <summary>
        /// Navigation Service
        /// </summary>
        public NavigationInvoker Navigation
        {
            get { return _navigation ?? (_navigation = new NavigationInvoker(this)); }
        }

        /// <summary>
        /// Navigation Service
        /// </summary>
        public class NavigationInvoker : Invoker
        {
            public NavigationInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 获取代理列表
            /// </summary>
            /// <param name="maxCount">最大数量</param>
            /// <param name="type">通信类型</param>
            /// <param name="direction">通信方向</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<CommunicationOption[]> GetProxyListSr(int maxCount, CommunicationType type,
                CommunicationDirection direction = CommunicationDirection.Unidirectional, CallingSettings settings = null)
            {
                var sr = NavigationService.GetProxyList(Sc,
                    new Navigation_GetProxyList_Request() { MaxCount = maxCount, CommunicationType = type, Direction = direction }, settings);

                return CreateSr(sr, r => r.CommunicationOptions);
            }

            /// <summary>
            /// 获取代理列表
            /// </summary>
            /// <param name="maxCount">最大数量</param>
            /// <param name="type">通信类型</param>
            /// <param name="duplex">通信方向</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public CommunicationOption[] GetProxyList(int maxCount, CommunicationType type,
                CommunicationDirection direction = CommunicationDirection.Unidirectional, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetProxyListSr(maxCount, type, direction, settings));
            }

            /// <summary>
            /// 获取代理地址
            /// </summary>
            /// <param name="type">通信类型</param>
            /// <param name="duplex">是否支持双向通信</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<string> GetProxySr(CommunicationType type, bool duplex, CallingSettings settings = null)
            {
                var sr = NavigationService.GetProxy(Sc, new Navigation_GetProxy_Request {
                    CommunicationType = type, Duplex = duplex,
                }, settings);

                return CreateSr(sr, r => r.Proxy);
            }

            /// <summary>
            /// 获取代理地址
            /// </summary>
            /// <param name="type">通信类型</param>
            /// <param name="duplex">是否支持双向通信</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public string GetProxy(CommunicationType type, bool duplex, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetProxySr(type, duplex, settings));
            }

            /// <summary>
            /// 获取代理地址的Url
            /// </summary>
            /// <param name="type">通信类型</param>
            /// <param name="duplex">是否支持双向通信</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<string> GetProxyUrlSr(CommunicationType type, bool duplex, CallingSettings settings = null)
            {
                var sr = NavigationService.GetProxyUrl(Sc, new Navigation_GetProxyUrl_Request {
                    CommunicationType = type, Duplex = duplex,
                }, settings);

                return CreateSr(sr, r => r.ProxyUrl);
            }

            /// <summary>
            /// 获取代理地址的Url
            /// </summary>
            /// <param name="type">通信类型</param>
            /// <param name="duplex">是否支持双向通信</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public string GetProxyUrl(CommunicationType type, bool duplex, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetProxyUrlSr(type, duplex, settings));
            }
        }
    }
}
