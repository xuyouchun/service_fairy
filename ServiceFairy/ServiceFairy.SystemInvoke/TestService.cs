using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Test;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 测试服务
    /// </summary>
    public static class TestService
    {
        /// <summary>
        /// 启动测试任务
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Test_StartTest_Reply> StartTest(IServiceClient serviceClient, Test_StartTest_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Test_StartTest_Reply>(SFNames.ServiceNames.Test + "/StartTest", request, settings);
        }
    }
}
