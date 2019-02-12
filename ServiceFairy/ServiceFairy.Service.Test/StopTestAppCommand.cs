using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Test;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Test
{
    /// <summary>
    /// 停止测试
    /// </summary>
    [AppCommand("StopTest", "停止测试任务")]
    class StopTestAppCommand : ACS<Service>.Func<Test_StopTest_Request, Test_StopTest_Reply>
    {
        protected override Test_StopTest_Reply OnExecute(AppCommandExecuteContext<Service> context, Test_StopTest_Request req, ref ServiceResult sr)
        {
            Service service = (Service)context.Service;

            service.TestTaskManager.StopTest(req.TestIds);
            return new Test_StopTest_Reply();
        }
    }
}
