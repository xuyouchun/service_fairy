using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Execute;
using Common.Contracts;

namespace ServiceFairyTestService
{
    /// <summary>
    /// 测试服务
    /// </summary>
    [AppEntityPoint]
    public class TestService : AppServiceBase
    {
        protected override CommunicateData OnCall(string command, CommunicateData data)
        {
            this.CallbackCommunicate.Call("TestService2/MyMethod", new CommunicateData(Encoding.UTF8.GetBytes("My God!"), DataFormat.Binary));

            return new CommunicateData(Encoding.UTF8.GetBytes("我爱北京天安门 天安门上太阳升!"), data.Format);
        }
    }
}
