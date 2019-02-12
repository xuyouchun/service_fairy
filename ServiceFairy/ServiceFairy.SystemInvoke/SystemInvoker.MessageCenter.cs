using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.Entities;
using ServiceFairy.Entities.MessageCenter;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private MessageCenterInvoker _messageCenter;

        /// <summary>
        /// MessageCenter Service
        /// </summary>
        public MessageCenterInvoker MessageCenter
        {
            get { return _messageCenter ?? (_messageCenter = new MessageCenterInvoker(this)); }
        }

        /// <summary>
        /// MesasgeCenter Service
        /// </summary>
        public class MessageCenterInvoker : Invoker
        {
            public MessageCenterInvoker(SystemInvoker owner)
                : base(owner)
            {

            }

            /// <summary>
            /// 批量持久化存储消息
            /// </summary>
            /// <param name="msgArrs">消息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SaveSr(UserMsgArray[] msgArrs, CallingSettings settings = null)
            {
                var req = new MessageCenter_Save_Request { MsgArrs = msgArrs };
                return MessageCenterService.Save(Sc, req);
            }

            /// <summary>
            /// 批量持久化存储消息
            /// </summary>
            /// <param name="msgArrs">消息</param>
            /// <param name="settings">调用设置</param>
            public void Save(UserMsgArray[] msgArrs, CallingSettings settings = null)
            {
                InvokeWithCheck(SaveSr(msgArrs, settings));
            }

            /// <summary>
            /// 读取用户持久化的消息
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>消息</returns>
            public ServiceResult<Msg[]> ReadSr(int userId, CallingSettings settings = null)
            {
                var sr = MessageCenterService.Read(Sc, new MessageCenter_Read_Request {
                    UserId = userId
                }, settings);

                return CreateSr(sr, r => r.Msgs);
            }

            /// <summary>
            /// 读取用户持久化的消息
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>消息</returns>
            public Msg[] Read(int userId, CallingSettings settings = null)
            {
                return InvokeWithCheck(ReadSr(userId, settings));
            }

            /// <summary>
            /// 清空指定用户持久化的消息
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ClearSr(int[] userIds, CallingSettings settings = null)
            {
                return MessageCenterService.Clear(Sc, new MessageCenter_Clear_Request { UserIds = userIds }, settings);
            }

            /// <summary>
            /// 清空指定用户持久化的消息
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void Clear(int[] userIds, CallingSettings settings = null)
            {
                InvokeWithCheck(ClearSr(userIds, settings));
            }

            /// <summary>
            /// 清空指定用户持久化的消息
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ClearSr(int userId, CallingSettings settings = null)
            {
                return ClearSr(new[] { userId }, settings);
            }

            /// <summary>
            /// 清空指定用户持久化的消息
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void Clear(int userId, CallingSettings settings = null)
            {
                InvokeWithCheck(ClearSr(userId, settings));
            }
        }
    }
}
