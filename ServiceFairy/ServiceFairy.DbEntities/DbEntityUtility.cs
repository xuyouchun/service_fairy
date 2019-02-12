using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable;

namespace ServiceFairy.DbEntities
{
    public static class DbEntityUtility
    {
        public static UtTableGroupReviseInfo[] LoadReviseInfo()
        {
            return UtTableGroupReviseInfo.LoadFromAssembly(typeof(DbEntityUtility).Assembly);
        }
    }
}
