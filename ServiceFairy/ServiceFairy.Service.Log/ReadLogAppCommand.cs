using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Log;

namespace ServiceFairy.Service.Log
{
    /// <summary>
    /// 读取日志
    /// </summary>
    [AppCommand("ReadLog", "读取日志")]
    class ReadLogAppCommand : ACS<Service>.Func<Log_ReadLog_Request, Log_ReadLog_Reply>
    {
        protected override Log_ReadLog_Reply OnExecute(AppCommandExecuteContext<Service> context, Log_ReadLog_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}
