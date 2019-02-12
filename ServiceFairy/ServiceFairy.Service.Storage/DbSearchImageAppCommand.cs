using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Storage;
using Common.Contracts.Service;
using Common.Data.UnionTable;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 在镜像表中搜索
    /// </summary>
    [AppCommand("DbSearchImage", "在镜像表中搜索")]
    class DbSearchImageAppCommand : AppCommandBase<Storage_DbSearchImage_Request, Storage_DbSearchImage_Reply>
    {
        protected override Storage_DbSearchImage_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbSearchImage_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service service = context.Service;

            DbSearchInfo info = req.SearchInfo;
            UnionTableSearchResult result = service.DatabaseManager.SearchPrimaryKeys(
                req.TableName, req.Fields, info.ToUnionTableSearchParameter()
            );

            return new Storage_DbSearchImage_Reply() { Result = result };
        }
    }
}
