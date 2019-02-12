using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy
{
    /// <summary>
    /// 各服务的状态码
    /// </summary>
    public enum SFStatusCodes
    {
        BusinessError = ServiceStatusCode.BusinessError,

        Tray = BusinessError | (10 << 16),

        Master = BusinessError | (11 << 16),

        Register = BusinessError | (12 << 16),

        Configuration = BusinessError | (13 << 16),

        Deploy = BusinessError | (14 << 16),

        Security = BusinessError | (15 << 16),

        Log = BusinessError | (64 << 16),

        Proxy = BusinessError | (65 << 16),

        Navigation = BusinessError | (66 << 16),

        Watch = BusinessError | (67 << 16),

        Cache = BusinessError | (68 << 16),

        Share = BusinessError | (69 << 16),

        Database = BusinessError | (70 << 16),

        DatabaseCenter = BusinessError | (71 << 16),

        User = BusinessError | (72 << 16),

        UserCenter = BusinessError | (73 << 16),

        Queue = BusinessError | (74 << 16),

        Sms = BusinessError | (75 << 16),

        File = BusinessError | (76 << 16),

        Message = BusinessError | (77 << 16),

        Group = BusinessError | (78 << 16),

        Test = BusinessError | (79 << 16),

        Email = BusinessError | (80 << 16),

        Statistics = BusinessError | (81 << 16),
    }
}
