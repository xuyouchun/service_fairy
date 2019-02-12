using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts.UIObject;
using Common.Utility;
using Common.WinForm;
using ServiceFairy.WinForm;

namespace ServiceFairy.Management.AppCommands
{
    partial class AppCommandNode
    {
        /// <summary>
        /// 测试
        /// </summary>
        class TestAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                AppCommandInvokeDialog dlg = new AppCommandInvokeDialog(
                    Kernel._clientCtx.MgrCtx.Invoker, Kernel._clientCtx.ClientDesc, 
                    Kernel._serviceDesc, Kernel._cmdInfo
                );

                uiOp.ShowDialog(dlg);
            }
        }
    }
}
