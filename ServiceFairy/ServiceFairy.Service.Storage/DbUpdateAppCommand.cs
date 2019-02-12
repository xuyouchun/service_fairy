using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Storage;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 数据库更新
    /// </summary>
    [AppCommand("DbUpdate", "在数据库中更新数据")]
    class DbUpdateAppCommand : AppCommandBase<Storage_DbUpdate_Request, Storage_DbUpdate_Reply>
    {
        protected override Storage_DbUpdate_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbUpdate_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service srv = context.Service;

            srv.DatabaseManager.Update(req.TableName, req.Data);
            return new Storage_DbUpdate_Reply();
        }
    }
}
