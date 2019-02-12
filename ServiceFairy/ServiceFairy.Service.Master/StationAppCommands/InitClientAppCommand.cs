using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Framework.TrayPlatform;
using System.Net;
using Common.Utility;
using ServiceFairy.Client;
using ServiceFairy.Entities.Master;
using Common.Communication.Wcf;
using Common;

namespace ServiceFairy.Service.Master.StationAppCommands
{
    /// <summary>
    /// 客户端的初始化
    /// </summary>
    [AppCommand("InitClient", "新服务终端的初始化", SecurityLevel = SecurityLevel.Public)]
    class InitClientAppCommand : ACS<Service>.Func<Master_InitClient_Request, Master_InitClient_Reply>
    {
        protected override Master_InitClient_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_InitClient_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            AppClientDeployInfo dInfo = null;
            string ip;

            if (context.IsLocalCall)
            {
                srv.DeployMapManager.Adjust(new AppClientAdjustInfo() {
                    ClientID = req.ClientID,
                    ServicesToStart = req.InitServices, CommunicationsToOpen = req.InitCommunicationOptions
                });

                ip = "127.0.0.1";
            }
            else
            {
                ip = context.CommunicateContext.From.Address;
                lock (string.Intern("InitClient_" + ip))
                {
                    dInfo = srv.DeployMapManager.GetDeployInfo(req.ClientID);
                    if (dInfo == null)
                    {
                        srv.DeployMapManager.AddDeployInfo(dInfo = new AppClientDeployInfo() {
                            ClientId = req.ClientID, Services = req.InitServices.ToArray(v => new AppServiceDeployInfo(v)),
                            CommunicateOptions = _GetCommunicationOptions(srv, ip, req.InitCommunicationOptions),
                        });
                    }
                    else
                    {
                        srv.DeployMapManager.Adjust(new AppClientAdjustInfo() {
                            ClientID = req.ClientID,
                            ServicesToStart = req.InitServices,
                            CommunicationsToOpen = _GetCommunicationOptionsToOpen(srv, ip, req.InitCommunicationOptions, dInfo.CommunicateOptions)
                        });
                    }
                }
            }

            dInfo = dInfo ?? srv.DeployMapManager.GetDeployInfo(req.ClientID);
            srv.AppClientManager.UpdateClientInfo(req.AppClientInfo);
            return new Master_InitClient_Reply() {
                MasterClientID = srv.Context.ClientID,
                InvokeInfos = new[] {
                    new AppInvokeInfo(srv.Context.ClientID, srv.Context.Platform.GetAllServiceInfos().ToArray(si => si.ServiceDesc),
                        srv.Context.Platform.GetAllCommunicationOptions())
                }
            };
        }

        // 获取需要开启的信道，需要至少有一个WTcp的单向调用方式
        private CommunicationOption[] _GetCommunicationOptions(Service svr, string ip, IList<CommunicationOption> ops)
        {
            List<CommunicationOption> opList = new List<CommunicationOption>(ops ?? Array<CommunicationOption>.Empty);
            if (!opList.Any(op => op.Type == CommunicationType.WTcp && !op.Duplex))
            {
                opList.Add(new CommunicationOption(_GetServiceAddress(svr, ip), CommunicationType.WTcp, false));
            }

            return opList.ToArray();
        }

        // 获取需要开启的信道，需要至少有一个WTcp的单向调用方式
        private CommunicationOption[] _GetCommunicationOptionsToOpen(Service srv, string ip, IList<CommunicationOption> initOps, IList<CommunicationOption> existsOps)
        {
            List<CommunicationOption> opList = new List<CommunicationOption>(initOps ?? Array<CommunicationOption>.Empty);
            opList.AddRange(existsOps ?? Array<CommunicationOption>.Empty);

            CommunicationOption[] ops = _GetCommunicationOptions(srv, ip, opList);
            if (!existsOps.IsNullOrEmpty())
                ops = ops.Except(existsOps).ToArray();

            return ops;
        }

        private ServiceAddress _GetServiceAddress(Service srv, string ip)
        {
            HashSet<int> usedPorts = srv.DeployMapManager.SearchPortsByIp(ip).ToHashSet();
            for (int port = Settings.StartPort, end = Settings.EndPort; port <= end; port++)
            {
                if (!usedPorts.Contains(port))
                    return new ServiceAddress(ip, port);
            }

            throw new ServiceException(ServiceStatusCode.ServerError, "在指定范围内没有可用的端口号");
        }
    }
}
