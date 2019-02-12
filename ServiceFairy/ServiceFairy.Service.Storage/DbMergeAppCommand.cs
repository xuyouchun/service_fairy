using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Storage;
using Common.Data;
using Common.Utility;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 数据库合并
    /// </summary>
    [AppCommand("DbMerge", "数据库合并")]
    class DbMergeAppCommand : AppCommandBase<Storage_DbMerge_Request, Storage_DbMerge_Reply>
    {
        protected override Storage_DbMerge_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbMerge_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service srv = context.Service;

            DataCollection dc = req.Data;
            int[] rowIndexes = dc.DataItems.PickIndex(di => di.PrimaryKey == null);
            srv.DatabaseManager.Merge(req.TableName, req.RouteKey, req.Data, req.CompareFields, req.Fields, req.MergeType);
            object[] newPrimaryKeys = dc.DataItems.PickValueByIndexes(rowIndexes).ToArray(di => di.PrimaryKey);

            return new Storage_DbMerge_Reply() { NewPrimaryKeys = newPrimaryKeys };
        }
    }
}
