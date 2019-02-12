using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;

namespace ServiceFairy.Management.DeployPackage
{
    partial class ServiceDeployPackageListNode
    {
        /// <summary>
        /// 同步安装包
        /// </summary>
        class SyncAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                
            }
        }
    }
}
