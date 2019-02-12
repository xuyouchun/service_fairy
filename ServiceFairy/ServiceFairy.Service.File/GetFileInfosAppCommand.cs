using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.File.UnionFile;
using Common.Utility;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 获取文件信息
    /// </summary>
    [AppCommand("GetFileInfos", title: "获取文件信息", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class GetFileInfosAppCommand : ACS<Service>.Func<File_GetFileInfos_Request, File_GetFileInfos_Reply>
    {
        protected override File_GetFileInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, File_GetFileInfos_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            UnionFileInfo[] fileInfos = req.Paths.ToArray(p => srv.FileSystemManager.GetFileInfo(p));
            return new File_GetFileInfos_Reply() { Infos = fileInfos };
        }
    }
}
