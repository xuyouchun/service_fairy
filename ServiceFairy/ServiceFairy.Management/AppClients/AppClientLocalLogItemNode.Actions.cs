using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using Common.Utility;
using ServiceFairy.WinForm;

namespace ServiceFairy.Management.AppClients
{
    partial class AppClientLocalLogItemNode
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
