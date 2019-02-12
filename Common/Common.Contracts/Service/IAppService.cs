using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 应用程序服务
    /// </summary>
    public interface IAppService : IObjectPropertyProvider, IDisposable
    {
        /// <summary>
        /// 初始化服务，并返回服务的信息，返回null则表明该服务不可用
        /// </summary>
        /// <param name="sp">与平台的通信接口</param>
        /// <param name="initModel">初始化方式</param>
        /// <returns>服务的信息</returns>
        AppServiceInfo Init(IServiceProvider sp, AppServiceInitModel initModel);

        /// <summary>
        /// 开始运行
        /// </summary>
        void Start();

        /// <summary>
        /// 停止运行
        /// </summary>
        void Stop();

        /// <summary>
        /// 状态
        /// </summary>
        AppServiceStatus Status { get; }

        /// <summary>
        /// 等待到指定的状态
        /// </summary>
        /// <param name="waitType"></param>
        /// <param name="millsecondTimeout"></param>
        /// <returns></returns>
        bool Wait(AppServiceWaitType waitType, int millsecondTimeout);

        /// <summary>
        /// 通信策略
        /// </summary>
        /// <returns></returns>
        ICommunicate Communicate { get; }
    }

    /// <summary>
    /// 服务的状态
    /// </summary>
    public enum AppServiceStatus
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Desc("初始状态")]
        Init,

        /// <summary>
        /// 正在运行
        /// </summary>
        [Desc("正在运行")]
        Running,

        /// <summary>
        /// 已停止
        /// </summary>
        [Desc("停止")]
        Stopped,
    }

    /// <summary>
    /// 服务的初始化方式
    /// </summary>
    public enum AppServiceInitModel
    {
        /// <summary>
        /// 运行
        /// </summary>
        Execute,

        /// <summary>
        /// 读取信息
        /// </summary>
        ReadInfo,
    }

    public enum AppServiceWaitType
    {
        /// <summary>
        /// 等待服务初始化完毕
        /// </summary>
        InitCompleted,

        /// <summary>
        /// 等待服务运行
        /// </summary>
        Running,
    }
}
