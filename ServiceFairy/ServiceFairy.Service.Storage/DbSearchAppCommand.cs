using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Data.UnionTable;
using ServiceFairy.Entities.Storage;
using Common.Data;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 数据库搜索
    /// </summary>
    [AppCommand("DbSearch", "数据符合条件的记录的主键")]
    class DbSearchAppCommand : AppCommandBase<Storage_DbSearch_Request, Storage_DbSearch_Reply>
    {
        protected override Storage_DbSearch_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbSearch_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service service = context.Service;

            DbSearchInfo info = req.SearchInfo;
            int totalCount;
            DataCollection data = service.DatabaseManager.Search(
                req.TableName, info.ToUnionTableSearchParameter(), req.Fields, out totalCount
            );

            return new Storage_DbSearch_Reply() { Data = data, TotalCount = totalCount };
        }
    }
}
