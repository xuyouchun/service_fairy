using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.Collection;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 获取文件的路由信息
    /// </summary>
    [AppCommand("GetRouteInfo", title: "获取文件的路由信息", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class GetRouteInfoAppCommand : ACS<Service>.Func<File_GetRouteInfo_Request, File_GetRouteInfo_Reply>
    {
        protected override File_GetRouteInfo_Reply OnExecute(AppCommandExecuteContext<Service> context, File_GetRouteInfo_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            FileRouteInfo[] infos = req.Paths.Distinct(IgnoreCaseEqualityComparer.Instance)
                .Select(path => new FileRouteInfo { Path = path, ClientID = srv.FileRoute.GetClientIdForPath(path) }).ToArray();

            return new File_GetRouteInfo_Reply() { Infos = infos };
        }
    }
}
