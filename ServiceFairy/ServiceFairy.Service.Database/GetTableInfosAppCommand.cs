using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Database;
using Common.Contracts.Service;
using Common.Data;
using Common.Utility;
using Common.Collection;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 获取表信息
    /// </summary>
    [AppCommand("GetTableInfos", "获取表信息", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class GetTableInfosAppCommand : ACS<Service>.Func<Database_GetTableInfos_Request,Database_GetTableInfos_Reply>
    {
        protected override Database_GetTableInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Database_GetTableInfos_Request req, ref ServiceResult sr)
        {
            TableInfo[] allTableInfos = context.Service.DbManager.GetTableInfos();

            TableInfo[] infos;
            if (req.TableNames == null)
            {
                infos = allTableInfos;
            }
            else
            {
                IgnoreCaseHashSet hs = req.TableNames.ToIgnoreCaseHashSet();
                infos = allTableInfos.Where(info => hs.Contains(info.Name)).ToArray();
            }

            return new Database_GetTableInfos_Reply() { TableInfos = infos };
        }
    }
}
