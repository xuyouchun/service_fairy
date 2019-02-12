using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.Contracts.Service;

namespace Common
{
    /// <summary>
    /// 全局对象
    /// </summary>
    static class Global
    {
        /// <summary>
        /// 全局定时器
        /// </summary>
        public static readonly GlobalTimer<ITask> GlobalTimer = GlobalTimer<ITask>.Default;
    }
}
