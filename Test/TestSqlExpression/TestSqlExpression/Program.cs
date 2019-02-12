using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.SqlExpressions;

namespace TestSqlExpression
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "[Basic.Time] <= '2012/8/11 4:26:46'";
            // 支持运算符的优先级
            SqlExpression exp = SqlExpression.Parse(s);

            Console.WriteLine(exp);
        }
    }
}
