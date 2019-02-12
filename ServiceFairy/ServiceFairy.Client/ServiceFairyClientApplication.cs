using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using Common.Contracts;
using Common.Framework.TrayPlatform;
using System.Threading;

namespace ServiceFairy.Client
{
    /// <summary>
    /// Service Fairy 客户端
    /// </summary>
    [AppEntryPoint]
    public class ServiceFairyClientApplication : ApplicationBase
    {
        public ServiceFairyClientApplication()
        {

        }

        public override void Run(Action<string, string[]> callback, WaitHandle waitHandle)
        {
            
        }
    }
}
