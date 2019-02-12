using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities.Message;
using Common.Contracts.Service;
using ServiceFairy.Entities.Addins.MesssageSubscript;
using ServiceFairy.Entities;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private MesasgeSubscriptInvoker _messageSubscript;

        /// <summary>
        /// MessageSubscript Addin
        /// </summary>
        public MesasgeSubscriptInvoker MesasgeSubscript
        {
            get { return _messageSubscript ?? (_messageSubscript = new MesasgeSubscriptInvoker(this)); }
        }

        public class MesasgeSubscriptInvoker : Invoker
        {
            public MesasgeSubscriptInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 消息通知
            /// </summary>
            /// <param name="userMsgs">用户消息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ApplyMessageSr(UserMsgItem[] userMsgs, CallingSettings settings = null)
            {
                return MessageSubscriptAddin.ApplyMessage(Sc,
                    new MesasgeSubscript_ApplyMessage_Request() { UserMsgs = userMsgs }, settings);
            }

            /// <summary>
            /// 消息通知
            /// </summary>
            /// <param name="userMsgs">用户消息</param>
            /// <param name="settings">调用设置</param>
            public void ApplyMessage(UserMsgItem[] userMsgs, CallingSettings settings = null)
            {
                InvokeWithCheck(ApplyMessageSr(userMsgs, settings));
            }

            /// <summary>
            /// 消息通知
            /// </summary>
            /// <param name="userMsg">用户消息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ApplyMessageSr(UserMsgItem userMsg, CallingSettings settings = null)
            {
                return ApplyMessageSr(new[] { userMsg }, settings);
            }

            /// <summary>
            /// 消息通知
            /// </summary>
            /// <param name="userMsg">用户消息</param>
            /// <param name="settings">调用设置</param>
            public void ApplyMessage(UserMsgItem userMsg, CallingSettings settings = null)
            {
                InvokeWithCheck(ApplyMessageSr(userMsg, settings));
            }
        }
    }
}
