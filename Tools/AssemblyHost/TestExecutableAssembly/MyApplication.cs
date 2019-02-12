using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Threading;
using System.Configuration;

namespace TestExecutableAssembly
{
    public class MyApplication : IExecutableAssembly
    {
        int _index;

        public int Execute(string[] args)
        {
            while (true)
            {
                string aaa = ConfigurationManager.AppSettings.Get("aaa");

                Console.WriteLine("Hello World - " + _index++);

                Thread.Sleep(1000);
            }
        }
    }
}
