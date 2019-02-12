using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Storage;
using Common.Data;
using Common.Utility;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 数据库插入
    /// </summary>
    [AppCommand("DbInsert", "向数据库插入数据")]
    class DbInsertAppCommand : AppCommandBase<Storage_DbInsert_Request, Storage_DbInsert_Reply>
    {
        protected override Storage_DbInsert_Reply OnExecute(AppCommandExecuteContext<Service> context, Storage_DbInsert_Request req, ref Common.Package.Service.ServiceResult sr)
        {
            Service srv = context.Service;

            DataCollection dc = req.Data;
            int[] rowIndexes = dc.DataItems.PickIndex(item => item.PrimaryKey == null);
            srv.DatabaseManager.Insert(req.TableName, dc);

            object[] newPrimaryKeys = dc.DataItems.PickValueByIndexes(rowIndexes).ToArray(di => di.PrimaryKey);
            return new Storage_DbInsert_Reply() { NewPrimaryKeys = newPrimaryKeys };
        }
    }
}
