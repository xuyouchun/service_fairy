using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Addins.MesssageSubscript;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 消息订阅插件
    /// </summary>
    public static class MessageSubscriptAddin
    {
        /// <summary>
        /// 消息订阅通知
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<MesasgeSubscript_ApplyMessage_Reply> ApplyMessage(IServiceClient serviceClient,
            MesasgeSubscript_ApplyMessage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<MesasgeSubscript_ApplyMessage_Reply>("ApplyMessage", request, settings);
        }
    }
}
