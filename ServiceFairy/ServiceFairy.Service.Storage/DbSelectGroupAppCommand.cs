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
    /// 数据库按组查询
    /// </summary>
    [AppCommand("DbSelectGroup", "从数据库中按字段组查询数据")]
    class DbSelectGroupAppCommand : AppCommandBase<Storage_DbSelectGroup_Request, Storage_DbSelectGroup_Reply>
    {
        protected override Storage_DbSelectGroup_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbSelectGroup_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service srv = context.Service;

            DataCollection dc = srv.DatabaseManager.SelectGroup(req.TableName, req.RouteKeys, req.GroupNames, req.Where.ToSqlExpression());
            return new Storage_DbSelectGroup_Reply() { Data = dc };
        }
    }
}
