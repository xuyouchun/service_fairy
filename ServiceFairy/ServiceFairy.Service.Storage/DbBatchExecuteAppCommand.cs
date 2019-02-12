using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Storage;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 批量执行数据库操作
    /// </summary>
    [AppCommand("DbBatchExecute", "批量执行数据库操作")]
    class DbBatchExecuteAppCommand : AppCommandBase<Storage_DbBatchExecute_Request, Storage_DbBatchExecute_Reply>
    {
        protected override Storage_DbBatchExecute_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbBatchExecute_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}
