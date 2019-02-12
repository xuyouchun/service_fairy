using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取文件系统顶级目录的信息
    /// </summary>
    [AppCommand("FsGetRootDirectoryInfos", title: "获取文件系统顶级目录的信息", SecurityLevel = SecurityLevel.Admin)]
    class FsGetRootDirectoryInfosAppCommand : ACS<Service>.Func<Tray_FsGetRootDirectoryInfos_Reply>
    {
        protected override Tray_FsGetRootDirectoryInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            Service srv = context.Service;

            FsDirectoryInfo[] infos = srv.FileSystem.GetFileSystemRootDirectoryInfos();
            return new Tray_FsGetRootDirectoryInfos_Reply() { Infos = infos };
        }
    }
}
