using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.UIObject;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Package.Service;

namespace Common.Package.UIObject.Actions
{
    /// <summary>
    /// 刷新列表
    /// </summary>
    public class RefreshAction : UIObjectExecutorBase
    {
        public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            ServiceUtility.RefreshCurrentListView(context, serviceObject);
        }
    }
}
