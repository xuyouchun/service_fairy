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
    /// 按组搜索数据
    /// </summary>
    [AppCommand("DbSearchGroup", "按组搜索数据")]
    class DbSearchGroupAppCommand : AppCommandBase<Storage_DbSearchGroup_Request, Storage_DbSearchGroup_Reply>
    {
        protected override Storage_DbSearchGroup_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbSearchGroup_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service service = context.Service;

            DbSearchInfo info = req.SearchInfo;
            int totalCount;
            DataCollection data = service.DatabaseManager.SearchGroup(
                req.TableName, info.ToUnionTableSearchParameter(),
                req.Groups, out totalCount
            );

            return new Storage_DbSearchGroup_Reply() { Data = data, TotalCount = totalCount };
        }
    }
}
