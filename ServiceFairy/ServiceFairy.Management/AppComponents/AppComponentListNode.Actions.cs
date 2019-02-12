using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;

namespace ServiceFairy.Management.AppComponents
{
    partial class AppComponentListNode
    {
        private static bool _showAll;

        class ShowAllAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                _showAll = !_showAll;
                Refresh(context, serviceObject);
            }

            public override UIObjectExecutorState GetState(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                return UIObjectExecutorState.OfChecked(_showAll);
            }
        }
    }
}
