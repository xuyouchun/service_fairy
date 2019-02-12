using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Utility;
using System.Threading;
using Common.Algorithms;
using System.Security.Cryptography;
using System.IO;

namespace CommonTest
{
    static class DataSecurityTest
    {
        private static readonly Random _r = new Random();

        public static void Test()
        {
            LargeDataCryptoServiceProvider sp = new LargeDataCryptoServiceProvider("this is it");
            StringBuilder sb = new StringBuilder();
            sb.Append("ABc");
            for (int k = 0; k < 20; k++)
            {
                sb.Append(sb.ToString());
            }

            string sss = sb.ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(sss);
            sp.Encrypt(buffer, "ABCDEFG");

            byte[] newBuffer;
            CryptoStream cryptoStream = new CryptoStream(new MemoryStream(buffer), sp.CreateCryptoTransform("ABCDEFG"), CryptoStreamMode.Read);
            newBuffer = cryptoStream.ToBytes();

            string newSss = Encoding.UTF8.GetString(newBuffer);
            bool equals = (sss == newSss);
            return;

            Console.WriteLine("Buffer Length: {0}", buffer.Length);

            Console.WriteLine("Press any key to start ...");
            Console.ReadKey(false);

            CountStopwatch sw = CountStopwatch.StartNew();
            for (int k = 0; k < 1000000000; k++)
            {
                sp.Encrypt(buffer, "ABCDEFG");

                sw.Increment();

                if (k % 10000 == 0)
                    Console.WriteLine(sw);
            }
        }
    }
}
