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
    /// 邮件池
    /// </summary>
    [AppComponent("邮件池", "存储邮件用于定时发送")]
    class EmailPoolAppComponent : TimerAppComponentBase
    {
        public EmailPoolAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        protected override void OnExecuteTask(string taskName)
        {
            
        }

        /// <summary>
        /// 添加邮件
        /// </summary>
        /// <param name="email"></param>
        public void AddEmail(EmailItem email)
        {

        }

        /// <summary>
        /// 批量添加邮件
        /// </summary>
        /// <param name="emails"></param>
        public void AddEmails(EmailItem[] emails)
        {

        }
    }
}
