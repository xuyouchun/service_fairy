using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// AppService的启动方式
    /// </summary>
    public enum TrayAppServiceStartType
    {
        /// <summary>
        /// 不启动
        /// </summary>
        NotStart,

        /// <summary>
        /// 同步启动
        /// </summary>
        SyncStart,

        /// <summary>
        /// 异步启动
        /// </summary>
        AsyncStart,
    }
}
