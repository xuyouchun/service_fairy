using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceFairy.Entities.Tray
{
    public enum TrayStatusCode
    {
        [Desc("Error")]
        Error = SFStatusCodes.Tray,

        /// <summary>
        /// 文件不存在
        /// </summary>
        [Desc("文件不存在")]
        FileNotFound = Error | 1,

        /// <summary>
        /// 目录不存在
        /// </summary>
        [Desc("目录不存在")]
        DirectoryNotFound = Error | 2,

        /// <summary>
        /// 不允许结束当前进程
        /// </summary>
        [Desc("不允许结束当前进程")]
        CannotKillCurrentProcess = Error | 3,
    }
}
