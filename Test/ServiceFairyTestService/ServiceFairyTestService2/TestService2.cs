using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Execute;

namespace ServiceFairyTestService2
{
    /// <summary>
    /// 测试服务
    /// </summary>
    [AppEntityPoint]
    public class TestService : AppServiceBase
    {
        protected override CommunicateData OnCall(string command, CommunicateData data)
        {
            string s = Encoding.UTF8.GetString(data.ToBytes());
            return new CommunicateData(Encoding.UTF8.GetBytes("[From Service2]" + s), data.Format);
        }
    }
}
