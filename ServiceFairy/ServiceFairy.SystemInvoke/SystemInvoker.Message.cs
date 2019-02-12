using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities.Message;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private MessageInvoker _message;

        /// <summary>
        /// Message Service
        /// </summary>
        public MessageInvoker Message
        {
            get { return _message ?? (_message = new MessageInvoker(this)); }
        }

        /// <summary>
        /// Mesasge Service
        /// </summary>
        public class MessageInvoker : Invoker
        {
            public MessageInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 添加消息
            /// </summary>
            /// <param name="msgArrs">消息项</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendMessagesSr(UserMsgArray[] msgArrs, CallingSettings settings = null)
            {
                return MessageService.SendMessages(Sc, new Message_SendMessages_Request {
                    MsgArrs = msgArrs
                }, settings);
            }

            /// <summary>
            /// 添加消息
            /// </summary>
            /// <param name="msgArrs">消息项</param>
            /// <param name="settings">调用设置</param>
            public void SendMessages(UserMsgArray[] msgArrs, CallingSettings settings = null)
            {
                InvokeWithCheck(SendMessagesSr(msgArrs, settings));
            }

            /// <summary>
            /// 添加消息
            /// </summary>
            /// <param name="msgs">消息</param>
            /// <param name="toUsers">接收者</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendMessagesSr(Msg[] msgs, Users toUsers, CallingSettings settings = null)
            {
                return SendMessagesSr(new[] { UserMsgArray.Create(msgs, toUsers) }, settings);
            }

            /// <summary>
            /// 添加消息
            /// </summary>
            /// <param name="msgs">消息</param>
            /// <param name="toUsers">接收者</param>
            /// <param name="settings">调用设置</param>
            public void SendMessages(Msg[] msgs, Users toUsers, CallingSettings settings = null)
            {
                InvokeWithCheck(SendMessagesSr(msgs, toUsers, settings));
            }

            /// <summary>
            /// 添加消息
            /// </summary>
            /// <param name="message">消息</param>
            /// <param name="toUsers">接收者</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendMessageSr(Msg message, Users toUsers, CallingSettings settings = null)
            {
                var req = new Message_SendMessage_Request { Message = message, ToUsers = toUsers };
                return MessageService.SendMessage(Sc, req, settings);
            }

            /// <summary>
            /// 添加消息
            /// </summary>
            /// <param name="message">消息</param>
            /// <param name="toUsers">接收者</param>
            /// <param name="settings">调用设置</param>
            public void SendMessage(Msg message, Users toUsers, CallingSettings settings = null)
            {
                InvokeWithCheck(SendMessageSr(message, toUsers, settings));
            }

            /// <summary>
            /// 添加消息
            /// </summary>
            /// <param name="message">消息</param>
            /// <param name="toUserIds">接收者</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendMessageSr(Msg message, int[] toUserIds = null, CallingSettings settings = null)
            {
                return SendMessageSr(message, Users.FromUserIds(toUserIds), settings);
            }

            /// <summary>
            /// 添加消息
            /// </summary>
            /// <param name="message">消息</param>
            /// <param name="toUserIds">接收者</param>
            /// <param name="settings">调用设置</param>
            public void SendMessage(Msg message, int[] toUserIds = null, CallingSettings settings = null)
            {
                InvokeWithCheck(SendMessageSr(message, toUserIds, settings));
            }

            /// <summary>
            /// 批量删除消息
            /// </summary>
            /// <param name="msgIndexes">消息索引号</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveMessagesSr(long[] msgIndexes, CallingSettings settings = null)
            {
                return MessageService.RemoveMessages(Sc, new Message_RemoveMessages_Request() { MsgIndexes = msgIndexes }, settings);
            }

            /// <summary>
            /// 批量删除消息
            /// </summary>
            /// <param name="msgIndexes">消息索引号</param>
            /// <param name="settings">调用设置</param>
            public void RemoveMessages(long[] msgIndexes, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveMessagesSr(msgIndexes, settings));
            }

            /// <summary>
            /// 删除消息
            /// </summary>
            /// <param name="msgIndex">消息索引号</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveMessageSr(long msgIndex, CallingSettings settings = null)
            {
                return MessageService.RemoveMessage(Sc, new Message_RemoveMessage_Request { MsgIndex = msgIndex }, settings);
            }

            /// <summary>
            /// 删除消息
            /// </summary>
            /// <param name="msgIndex">消息索引号</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void RemoveMessage(long msgIndex, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveMessageSr(msgIndex, settings));
            }

            /// <summary>
            /// 获取消息
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Msg[]> GetMessageSr(int[] userIds, CallingSettings settings = null)
            {
                var sr = MessageService.GetMessages(Sc, new Message_GetMessages_Request() { UserIds = userIds }, settings);
                return CreateSr(sr, r => r.Msgs);
            }

            /// <summary>
            /// 获取消息
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Msg[] GetMessage(int[] userIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMessageSr(userIds, settings));
            }

            /// <summary>
            /// 获取消息
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<Msg[]> GetMessageSr(int userId, CallingSettings settings = null)
            {
                return GetMessageSr(new[] { userId }, settings);
            }

            /// <summary>
            /// 获取消息
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public Msg[] GetMessage(int userId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMessageSr(userId, settings));
            }

            /// <summary>
            /// 将指定用户持久化存储的消息重新发送
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ResendMessageSr(int[] userIds, CallingSettings settings = null)
            {
                return MessageService.ResendMessage(Sc, new Message_ResendMessage_Request { UserIds = userIds }, settings);
            }

            /// <summary>
            /// 将指定用户持久化存储的消息重新发送
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void ResendMessage(int[] userIds, CallingSettings settings = null)
            {
                InvokeWithCheck(ResendMessageSr(userIds, settings));
            }

            /// <summary>
            /// 将指定用户持久化存储的消息重新发送
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ResendMessageSr(int userId, CallingSettings settings = null)
            {
                return ResendMessageSr(new[] { userId }, settings);
            }

            /// <summary>
            /// 将指定用户持久化存储的消息重新发送
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void ResendMessage(int userId, CallingSettings settings = null)
            {
                InvokeWithCheck(ResendMessageSr(userId, settings));
            }
        }
    }
}
