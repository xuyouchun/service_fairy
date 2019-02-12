using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Database;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 更新数据
    /// </summary>
    [AppCommand("Update", "更新数据", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class UpdateAppCommand : ACS<Service>.Func<Database_Update_Request, Database_Update_Reply>
    {
        protected override Database_Update_Reply OnExecute(AppCommandExecuteContext<Service> context, Database_Update_Request req, ref ServiceResult sr)
        {
            int effectRowCount = context.Service.DbQuerier.Update(req.TableName, req.Data, req.RouteKeys, req.Where, req.Settings);
            return new Database_Update_Reply() { EffectRowCount = effectRowCount };
        }
    }
}
