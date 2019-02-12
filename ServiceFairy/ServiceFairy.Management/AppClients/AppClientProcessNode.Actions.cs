using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using Common.Utility;

namespace ServiceFairy.Management.AppClients
{
    partial class AppClientProcessNode
    {
        class KillProcessAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                if (uiOp.ShowQuery("确认要结束该进程吗？") != true)
                    return;

                Ctx.MgrCtx.Invoker.Tray.KillProcess(PInfo.ID);
            }
        }
    }
}
