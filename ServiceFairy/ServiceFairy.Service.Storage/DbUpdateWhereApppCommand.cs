using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Storage;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 按条件更新数据
    /// </summary>
    [AppCommand("DbUpdateWhere", "按条件更新数据")]
    class DbUpdateWhereApppCommand : AppCommandBase<Storage_DbUpdateWhere_Request, Storage_DbUpdateWhere_Reply>
    {
        protected override Storage_DbUpdateWhere_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbUpdateWhere_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            context.Service.DatabaseManager.UpdateWhere(req.TableName, req.Data, req.Where.ToSqlExpression());
            return new Storage_DbUpdateWhere_Reply();
        }
    }
}
