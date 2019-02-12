using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Email;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private EmailInvoker _email;

        /// <summary>
        /// Email Service
        /// </summary>
        public EmailInvoker Email
        {
            get { return _email ?? (_email = new EmailInvoker(this)); }
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public class EmailInvoker : Invoker
        {
            public EmailInvoker(SystemInvoker owner)
                : base(owner)
            {

            }

            /// <summary>
            /// 发送邮件
            /// </summary>
            /// <param name="emails">邮件</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendSr(EmailItem[] emails, CallingSettings settings = null)
            {
                return EmailService.Send(Sc, new Email_Send_Request { Emails = emails }, settings);
            }

            /// <summary>
            /// 发送邮件
            /// </summary>
            /// <param name="emails">邮件</param>
            /// <param name="settings">调用设置</param>
            public void Send(EmailItem[] emails, CallingSettings settings = null)
            {
                InvokeWithCheck(SendSr(emails, settings));
            }

            /// <summary>
            /// 发送邮件
            /// </summary>
            /// <param name="email">邮件</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendSr(EmailItem email, CallingSettings settings = null)
            {
                Contract.Requires(email != null);

                return SendSr(new[] { email }, settings);
            }

            /// <summary>
            /// 发送邮件
            /// </summary>
            /// <param name="email">邮件</param>
            /// <param name="settings">调用设置</param>
            public void Send(EmailItem email, CallingSettings settings = null)
            {
                InvokeWithCheck(SendSr(email, settings));
            }

            /// <summary>
            /// 发送邮件
            /// </summary>
            /// <param name="to">接收者</param>
            /// <param name="title">标题</param>
            /// <param name="body">内容</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult SendSr(string to, string title, string body, CallingSettings settings = null)
            {
                Contract.Requires(to != null);
                return SendSr(new EmailItem { To = new[] { to }, Title = title, Body = body }, settings);
            }

            /// <summary>
            /// 发送邮件
            /// </summary>
            /// <param name="to">接收者</param>
            /// <param name="title">标题</param>
            /// <param name="body">内容</param>
            /// <param name="settings">调用设置</param>
            public void Send(string to, string title, string body, CallingSettings settings = null)
            {
                InvokeWithCheck(SendSr(to, title, body, settings));
            }
        }
    }
}
