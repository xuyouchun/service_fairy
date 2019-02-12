using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable;

namespace DbLogDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string conStr = "Data Source=117.79.130.229;Initial Catalog=UnionTableMeta2;User ID=hefengxin;Password=zBr6s336";
            UtDatabase db = UtDatabase.Create(conStr, typeof(DbNameCardSharingLog));

            DbNameCardSharingLog log = new DbNameCardSharingLog {
                Device = "Device", GeoKey = "GeoKey", LogDate = DateTime.Now, LogSource = "LogSource",
                Rom = "Rom", UserId = "UserId", UserName = "UserName",
            };

            // 插入一条记录，注意Id留空，将会使用自增的ID
            log.Insert(db);

            int logId = log.Id;  // 自动生成的ID
            Console.WriteLine("{0} - OK!", logId);

            // 同时插入多条记录，注意Id留空，将会使用自增的ID
            List<DbNameCardSharingLog> logs = new List<DbNameCardSharingLog>();
            for (int k = 0; k < 10000; k++)
            {
                logs.Add(new DbNameCardSharingLog {
                    Device = "Device_" + k, GeoKey = "GeoKey_" + k, LogDate = DateTime.Now, LogSource = "LogSource_" + k,
                    Rom = "Rom_" + k, UserId = "UserId_" + k, UserName = "UserName_" + k,
                });
            }

            DbNameCardSharingLog.Insert(db, logs);
            Console.WriteLine("OK!");
        }
    }
}
