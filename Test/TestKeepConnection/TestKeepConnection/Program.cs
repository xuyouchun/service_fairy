using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using ServiceFairy;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.Entities.Navigation;
using Common.Contracts;
using Common.Package;
using Common.Utility;
using System.Threading;

namespace TestKeepConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            TestJson.Test();
            //TestStateChange.Test();
        }
    }
}
