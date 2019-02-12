using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Database;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 执行SQL语句
    /// </summary>
    [AppCommand("ExecuteSql", "执行SQL语句", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class ExecuteSqlAppCommand : ACS<Service>.Func<Database_ExecuteSql_Request, Database_ExecuteSql_Reply>
    {
        protected override Database_ExecuteSql_Reply OnExecute(AppCommandExecuteContext<Service> context, Database_ExecuteSql_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}
