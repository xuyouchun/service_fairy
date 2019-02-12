using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 删除文件或目录
    /// </summary>
    [AppCommand("FsDeletePath", title: "删除文件或目录", SecurityLevel = SecurityLevel.Admin)]
    class FsDeletePathAppCommand : ACS<Service>.Action<Tray_FsDeletePath_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Tray_FsDeletePath_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            srv.FileSystem.Delete(req.Paths);
        }
    }
}
