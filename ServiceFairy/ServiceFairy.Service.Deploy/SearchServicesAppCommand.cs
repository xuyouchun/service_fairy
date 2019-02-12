using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Deploy;

namespace ServiceFairy.Service.Deploy
{
    /// <summary>
    /// 寻找指定服务所在的位置
    /// </summary>
    [AppCommand("SearchServices", "寻找指定服务的所在位置", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class SearchServicesAppCommand : ACS<Service>.Func<Deploy_SearchServices_Request, Deploy_SearchServices_Reply>
    {
        protected override Deploy_SearchServices_Reply OnExecute(AppCommandExecuteContext<Service> context, Deploy_SearchServices_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            if (!srv.DeployMapManager.WaitForAvaliable(TimeSpan.FromSeconds(15)))
                throw CreateServiceException("部署地图尚未初始化", ServerErrorCode.DataNotReady);

            return new Deploy_SearchServices_Reply() { AppInvokeInfos = srv.DeployMapManager.SearchServices(req.ServiceDesc) };
        }
    }
}
