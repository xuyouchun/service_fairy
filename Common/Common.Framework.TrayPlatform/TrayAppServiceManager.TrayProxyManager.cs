using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Framework.TrayPlatform
{
    partial class TrayAppServiceManager
    {
        /// <summary>
        /// 代理服务管理器
        /// </summary>
        class TrayProxyManager : MarshalByRefObjectEx, ITrayProxyManager
        {
            public TrayProxyManager(TrayAppServiceManager owner)
            {
                _owner = owner;
            }

            private readonly TrayAppServiceManager _owner;

            /// <summary>
            /// 是否启用代理服务
            /// </summary>
            public bool Enabled
            {
                get { return _owner._callback.ProxyEnabled; }
            }

            /// <summary>
            /// 确保代理处于开启状态
            /// </summary>
            public void EnsureEnable()
            {
                _owner._callback.KeepProxyEnable(this);
            }

            /// <summary>
            /// 关闭代理
            /// </summary>
            public void Disable()
            {
                _owner._callback.DisableProxy(this);
            }
        }

    }
}
