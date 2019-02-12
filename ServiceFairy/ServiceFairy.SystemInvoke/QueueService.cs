using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Queue;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 队列服务
    /// </summary>
    public static class QueueService
    {
        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult AddTask(IServiceClient serviceClient, Queue_AddTask_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Queue + "/AddTask", request, settings);
        }
    }
}
