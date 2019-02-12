using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Storage;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 获取表信息
    /// </summary>
    [AppCommand("DbGetTableInfos", "获取表的信息")]
    class DbGetTableInfosAppCommand : AppCommandBase<Storage_DbGetTableInfos_Request, Storage_DbGetTableInfos_Reply>
    {
        protected override Storage_DbGetTableInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbGetTableInfos_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service srv = context.Service;

            DbTableInfo[] tableInfos = req.TableNames != null ?
                srv.DatabaseManager.GetTableInfos(req.TableNames, req.IncludeDetail) :
                srv.DatabaseManager.GetAllTableInfos(req.IncludeDetail);

            return new Storage_DbGetTableInfos_Reply() { DbTableInfos = tableInfos };
        }
    }
}
