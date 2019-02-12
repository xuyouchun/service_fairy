using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Navigation;
using Common.Communication.Wcf;
using ServiceFairy.Entities.Proxy;
using Common;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private ProxyInvoker _proxy;

        /// <summary>
        /// Proxy Service
        /// </summary>
        public ProxyInvoker Proxy
        {
            get { return _proxy ?? (_proxy = new ProxyInvoker(this)); }
        }

        /// <summary>
        /// Proxy Service
        /// </summary>
        public class ProxyInvoker : Invoker
        {
            public ProxyInvoker(SystemInvoker owner)
                : base(owner)
            {

            }

            /// <summary>
            /// 获取在线用户
            /// </summary>
            /// <remarks>需要明确指定哪个终端</remarks>
            /// <param name="sortField">排序字段</param>
            /// <param name="sortType">排序方式</param>
            /// <param name="start">起始</param>
            /// <param name="count">数量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ProxyOnlineUserInfo[]> GetOnlineUsersSr(GetOnlineUsersSortField sortField = GetOnlineUsersSortField.ConnectionTime,
                SortType sortType = SortType.Desc, int start = 0, int count = int.MaxValue, CallingSettings settings = null)
            {
                Proxy_GetOnlineUsers_Request request = new Proxy_GetOnlineUsers_Request() {
                    SortField = sortField, SortType = sortType, Start = start, Count = count,
                };

                var sr = ProxyService.GetOnlineUsers(Sc, request, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取在线用户
            /// </summary>
            /// <remarks>需要明确指定哪个终端</remarks>
            /// <param name="sortField">排序字段</param>
            /// <param name="sortType">排序方式</param>
            /// <param name="start">起始</param>
            /// <param name="count">数量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ProxyOnlineUserInfo[] GetOnlineUsers(GetOnlineUsersSortField sortField = GetOnlineUsersSortField.ConnectionTime,
                SortType sortType = SortType.Desc, int start = 0, int count = int.MaxValue, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetOnlineUsersSr(sortField, sortType, start, count, settings));
            }

            /// <summary>
            /// 设置终端设置的信息
            /// </summary>
            /// <param name="deviceName">设备名称</param>
            /// <param name="osVersion">操作系统版本号</param>
            /// <param name="supportedDataFormats">支持的数据编码方式</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult InitSr(string deviceName, string osVersion, DataFormat[] supportedDataFormats,
                CallingSettings settings = null)
            {
                return ProxyService.Init(Sc, new Proxy_Init_Request {
                    DeviceName = deviceName, OsVersion = osVersion, SupportedDataFormats = supportedDataFormats
                }, settings);
            }

            /// <summary>
            /// 设置终端设置的信息
            /// </summary>
            /// <param name="deviceName">设备名称</param>
            /// <param name="osVersion">操作系统版本号</param>
            /// <param name="supportedDataFormats">支持的数据编码方式</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void Init(string deviceName, string osVersion, DataFormat[] supportedDataFormats,
                CallingSettings settings = null)
            {
                InvokeWithCheck(InitSr(deviceName, osVersion, supportedDataFormats, settings));
            }
        }
    }
}
