using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Storage;
using Common.Data.UnionTable;
using Common.Data;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 查询表中所有字段的数据
    /// </summary>
    [AppCommand("DbSelectAllFields", "查询表中所有字段的数据")]
    class DbSelectAllFieldsAppCommand : AppCommandBase<Storage_DbSelectAllFields_Request, Storage_DbSelectAllFields_Reply>
    {
        protected override Storage_DbSelectAllFields_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbSelectAllFields_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service srv = context.Service;

            DataCollection dc = srv.DatabaseManager.SelectAllFields(req.TableName, req.RouteKeys, req.Where.ToSqlExpression());
            return new Storage_DbSelectAllFields_Reply() { Data = dc };
        }
    }
}
