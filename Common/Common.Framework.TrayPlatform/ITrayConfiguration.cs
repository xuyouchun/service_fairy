using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public interface ITrayConfiguration
    {
        /// <summary>
        /// 获取配置文件的内容
        /// </summary>
        /// <returns></returns>
        string GetConfiguration();

        /// <summary>
        /// 更新配置文件的内容
        /// </summary>
        /// <param name="configuration"></param>
        void UpdateConfiguration(string configuration);

        /// <summary>
        /// 寻找指定服务的配置管理器
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        ITrayConfiguration FindTrayConfiguration(ServiceDesc serviceDesc);

        /// <summary>
        /// 注册一个变化通知函数
        /// </summary>
        /// <param name="notifier"></param>
        void RegisterChangedNotify(ITrayConfigurationChangedNotify notifier);

        /// <summary>
        /// 取消注册一个变化通知函数
        /// </summary>
        /// <param name="notifier"></param>
        void UnregisterChangedNotify(ITrayConfigurationChangedNotify notifier);
    }

    /// <summary>
    /// 变化通知函数
    /// </summary>
    public interface ITrayConfigurationChangedNotify
    {
        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="old"></param>
        /// <param name="new"></param>
        void Notify(string old, string @new);
    }
}
