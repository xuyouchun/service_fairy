using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Test;
using ServiceFairy.Service.Test.Components;

namespace ServiceFairy.Service.Test
{
    /// <summary>
    /// 获取任务的进度
    /// </summary>
    [AppCommand("GetTaskProgress", "获取任务的进度")]
    class GetTaskProgressAppCommand : ACS<Service>.Func<Test_GetTaskProgress_Request, Test_GetTaskProgress_Reply>
    {
        protected override Test_GetTaskProgress_Reply OnExecute(AppCommandExecuteContext<Service> context, Test_GetTaskProgress_Request req, ref ServiceResult sr)
        {
            TestTaskManagerAppComponent mgr = ((Service)context.Service).TestTaskManager;

            TaskProgress[] progresses = (req.TaskIds == null) ?
                mgr.GetAllTestProgresses() : mgr.GetTaskProgresses(req.TaskIds);

            return new Test_GetTaskProgress_Reply() { Progresses = progresses };
        }
    }
}
