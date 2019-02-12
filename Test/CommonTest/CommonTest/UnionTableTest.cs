using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Utility;

namespace CommonTest
{
    static class UnionTableTest
    {
        public static void Test()
        {
            /*
            CountStopwatch sw = CountStopwatch.StartNew();

            for (int g = 0; g < 20; g++)
            {
                ThreadUtility.StartNew(delegate {
                    using (SystemInvoker invoker = SystemInvoker.FromNavigation(navigation))
                    {
                        RemoteUtConnectionProvider utConProvider = new RemoteUtConnectionProvider(invoker, null);
                        for (int k = 1; ; k++)
                        {
                            DbTest dbTest = new DbTest() { Content = "Content_" + k, Time = DateTime.Now, Title = "Title_" + k };
                            dbTest.Insert(utConProvider);

                            sw.Increment();
                        }
                    }
                });
            }

            sw.AutoTrace();*/

            return;
        }
    }
}
