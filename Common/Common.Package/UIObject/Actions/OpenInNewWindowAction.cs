using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.UIObject;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.Package.Service;

namespace Common.Package.UIObject.Actions
{
    /// <summary>
    /// “打开”动作
    /// </summary>
    public class OpenInNewWindowAction : UIObjectExecutorBase
    {
        public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            ServiceUtility.ShowSubList(context, serviceObject, true);
        }
    }
}
