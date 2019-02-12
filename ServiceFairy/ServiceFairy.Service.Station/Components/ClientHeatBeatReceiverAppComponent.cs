using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Client;
using ServiceFairy.Components;
using Common.Package;
using Common.Package.Serializer;
using Common.Contracts;
using Common.Utility;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities;
using Common;
using ServiceFairy.Entities.Station;

namespace ServiceFairy.Service.Station.Components
{
    /// <summary>
    /// 终端心跳接收器
    /// </summary>
    [AppComponent("终端的心跳接收器", "收集各个终端通过心跳发来的数据")]
    class ClientHeatBeatReceiverAppComponent : AppComponent
    {
        public ClientHeatBeatReceiverAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 接收到新的心跳
        /// </summary>
        /// <param name="appClientInfo"></param>
        /// <param name="cookieCollection"></param>
        public void NewHeartBeat(AppClientInfo appClientInfo, AppServiceCookieCollection cookieCollection)
        {
            // 保存服务终端的状态
            _service.AppClientInfoManager.UpdateClientInfo(appClientInfo);
            
            //　服务终端的Cookie
            if (cookieCollection != null && cookieCollection.Cookies != null)
            {
                foreach (AppServiceCookie cookie in cookieCollection.Cookies)
                {
                    switch (cookie.DataType)
                    {
                        case ServiceCookieNames.EVENT_REGISTER:
                            _RegisterEvent(cookie);
                            break;

                        case ServiceCookieNames.ADDIN_REGISTER:
                            _RegisterAddin(cookie);
                            break;
                    }
                }
            }
        }

        // 注册事件
        private void _RegisterEvent(AppServiceCookie cookie)
        {
            try
            {
                RegisterEventCookieData cookieData
                    = SerializerUtility.Deserialize<RegisterEventCookieData>(DataFormat.Unknown, cookie.Data);

                _service.ServiceEventManager.Register(cookie.EndPoint, cookieData.Items.ToArray(item => item.EventName));
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        // 注册插件
        private void _RegisterAddin(AppServiceCookie cookie)
        {
            try
            {
                ServiceAddinCookieData cookieData
                    = SerializerUtility.Deserialize<ServiceAddinCookieData>(DataFormat.Unknown, cookie.Data);

                _service.ServiceAddinManager.Register(cookie.EndPoint, cookieData.Relations ?? Array<ServiceAddinRelation>.Empty);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        /// <summary>
        /// 根据目标寻找插件
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public ServiceAddinRelation[] GetAddinRelactionsOfTarget(ServiceDesc target)
        {
            Contract.Requires(target != null);

            return _service.ServiceAddinManager.GetByTarget(target);
        }

        /// <summary>
        /// 根据目标寻找插件
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        public ServiceAddinRelation[] GetAddinRelactionsOfTargets(ServiceDesc[] targets)
        {
            Contract.Requires(targets != null);

            return _service.ServiceAddinManager.GetByTargets(targets);
        }
    }
}
