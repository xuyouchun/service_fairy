using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Security;

namespace ServiceFairy.Service.Security
{
    /// <summary>
    /// 修改密码
    /// </summary>
    [AppCommand("ModifyPassword", "修改密码", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ModifyPasswordAppCommand : ACS<Service>.Action<Security_ModifyPassword_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Security_ModifyPassword_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}
