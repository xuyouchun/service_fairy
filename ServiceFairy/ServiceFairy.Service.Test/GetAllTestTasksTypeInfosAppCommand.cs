using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Test;
using Common.Contracts.Service;
using ServiceFairy.Service.Test.Components;

namespace ServiceFairy.Service.Test
{
    /// <summary>
    /// 获取所有测试任务的类型信息
    /// </summary>
    [AppCommand("GetAllTestTasksTypeInfos", "获取所有测试任务的类型信息")]
    class GetAllTestTasksTypeInfosAppCommand : ACS<Service>.Func<Test_GetAllTestTasksTypeInfos_Request, Test_GetAllTestTasksTypeInfos_Reply>
    {
        protected override Test_GetAllTestTasksTypeInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Test_GetAllTestTasksTypeInfos_Request req, ref ServiceResult sr)
        {
            return new Test_GetAllTestTasksTypeInfos_Reply() {
                Infos = TestTaskFactory.GetAllInfos(),
            };
        }
    }
}
