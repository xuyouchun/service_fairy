using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Test;

namespace ServiceFairy.Service.Test
{
    /// <summary>
    /// 启动测试
    /// </summary>
    [AppCommand("StartTest", "启动测试任务")]
    class StartTestAppCommand : ACS<Service>.Func<Test_StartTest_Request, Test_StartTest_Reply>
    {
        protected override Test_StartTest_Reply OnExecute(AppCommandExecuteContext<Service> context, Test_StartTest_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            Guid taskId = ((Service)context.Service).TestTaskManager.StartTest(req.Name, req.Title, req.Args);

            return new Test_StartTest_Reply() { TaskId = taskId };
        }
    }
}
