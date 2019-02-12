using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;
using Common.Algorithms;
using Common.Contracts.Entities;
using Common.Package.Storage;
using Common.Data.SqlExpressions;

namespace CommonTest
{
    unsafe class Program
    {
        static int[] _arr = new int[] { 0, 1, 2 };

        static void Main(string[] args)
        {
            string sss = "1" + "+86 15001261438";
            string s = SecurityUtility.Md5(sss);

            List<HashSet<int>> arrs = new List<HashSet<int>>();
            for (int k = 0; k < 100000; k++)
            {
                IList<int> list = _arr.PickRandom(1);
                arrs.Add(list.ToHashSet());
            }

            for (int k = 0; k < _arr.Length; k++)
            {
                int v = _arr[k];
                int count = arrs.Count(hs => hs.Contains(v));

                Console.WriteLine("{0}: {1}", v, (double)count / arrs.Count);
            }
        }
    }
}
