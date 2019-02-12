using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Net;
using Common.Contracts;
using System.Diagnostics.Contracts;
using Common.Communication.Wcf;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// ServiceManager的回调接口
    /// </summary>
    public interface IServiceManagerCallback
    {
        /// <summary>
        /// 启动侦听
        /// </summary>
        /// <param name="option">通信选项</param>
        void StartListener(CommunicationOption option);

        /// <summary>
        /// 保存服务的调用
        /// </summary>
        /// <param name="serverInfos"></param>
        void UpdateInvokeInfos(AppInvokeInfo[] serverInfos);

        /// <summary>
        /// 获取调用列表
        /// </summary>
        /// <returns></returns>
        AppInvokeInfo[] GetInvokeInfos();

        /// <summary>
        /// 获取所有的通信方式
        /// </summary>
        /// <returns></returns>
        CommunicationOption[] GetAllCommunicateOptions();

        /// <summary>
        /// 停止指定的侦听
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        bool StopListener(ServiceAddress address);

        /// <summary>
        /// 创建通道工厂
        /// </summary>
        /// <returns></returns>
        ICommunicateFactory CreateCommunicateFactory();

        /// <summary>
        /// 创建状态管理器
        /// </summary>
        /// <returns></returns>
        ITraySessionStateManager CreateTraySessionStateManager();

        /// <summary>
        /// 代理是否处于启用状态
        /// </summary>
        bool ProxyEnabled { get; }

        /// <summary>
        /// 确保代理处于开启状态
        /// </summary>
        /// <param name="owner"></param>
        void KeepProxyEnable(object owner);

        /// <summary>
        /// 禁用代理
        /// </summary>
        /// <param name="owner"></param>
        void DisableProxy(object owner);

        /// <summary>
        /// 重新启动平台
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="args"></param>
        void DoCommand(string commandName, string[] args);
    }
}
