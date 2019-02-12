using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 执行自定义命令
    /// </summary>
    [AppCommand("DoCommand", "执行自定义命令")]
    class DoCommandAppCommand : ACS<Service>.Func<Tray_DoCommand_Request, Tray_DoCommand_Reply>
    {
        protected override Tray_DoCommand_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_DoCommand_Request request, ref ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}
