using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.SqlExpressions;

namespace CommonTest
{
    static class SqlExpressionTest
    {
        public static void Test()
        {
            string sss = "MyFunc(a>b, c<d) And Func2(a, b, c, d, e)";
            SqlExpression sqlExp = SqlExpression.Parse(sss);
            string[] fieldNames = SqlExpression.GetAllFieldNames(sqlExp);
            
            string expStr = sqlExp.ToString();
            Console.WriteLine(expStr);
        }
    }
}
