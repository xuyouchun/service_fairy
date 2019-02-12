using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities.MessageCenter;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 消息中心服务
    /// </summary>
    public static class MessageCenterService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.MessageCenter + "/" + method;
        }

        /// <summary>
        /// 将消息批量持久化存储
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Save(IServiceClient serviceClient,
            MessageCenter_Save_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("Save"), request, settings);
        }

        /// <summary>
        /// 读取用户持久化的消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<MessageCenter_Read_Reply> Read(IServiceClient serviceClient,
            MessageCenter_Read_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<MessageCenter_Read_Reply>(_GetMethod("Read"), request, settings);
        }


        /// <summary>
        /// 清除指定用户的消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="requet"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Clear(IServiceClient serviceClient,
            MessageCenter_Clear_Request requet, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("Clear"), requet, settings);
        }
    }
}
