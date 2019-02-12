using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Database;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 删除数据
    /// </summary>
    [AppCommand("Delete", "删除数据", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class DeleteAppCommand : ACS<Service>.Func<Database_Delete_Request, Database_Delete_Reply>
    {
        protected override Database_Delete_Reply OnExecute(AppCommandExecuteContext<Service> context, Database_Delete_Request req, ref ServiceResult sr)
        {
            int effectRowCount = context.Service.DbQuerier.Delete(req.TableName, req.Data, req.Where, req.Settings);
            return new Database_Delete_Reply() { EffectRowCount = effectRowCount };
        }
    }
}
