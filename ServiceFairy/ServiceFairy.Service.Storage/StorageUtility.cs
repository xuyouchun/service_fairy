using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.SqlExpressions;

namespace ServiceFairy.Service.Storage
{
    static class StorageUtility
    {
        public static SqlExpression ToSqlExpression(this string exp)
        {
            if (string.IsNullOrEmpty(exp))
                return null;

            return SqlExpression.Parse(exp);
        }
    }
}
