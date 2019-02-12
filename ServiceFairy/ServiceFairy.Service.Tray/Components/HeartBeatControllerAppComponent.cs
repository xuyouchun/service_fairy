using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Utility;
using Common;
using ServiceFairy.Client;
using Common.Framework.TrayPlatform;
using ServiceFairy.Entities.Master;
using ServiceFairy.Entities.Station;
using System.Diagnostics;

namespace ServiceFairy.Service.Tray.Components
{
    /// <summary>
    /// 服务终端的心跳控制器
    /// </summary>
    [AppComponent("服务终端的心跳控制器", "向“" + SFNames.ServiceNames.Station + "”服务定期发送心跳，以维持与中心服务的链接")]
    class HeartBeatControllerAppComponent : TimerAppComponentBase
    {
        public HeartBeatControllerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(2))
        {
            _service = service;
            _clientInfoCache = new SingleMemoryCache<ClientInfoWrapper>(TimeSpan.FromMinutes(1), _GetClientInfo);
        }

        private readonly Service _service;
        private DateTime _lastDeployVersion = default(DateTime);
        private volatile bool _hasInit = false;
        private volatile bool _clientAvalidate = false;

        protected override void OnExecuteTask(string taskName)
        {
            if (!_hasInit)
            {
                if (!_Init())
                    return;

                _hasInit = true;
                LogManager.LogMessage("已连接到中心服务器");
                _service.ServiceEvent.Raise(ServiceEventNames.EVENT_CLIENT_CONNECTED, null);
            }

            _HeartBeat();
        }

        /// <summary>
        /// 向服务器询问初始化的信息
        /// </summary>
        private bool _Init()
        {
            return AutoRetry((Func<bool>)_TryInit, TimeSpan.FromSeconds(10), 3);
        }

        private bool _TryInit()
        {
            try
            {
                Master_InitClient_Reply r = _service.Invoker.Master.InitClient(_service.Context.ServiceEndPoint,
                    _GetAppClientInfo(),
                    _service.Context.Platform.GetAllServices().Union(ClientUtility.LoadInitServices(_service)).ToArray(),
                    ClientUtility.LoadInitCommunications(_service)
                );

                if (r.InvokeInfos != null && _IsAvaliable(r.InvokeInfos))
                {
                    _service.Context.Platform.UpdateInvokeInfos(r.InvokeInfos);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }

            return false;
        }

        // 获取终端信息
        private AppClientInfo _GetAppClientInfo()
        {
            ClientInfoWrapper ciWrapper = _clientInfoCache.Get();
            return new AppClientInfo() {
                IPs = ciWrapper.IPs,
                Desc = ciWrapper.Desc,
                Title = ciWrapper.Title,
                HostName = ciWrapper.HostName,
                ClientId = _service.Context.ClientID,
                Communications = _service.Context.Platform.GetAllCommunicationOptions(),
                InvokeInfos = _service.Context.Platform.GetInvokeInfos(),
                ServiceInfos = _service.Context.Platform.GetAllServiceInfos(),
                RuntimeInfo = _GetAppClientRuntimeInfo(),
                PlatformDeployId = _service.Context.PlatformDeployId,
                ConnectedTime = DateTime.MinValue,
                Avaliable = _clientAvalidate,
            };
        }

        private bool _IsAvaliable(AppInvokeInfo[] appInvokeInfos)
        {
            return ServiceFairyUtility.ContainsService(appInvokeInfos, SFNames.ServiceNames.Station);
        }

        /// <summary>
        /// 心跳
        /// </summary>
        private void _HeartBeat()
        {
            AppClientInfo clientInfo = _GetAppClientInfo();
            AppServiceCookieCollection cookies = _service.Context.CookieManager.GetCookies();
            ServiceResult<Station_ClientHeartBeat_Reply> reply = _service.Invoker.Station.ClientHeartBeatSr(
                clientInfo, cookies, _service.Context.ServiceDesc, _service.Context.ClientID
            );

            if (reply.Succeed)
            {
                Station_ClientHeartBeat_Reply r = reply.Result;
                ConfigurationVersionPair[] cfgVersions = r.ConfigurationVersionPairs;

                // 记录应答Cookie
                _RecordReplyCookies(reply.Result.Cookies);

                if (r.ConfigurationVersionPairs != null)  // 配置文件变化
                {
                    bool modified = _service.DeployExecutor.DownloadAndUpdateConfiguration(r.ConfigurationVersionPairs.ToDictionary(v => v.ServiceDesc, v => v.Version, true));
                    if (modified)
                        _RaiseConfigurationModifiedEvent();
                }

                if (r.DeployVersion != _lastDeployVersion)  // 部署地图变化
                {
                    bool? modified = _service.DeployExecutor.DownloadAndUpdateAppServicesAndPlatform(r.DeployVersion);
                    if (modified != null)
                    {
                        if (modified == true)
                            _RaiseDeployModifiedEvent();
                        else // 当没有变化时，再将版本号替换为新的
                            _lastDeployVersion = r.DeployVersion;
                    }
                }
                else
                {
                    if (!_clientAvalidate)
                    {
                        _clientAvalidate = true;
                        ExecuteImmediately();
                    }
                }
            }
            else
            {
                _Init();
            }
        }

        private void _RecordReplyCookies(AppServiceCookieCollection cookies)
        {
            _service.Context.CookieManager.SetReplyCookies(cookies);
        }

        class ClientInfoWrapper
        {
            public string Title, Desc, HostName;
            public string[] IPs;
        }

        private readonly SingleMemoryCache<ClientInfoWrapper> _clientInfoCache;

        private ClientInfoWrapper _GetClientInfo()
        {
            string hostName = NetworkUtility.GetHostName();
            return new ClientInfoWrapper {
                IPs = NetworkUtility.GetAllEnableIP4Addresses().ToArray(v => v.ToString()),
                Title = StringUtility.GetFirstNotNullOrWhiteSpaceString(
                    _service.Context.Platform.GetData(SFNames.DataKeys.CLIENT_TITLE).ToStringIgnoreNull(),
                    hostName
                ),
                HostName = hostName,
                Desc = StringUtility.GetFirstNotNullOrWhiteSpaceString(
                    _service.Context.Platform.GetData(SFNames.DataKeys.CLIENT_DESC).ToStringIgnoreNull()
                ),
            };
        }

        /// <summary>
        /// 获取服务终端的运行时信息
        /// </summary>
        /// <returns></returns>
        public AppClientRuntimeInfo _GetAppClientRuntimeInfo()
        {
            Process p = Process.GetCurrentProcess();
            return new AppClientRuntimeInfo() {
                WorkingSetMemorySize = p.WorkingSet64, PrivateMemorySize = p.PrivateMemorySize64,
                OnlineUserStatInfo = _service.Context.SessionStateManager.GetOnlineUserStatInfo(),
            };
        }

        /// <summary>
        /// 部署发生变化
        /// </summary>
        public event EventHandler DeployModified;

        private void _RaiseDeployModifiedEvent()
        {
            ExecuteImmediately();

            var eh = DeployModified;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        /// <summary>
        /// 配置发生变化
        /// </summary>
        public event EventHandler ConfigurationModified;

        private void _RaiseConfigurationModifiedEvent()
        {
            var eh = ConfigurationModified;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        protected override void OnDispose()
        {
            _clientInfoCache.Dispose();

            base.OnDispose();
        }
    }
}
