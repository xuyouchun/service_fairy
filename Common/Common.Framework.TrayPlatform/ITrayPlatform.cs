using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Common.Communication.Wcf;
using Common.Contracts.Service;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 平台服务接口
    /// </summary>
    public interface ITrayPlatform
    {
        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetData(string key, object value);

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetData(string key);

        /// <summary>
        /// 状态变化通知
        /// </summary>
        /// <param name="status"></param>
        void StatusChangedNotify(AppServiceStatus status);

        /// <summary>
        /// 部署并启动相应的服务
        /// </summary>
        /// <param name="serviceAseemblyFile"></param>
        /// <param name="configuration"></param>
        /// <param name="autoStart"></param>
        ServiceInfo LoadService(string serviceAseemblyFile, string configuration = null, bool autoStart = true);

        /// <summary>
        /// 卸载Service
        /// </summary>
        /// <param name="serviceDesc"></param>
        bool UnloadService(ServiceDesc serviceDesc);

        /// <summary>
        /// 设置服务的状态
        /// </summary>
        /// <param name="serviceDesc">服务的描述</param>
        /// <param name="status">服务的状态</param>
        void SetServiceStatus(ServiceDesc serviceDesc, AppServiceStatus status);

        /// <summary>
        /// 设置服务的有效状态
        /// </summary>
        /// <param name="serviceDesc">服务的状态</param>
        /// <param name="avaliable">服务的有效状态</param>
        void SetServiceAvaliable(ServiceDesc serviceDesc, bool avaliable);

        /// <summary>
        /// 开启监听
        /// </summary>
        /// <param name="option"></param>
        void StartListener(CommunicationOption option);

        /// <summary>
        /// 停止一个监听
        /// </summary>
        /// <param name="address"></param>
        void StopListener(ServiceAddress address);

        /// <summary>
        /// 保存调用列表
        /// </summary>
        /// <param name="serverInfos">调用列表</param>
        void UpdateInvokeInfos(AppInvokeInfo[] serverInfos);

        /// <summary>
        /// 获取调用列表
        /// </summary>
        /// <returns></returns>
        AppInvokeInfo[] GetInvokeInfos();

        /// <summary>
        /// 获取所有的服务
        /// </summary>
        /// <returns></returns>
        ServiceInfo[] GetAllServiceInfos();

        /// <summary>
        /// 获取所有的通信方式
        /// </summary>
        /// <returns></returns>
        CommunicationOption[] GetAllCommunicationOptions();

        /// <summary>
        /// 注册一个对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        void RegisterHandler(Type type, object handler);

        /// <summary>
        /// 取消注册一个对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        void UnregisterHandler(Type type, object handler);

        /// <summary>
        /// 获取运行目录
        /// </summary>
        /// <returns></returns>
        string GetRunningPath();

        /// <summary>
        /// 重启平台
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="args"></param>
        void DoCommand(string commandName, string[] args);
    }
}
