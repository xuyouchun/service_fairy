using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;

namespace MobileAreaTestData
{
    unsafe class Program
    {
        static void Main(string[] args)
        {
            //ConvertNationnalCode.Run();
            //GenerateQuhaoUtility.Run();

            //GenerateTestDataUtility.Run();
            ConvertTestDataUtility.Run();
        }
    }
}
