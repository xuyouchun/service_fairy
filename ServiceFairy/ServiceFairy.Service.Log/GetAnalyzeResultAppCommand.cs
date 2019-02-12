using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Log;

namespace ServiceFairy.Service.Log
{
    /// <summary>
    /// 获取日志分析结果
    /// </summary>
    [AppCommand("GetAnalyzeResult", "获取日志分析结果")]
    class GetAnalyzeResultAppCommand : ACS<Service>.Func<Log_GetAnalyzeResult_Reply>
    {
        protected override Log_GetAnalyzeResult_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}
