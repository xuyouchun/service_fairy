using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts.UIObject;
using Common.WinForm;
using ServiceFairy.WinForm;
using Common.Utility;

namespace ServiceFairy.Management.AppClients
{
    partial class AppClientSystemLogItemNode
    {
        class OpenAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);

                LogItemViewDialog dlg = new LogItemViewDialog();
                dlg.SetValue(LogItem);
                uiOp.ShowDialog(dlg);
            }
        }
    }
}
