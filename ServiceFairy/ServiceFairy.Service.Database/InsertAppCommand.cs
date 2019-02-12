using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Database;
using Common.Contracts.Service;
using Common.Utility;
using Common.Data;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 插入数据
    /// </summary>
    [AppCommand("Insert", "插入数据", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class InsertAppCommand : ACS<Service>.Func<Database_Insert_Request, Database_Insert_Reply>
    {
        protected override Database_Insert_Reply OnExecute(AppCommandExecuteContext<Service> context, Database_Insert_Request req, ref ServiceResult sr)
        {
            DataList dl = req.Data;

            int pkIndex = dl.GetPrimaryKeyColumnIndex();
            int[] rowIndexes = null;
            object[] newPrimaryKeys = null;

            if (pkIndex >= 0)
                rowIndexes = dl.Rows.PickIndex(row => row.Cells[pkIndex] == null);

            int effectRowCount = context.Service.DbQuerier.Insert(req.TableName, req.Data, req.AutoUpdate, req.Settings);

            if (pkIndex >= 0)
                newPrimaryKeys = dl.Rows.PickValueByIndexes(rowIndexes).ToArray(di => di.Cells[pkIndex]);

            return new Database_Insert_Reply { EffectRowCount = effectRowCount, NewPrimaryKeys = newPrimaryKeys };
        }
    }
}
