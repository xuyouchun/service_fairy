using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using System.Threading;

namespace CommonTest
{
    class Cache
    {
        static readonly SingleMemoryCache<int[]> _cache = new SingleMemoryCache<int[]>(TimeSpan.FromSeconds(5), delegate {
            Console.WriteLine("Load it!");
            return new int[10];
        });

        internal static void MyMethod()
        {
            for (int k = 0; k < 1000; k++)
            {
                int[] items = _cache.Get();

                Console.WriteLine(items.Length);

                Thread.Sleep(2000);
            }
        }
    }
}
