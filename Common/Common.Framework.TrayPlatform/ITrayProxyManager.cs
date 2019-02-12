using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// AppService的代理管理器
    /// </summary>
    public interface ITrayProxyManager
    {
        /// <summary>
        /// 当前代理是否启用
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// 维持代理的启用状态
        /// </summary>
        void EnsureEnable();

        /// <summary>
        /// 禁用代理
        /// </summary>
        void Disable();
    }
}
