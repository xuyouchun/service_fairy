using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts;
using System.Configuration;
using Common.Utility;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace Common.Framework.TrayPlatform
{
    partial class TrayAppServiceManager
    {
        /// <summary>
        /// 配置文件读取器
        /// </summary>
        class TrayConfiguration : MarshalByRefObjectEx, ITrayConfiguration
        {
            public TrayConfiguration(TrayAppServiceManager owner, string configuration)
            {
                _owner = owner;
                _configuration = configuration;
            }

            private string _configuration;
            private readonly TrayAppServiceManager _owner;
            private readonly HashSet<ITrayConfigurationChangedNotify> _notifiers = new HashSet<ITrayConfigurationChangedNotify>();

            /// <summary>
            /// 更新配置文件的内容
            /// </summary>
            /// <param name="configuration"></param>
            public void UpdateConfiguration(string configuration)
            {
                lock (_notifiers)
                {
                    if (_configuration == configuration)
                        return;

                    string old = _configuration;
                    _configuration = configuration;
                    foreach (ITrayConfigurationChangedNotify notify in _notifiers)
                    {
                        try
                        {
                            notify.Notify(old, configuration);
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogError(ex);
                        }
                    }
                }
            }

            /// <summary>
            /// 获取配置文件的内容
            /// </summary>
            /// <returns></returns>
            public string GetConfiguration()
            {
                return _configuration;
            }

            /// <summary>
            /// 寻找指定服务的配置管理器
            /// </summary>
            /// <param name="serviceDesc"></param>
            /// <returns></returns>
            public ITrayConfiguration FindTrayConfiguration(ServiceDesc serviceDesc)
            {
                if (serviceDesc == null)
                    return this;

                TrayAppServiceInfo si = _owner.GetServiceInfo(serviceDesc);
                return si == null ? null : si.Configuration;
            }

            /// <summary>
            /// 注册一个变化通知
            /// </summary>
            /// <param name="notifier"></param>
            public void RegisterChangedNotify(ITrayConfigurationChangedNotify notifier)
            {
                Contract.Requires(notifier != null);

                lock (_notifiers)
                {
                    _notifiers.Add(notifier);
                }
            }

            /// <summary>
            /// 取消注册一个变化通知
            /// </summary>
            /// <param name="notifier"></param>
            public void UnregisterChangedNotify(ITrayConfigurationChangedNotify notifier)
            {
                Contract.Requires(notifier != null);

                lock (_notifiers)
                {
                    _notifiers.Remove(notifier);
                }
            }
        }
    }
}
