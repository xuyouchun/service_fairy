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
    /// 搜索全部数据
    /// </summary>
    [AppCommand("DbSearchAllFields", "查寻全部字段的数据")]
    class DbSearchAllFieldsAppCommand : AppCommandBase<Storage_DbSearchAllFields_Request, Storage_DbSearchAllFields_Reply>
    {
        protected override Storage_DbSearchAllFields_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbSearchAllFields_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service service = context.Service;

            DbSearchInfo info = req.SearchInfo;
            int totalCount;
            DataCollection data = service.DatabaseManager.SearchAllFields(
                req.TableName, info.ToUnionTableSearchParameter(), out totalCount
            );

            return new Storage_DbSearchAllFields_Reply() { Data = data, TotalCount = totalCount };
        }
    }
}
