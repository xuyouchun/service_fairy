using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 统计服务
    /// </summary>
    public static class StatisticsService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.Statistics + "/" + method;
        }

        
    }
}
