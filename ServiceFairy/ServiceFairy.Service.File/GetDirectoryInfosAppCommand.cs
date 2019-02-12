using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.Utility;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 获取指定目录的信息及子目录及文件信息
    /// </summary>
    [AppCommand("GetDirectoryInfos", title: "获取指定目录的信息及子目录及文件信息", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class GetDirectoryInfosAppCommand : ACS<Service>.Func<File_GetDirectoryInfos_Request, File_GetDirectoryInfos_Reply>
    {
        protected override File_GetDirectoryInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, File_GetDirectoryInfos_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            DirectoryInfoItem[] items = req.Paths.ToArray(path => _GetDirectoryInfoItem(srv, path, req.Pattern, req.Option));
            return new File_GetDirectoryInfos_Reply() { Items = items };
        }

        private DirectoryInfoItem _GetDirectoryInfoItem(Service srv, string path, string pattern, GetDirectoryInfosOption option)
        {
            var fs = srv.FileSystemManager;
            return new DirectoryInfoItem() {
                DirectoryInfo = fs.GetDirectoryInfo(path),
                SubDirectoryInfos = (option & GetDirectoryInfosOption.Directory) != 0 ? fs.GetSubDirectoryInfos(path, pattern) : null,
                FileInfos = (option & GetDirectoryInfosOption.File) != 0 ? fs.GetFileInfos(path, pattern) : null,
            };
        }
    }
}
