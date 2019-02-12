using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.DatabaseCenter;

namespace ServiceFairy.Service.DatabaseCenter
{
    /// <summary>
    /// 获取数据库连接字符串
    /// </summary>
    [AppCommand("GetConStr", "获取数据库连接字符串", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetConStrAppCommand : ACS<Service>.Func<DatabaseCenter_GetConStr_Request, DatabaseCenter_GetConStr_Reply>
    {
        protected override DatabaseCenter_GetConStr_Reply OnExecute(AppCommandExecuteContext<Service> context, DatabaseCenter_GetConStr_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            return new DatabaseCenter_GetConStr_Reply {
                ConStr = context.Service.DbManager.GetConStr(request.Name),
            };
        }
    }
}
