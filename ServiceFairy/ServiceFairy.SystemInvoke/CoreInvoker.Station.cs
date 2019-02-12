using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package.Service;
using ServiceFairy.Entities;
using ServiceFairy.Client;
using ServiceFairy.Entities.Station;

namespace ServiceFairy.SystemInvoke
{
	partial class CoreInvoker
	{
        private StationInvoker _station;

        /// <summary>
        /// Station Services
        /// </summary>
        public StationInvoker Station
        {
            get { return _station ?? (_station = new StationInvoker(this)); }
        }

        public class StationInvoker : Invoker
        {
            public StationInvoker(CoreInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 服务终端的心跳逻辑
            /// </summary>
            /// <param name="clientInfo">终端信息</param>
            /// <param name="caller">调用者</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Station_ClientHeartBeat_Reply> ClientHeartBeatSr(AppClientInfo clientInfo, AppServiceCookieCollection cookies, ServiceDesc caller, Guid clientId, CallingSettings settings = null)
            {
                return StationService.ClientHeartBeat(Sc, new Station_ClientHeartBeat_Request() {
                    AppClientInfo = clientInfo, Cookies = cookies, Caller = caller, ClientID = clientId
                }, settings);
            }

            /// <summary>
            /// 服务终端的心跳逻辑
            /// </summary>
            /// <param name="clientInfo">终端信息</param>
            /// <param name="caller">调用者</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Station_ClientHeartBeat_Reply ClientHeartBeat(AppClientInfo clientInfo, AppServiceCookieCollection cookies, ServiceDesc caller, Guid clientId, CallingSettings settings = null)
            {
                return InvokeWithCheck(ClientHeartBeatSr(clientInfo, cookies, caller, clientId, settings));
            }

            /// <summary>
            /// 激发事件
            /// </summary>
            /// <param name="source">事件源</param>
            /// <param name="eventName">事件名称</param>
            /// <param name="eventArgs">事件参数</param>
            /// <param name="enableRoute">是否通知其它的Register服务，继续分发该事件</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RaiseEventSr(ServiceEndPoint source, string eventName, byte[] eventArgs, bool enableRoute, CallingSettings settings = null)
            {
                Contract.Requires(source != null && !string.IsNullOrEmpty(eventName));

                return StationService.RaiseEvent(Sc, new Station_RaiseEvent_Request {
                    Caller = source.ServiceDesc, ClientID = source.ClientId, EventArgs = eventArgs, EventName = eventName, EnableRoute = enableRoute
                }, settings ?? CallingSettings.OneWay);
            }

            /// <summary>
            /// 激发事件
            /// </summary>
            /// <param name="source">事件源</param>
            /// <param name="eventName">事件名称</param>
            /// <param name="eventArgs">事件参数</param>
            /// <param name="enableRoute">是否通知其它的Register服务，继续分发该事件</param>
            /// <param name="settings">调用设置</param>
            public void RaiseEvent(ServiceEndPoint source, string eventName, byte[] eventArgs, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(RaiseEventSr(source, eventName, eventArgs, enableRoute, settings));
            }

            /// <summary>
            /// 注册插件
            /// </summary>
            /// <param name="relations">关联</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RegisterAddinsSr(ServiceAddinRelations[] relations, CallingSettings settings = null)
            {
                return StationService.RegisterAddins(Sc, new Station_RegisterAddins_Request { Relations = relations }, settings);
            }

            /// <summary>
            /// 注册插件
            /// </summary>
            /// <param name="relations"></param>
            /// <param name="settings"></param>
            public void RegisterAddins(ServiceAddinRelations[] relations, CallingSettings settings = null)
            {
                InvokeWithCheck(RegisterAddinsSr(relations, settings));
            }
        }
	}
}
