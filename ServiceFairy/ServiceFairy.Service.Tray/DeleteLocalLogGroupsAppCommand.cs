using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 删除日志组
    /// </summary>
    [AppCommand("DeleteLocalLogGroups", "删除日志组", SecurityLevel = SecurityLevel.Admin)]
    class DeleteLocalLogGroupsAppCommand : ACS<Service>.Action<Tray_DeleteLocalLogGroups_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Tray_DeleteLocalLogGroups_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            context.Service.LocalLogManager.DeleteLogGroups(request.Groups);
        }
    }
}
