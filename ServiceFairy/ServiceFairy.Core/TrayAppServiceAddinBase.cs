using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;

namespace ServiceFairy
{
    /// <summary>
    /// 服务插件的基类
    /// </summary>
    public abstract class TrayAppServiceAddinBase : AppServiceAddinBaseEx
    {
        public TrayAppServiceAddinBase(AppServiceBase service)
            : base(service)
        {

        }

        protected override IAppCommand[] OnLoadAppCommands()
        {
            AppCommandCollection cmds = ServiceFairyUtility.LoadAppCommandsFromInstance(this);
            return base.OnLoadAppCommands().Union(cmds).ToArray();
        }
    }
}
