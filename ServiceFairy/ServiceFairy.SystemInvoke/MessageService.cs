using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities.Message;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 消息服务
    /// </summary>
    public static class MessageService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.Message + "/" + method;
        }

        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult SendMessages(IServiceClient serviceClient, 
            Message_SendMessages_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("SendMessages"), request, settings);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult SendMessage(IServiceClient serviceClient,
            Message_SendMessage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("SendMessage"), request, settings);
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RemoveMessages(IServiceClient serviceClient,
            Message_RemoveMessages_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("RemoveMessages"), request, settings);
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RemoveMessage(IServiceClient serviceClient,
            Message_RemoveMessage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("RemoveMessage"), request, settings);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Message_GetMessages_Reply> GetMessages(IServiceClient serviceClient,
            Message_GetMessages_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Message_GetMessages_Reply>(_GetMethod("GetMessages"), request, settings);
        }

        /// <summary>
        /// 将指定用户的持久化存储消息重新发送
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Message_ResendMessage_Request> ResendMessage(IServiceClient serviceClient,
            Message_ResendMessage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Message_ResendMessage_Request>(_GetMethod("ResendMessage"), request, settings);
        }
    }
}
