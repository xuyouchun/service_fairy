using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Database;
using Common.Contracts.Service;
using Common.Data;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 选取数据
    /// </summary>
    [AppCommand("Select", "选取数据", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class SelectAppCommand : ACS<Service>.Func<Database_Select_Request, Database_Select_Reply>
    {
        protected override Database_Select_Reply OnExecute(AppCommandExecuteContext<Service> context, Database_Select_Request req, ref ServiceResult sr)
        {
            DataList data = context.Service.DbQuerier.Select(req.TableName, req.RouteKeys, req.Param, req.Columns, req.Settings);
            return new Database_Select_Reply() { Data = data };
        }
    }
}
