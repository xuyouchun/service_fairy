using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Queue;
using Common.Contracts.Service;
using ServiceFairy.Service.Queue.Components;

namespace ServiceFairy.Service.Queue
{
    /// <summary>
    /// 添加任务
    /// </summary>
    [AppCommand("AddTask", "将任务添加到队列中", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class AddTaskAppCommand : ACS<Service>.Action<Queue_AddTask_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Queue_AddTask_Request req, ref ServiceResult sr)
        {
            Service service = (Service)context.Service;
            service.TaskManager.AddTasks(req.Tasks);
        }
    }
}
