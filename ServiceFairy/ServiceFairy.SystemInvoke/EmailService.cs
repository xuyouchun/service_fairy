using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Email;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public static class EmailService
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Send(IServiceClient serviceClient,
            Email_Send_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Email + "/Send", request, settings);
        }
    }
}
