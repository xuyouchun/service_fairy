using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Email;

namespace ServiceFairy.Service.Email.Components
{
    /// <summary>
    /// 邮件发送器
    /// </summary>
    [AppComponent("邮件发送器", "发送邮件")]
    class EmailSenderAppComponent : AppComponent
    {
        public EmailSenderAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        public void SendEmail(EmailItem email)
        {

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="emails"></param>
        public void SendEmails(EmailItem[] emails)
        {

        }
    }
}
