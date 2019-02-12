using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取自定义命令信息
    /// </summary>
    [AppCommand("GetCustomCommandInfos", "获取自定义命令信息")]
    class GetCustomCommandInfosAppCommand : ACS<Service>.Func<Tray_GetCustomCommandInfos_Reply>
    {
        protected override Tray_GetCustomCommandInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}
