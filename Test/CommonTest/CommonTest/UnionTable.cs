using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Package;
using Common;
using Common.Data;
using Common.Data.SqlExpressions;
using Common.Data.UnionTable;
using ServiceFairy.DbEntities.User;
using ServiceFairy.DbEntities;
using Common.Data.UnionTable.Metadata;
using System.Threading;

namespace CommonTest
{
    class UnionTable
    {
        //const string _connectionString = "Data Source=xuyc-pc;Initial Catalog=UnionTableMeta;User ID=xuyc;Password=xuyc";
        const string _connectionString = "Data Source=117.79.130.229;Initial Catalog=UnionTableMeta2;User ID=hefengxin;Password=zBr6s336";
        //static DbService dbService = new DbService(new SqlMetaDataProvider(_connectionString));

        public static void Test()
        {
            UtService utService = new UtService();
            UtDatabase db = utService.Connect(_connectionString);

            db.InitMetaDataSchame();
            UtTableGroupReviseInfo[] reviseInfos = DbEntityUtility.LoadReviseInfo();
            db.InitMetaData(reviseInfos);

            List<DbUserContact> contacts = new List<DbUserContact>();
            for (int k = 10; k <= 11; k++)
            {
                contacts.Add(new DbUserContact {
                    CtFirstName = "New_CtFirstName_" + k,
                    CtLastName = "New_CtLastName_" + k,
                    CtUserId = 1000 + k,
                    CtUserName = "YEAH! CtUserName_" + k,
                    UserId = 1,
                });
            }

            //DbUserContact.Insert(db, contacts.ToArray());
            int effectCount = DbUserContact.Merge(db, 1, contacts, null, 
                new[] { DbUserContact.F_CtUserName, DbUserContact.F_CtFirstName },
                (string)SqlExpression.Like(DbUserContact.F_CtLastName, "New%"),
                //null,
                UtConnectionMergeOption.All
            );

            return;
        }
    }
}