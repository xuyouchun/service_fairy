using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Framework.TrayPlatform;
using Common.Package.Service;
using ServiceFairy.Entities.Station;
using ServiceFairy.Entities.Master;
using Common;
using Common.Utility;
using ServiceFairy.Components;
using Common.Package.Serializer;
using Common.Contracts;
using ServiceFairy.Client;

namespace ServiceFairy.Service.Station
{
    /// <summary>
    /// 响应服务终端的心跳
    /// </summary>
    [AppCommand("ClientHeartBeat", "响应服务终端的心跳", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class ClientHeartBeatAppCommand : ACS<Service>.Func<Station_ClientHeartBeat_Request, Station_ClientHeartBeat_Reply>
    {
        protected override Station_ClientHeartBeat_Reply OnExecute(AppCommandExecuteContext<Service> context, Station_ClientHeartBeat_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            srv.ClientHeatBeatReceiver.NewHeartBeat(req.AppClientInfo, req.Cookies);

            // 返回配置文件与部署策略的版本
            var mgr = srv.ServiceVersionManager;
            var cfgVers = mgr.GetConfigurationVersions(
                (req.AppClientInfo.ServiceInfos ?? Array<ServiceInfo>.Empty).ToArray(si => si.ServiceDesc));

            DateTime deployVer = mgr.GetDeployVersion(req.ClientID);
            AppServiceCookieCollection replyCookies = _GetReplyCookies(srv, req.AppClientInfo);

            return new Station_ClientHeartBeat_Reply() {
                DeployVersion = deployVer, ConfigurationVersionPairs = cfgVers, Cookies = replyCookies,
            };
        }

        private AppServiceCookieCollection _GetReplyCookies(Service srv, AppClientInfo clientInfo)
        {
            List<AppServiceCookie> cookies = new List<AppServiceCookie>();

            cookies.AddRange(clientInfo.ServiceInfos.Select(si => new AppServiceCookie() {
                DataType = ServiceCookieNames.ADDIN_LIST,
                Data = SerializerUtility.SerializeToBytes(DataFormat.Unknown,
                    srv.ClientHeatBeatReceiver.GetAddinRelactionsOfTarget(si.ServiceDesc)
                ),
                EndPoint = new ServiceEndPoint(Guid.Empty, si.ServiceDesc)
            }));

            return new AppServiceCookieCollection() { Cookies = cookies.ToArray() };
        }
    }
}
