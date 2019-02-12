using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities.Master;
using Common.Package;

namespace ServiceFairy.Service.Master
{
    /// <summary>
    /// 下载服务管理器信息
    /// </summary>
    [AppCommand("DownloadServiceUIInfo", "下载服务管理器图标资源及程序集", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class DownloadServiceUIInfoAppCommand : ACS<Service>.Func<Master_DownloadServiceUIInfo_Request, Master_DownloadServiceUIInfo_Reply>
    {
        protected override Master_DownloadServiceUIInfo_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_DownloadServiceUIInfo_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            ServiceDesc[] sds = req.ServiceDescs;
            ServiceUIInfo[] uiInfos = sds.Select(sd => _TryGetUIInfo(srv, sd, req.IncludeAssembly, req.IncludeResource)).WhereNotNull().ToArray();
            return new Master_DownloadServiceUIInfo_Reply() {
                ServiceUIInfos = uiInfos
            };
        }

        private ServiceUIInfo _TryGetUIInfo(Service srv, ServiceDesc sd, bool includeAssembly, bool includeResource)
        {
            try
            {
                return _Clone(srv.ServiceUIInfoManager.Get(sd), includeAssembly, includeResource);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return null;
            }
        }

        private ServiceUIInfo _Clone(ServiceUIInfo info, bool includeAssembly, bool includeResource)
        {
            if (info == null)
                return null;

            return ServiceUIInfo.Clone(info, includeAssembly, includeResource);
        }
    }
}
