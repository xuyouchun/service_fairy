using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Storage;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 数据库删除
    /// </summary>
    [AppCommand("DbDelete", "从数据库中删除数据")]
    class DbDeleteAppCommand : AppCommandBase<Storage_DbDelete_Request, Storage_DbDelete_Reply>
    {
        protected override Storage_DbDelete_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbDelete_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            context.Service.DatabaseManager.Delete(req.TableName, req.RouteKeys, req.Where.ToSqlExpression());
            return new Storage_DbDelete_Reply();
        }
    }
}
