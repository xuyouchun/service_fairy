using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using System.ServiceModel.Description;
using System.ServiceModel;
using Common.Contracts.Service;
using Common.Communication.Wcf.Encoders;
using Common.Contracts;
using Common.Communication.Wcf.Strategies.WcfConnectionStrategies;

namespace Common.Communication.Wcf.Strategies
{
    /// <summary>
    /// 连接策略
    /// </summary>
    interface IWcfCommunicationStrategy
    {
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        IWcfServiceInterface CreateServiceInterface(WcfConnection owner, CommunicationOption option);

        /// <summary>
        /// 添加一个连接点
        /// </summary>
        /// <param name="serviceHost"></param>
        /// <param name="listener"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        ServiceHost CreateServiceHost(WcfListener listener, CommunicationOption option);

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        WcfConnectionBase CreateConnection(WcfListener listener, CommunicationOption option);

        /// <summary>
        /// 处理调用请求
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="input"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        EntityMessage Invoke(WcfListener listener, EntityMessage input);

        /// <summary>
        /// 创建指定实体的消息
        /// </summary>
        /// <param name="option">通信设置</param>
        /// <param name="context">上下文环境</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        EntityMessage CreateMessage(CommunicationOption option, CommunicateContext context, CallingSettings settings);
    }

    /// <summary>
    /// 用于标记连接策略
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    class WcfCommunicationStrategyAttribute : Attribute
    {
        public WcfCommunicationStrategyAttribute(CommunicationType type)
        {
            Type = type;
        }

        /// <summary>
        /// 通信策略
        /// </summary>
        public CommunicationType Type { get; private set; }
    }
}
