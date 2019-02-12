using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Database;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 合并数据
    /// </summary>
    [AppCommand("Merge", "合并数据", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class MergeAppCommand : ACS<Service>.Func<Database_Merge_Request, Database_Merge_Reply>
    {
        protected override Database_Merge_Reply OnExecute(AppCommandExecuteContext<Service> context, Database_Merge_Request req, ref ServiceResult sr)
        {
            int effectRowCount = context.Service.DbQuerier.Merge(req.TableName, req.RouteKey, req.Data, req.CompareColumns, req.Where, req.Option, req.Settings);

            return new Database_Merge_Reply() { EffectRowCount = effectRowCount };
        }
    }
}
