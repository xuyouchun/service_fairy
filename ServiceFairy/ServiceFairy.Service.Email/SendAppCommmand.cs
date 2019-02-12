using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Email;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Service.Email
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    [AppCommand("Send", "发送邮件", SecurityLevel = SecurityLevel.AppRunningLevel), Remarks(Remarks)]
    class SendAppCommmand : ACS<Service>.Action<Email_Send_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Email_Send_Request request, ref ServiceResult sr)
        {
            
        }

        const string Remarks = @"发送邮件";
    }
}
