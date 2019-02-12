using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.Service;

namespace ServiceFairy.Service.Proxy.Components
{
    /// <summary>
    /// 代理访问管理器
    /// </summary>
    [AppComponent("代理访问管理器", "")]
    class ProxyAccessManager : AppComponent
    {
        public ProxyAccessManager(Service service)
            : base(service)
        {

        }




    }
}
