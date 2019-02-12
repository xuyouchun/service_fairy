using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Data.UnionTable;
using ServiceFairy.Entities.Storage;
using Common.Data;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 从数据库中查询
    /// </summary>
    [AppCommand("DbSelect", "从数据库中查询符合条件的数据")]
    class DbSelectAppCommand : AppCommandBase<Storage_DbSelect_Request, Storage_DbSelect_Reply>
    {
        protected override Storage_DbSelect_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbSelect_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service srv = context.Service;

            DataCollection data = srv.DatabaseManager.Select(req.TableName, req.RouteKeys, req.Fields, req.SearchInfo.ToUnionTableSearchParameter());
            return new Storage_DbSelect_Reply() { Data = data };
        }
    }
}
