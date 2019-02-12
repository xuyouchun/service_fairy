using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Data.UnionTable;
using Common.Package;
using System.Threading.Tasks;
using Common.Utility;
using Common.Data.SqlExpressions;

namespace TestDb
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "([Basic.Time] <= '2012/8/11 4:26:46')";
            SqlExpression exp = SqlExpression.Parse(s);

            return;
        }
    }
}
