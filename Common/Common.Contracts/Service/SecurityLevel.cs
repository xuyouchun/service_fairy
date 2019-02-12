using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 安全级别
    /// </summary>
    public enum SecurityLevel : byte
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [Desc("未定义")]
        Undefined = 0,

        /// <summary>
        /// 公共权限
        /// </summary>
        [Desc("公共权限")]
        Public = 1,

        /// <summary>
        /// 普通用户权限
        /// </summary>
        [Desc("普通用户权限")]
        User = 32,

        /// <summary>
        /// 应用服务运行权限
        /// </summary>
        [Desc("应用服务运行权限")]
        AppRunningLevel = 128,

        /// <summary>
        /// 系统服务运行权限
        /// </summary>
        [Desc("系统服务运行权限")]
        SysRunningLevel = 160,

        /// <summary>
        /// 核心服务运行权限
        /// </summary>
        [Desc("核心服务运行权限")]
        CoreRunningLevel = 192,

        /// <summary>
        /// 管理员权限
        /// </summary>
        [Desc("管理员权限")]
        Admin = 224,

        /// <summary>
        /// 超级管理员权限
        /// </summary>
        [Desc("超级管理员权限")]
        SuperAdmin = 255,
    }
}
