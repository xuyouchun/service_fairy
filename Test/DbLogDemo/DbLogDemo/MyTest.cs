using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Data.UnionTable;

namespace DbLogDemo
{
    static class MyTest
    {
        public static void Test()
        {
            const string conStr = "Data Source=117.79.130.229;Initial Catalog=UnionTableMeta2;User ID=hefengxin;Password=zBr6s336";
            UtDatabase db = UtDatabase.Create(conStr, typeof(DbNameCardSharingLog));

            /*
            DbSearchParam param = DbSearchParam.Top(5, where: "ID < 8", order: "ID Desc");
            DbNameCardSharingLog[] logs = DbNameCardSharingLog.Select(db, param: param, settings: UtInvokeSettings.FromPartialTable(0));*/

            int count = DbNameCardSharingLog.DeleteAll(db, new UtInvokeSettings { EnsureEffectAll = true });

            Console.WriteLine("OK!");
        }
    }
}
