using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Log;

namespace ServiceFairy.Service.Log
{
    /// <summary>
    /// 删除日志
    /// </summary>
    [AppCommand("DeleteLog", "删除日志")]
    class DeleteLogAppCommand : ACS<Service>.Action<Log_DeleteLog_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Log_DeleteLog_Request request, ref ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}
