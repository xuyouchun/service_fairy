using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Communication.Wcf.Strategies.WcfConnectionStrategies;
using System.ServiceModel.Description;
using System.ServiceModel;
using Common.Communication.Wcf.Service;
using Common.Contracts.Service;
using Common.Communication.Wcf.Encoders;
using Common.Contracts;

namespace Common.Communication.Wcf.Strategies
{
    /// <summary>
    /// WCF各种策略的创建工厂
    /// </summary>
    static class WcfFactory
    {
        static WcfFactory()
        {
            _communicationStrageties = typeof(WcfFactory).Assembly.SearchTypes<CommunicationType, WcfCommunicationStrategyAttribute>((attrs, t) => attrs[0].Type)
                .ToDictionary(item => item.Key, item => new WcfCommunicationStrategyProxy(item.Value));
        }

        private static readonly Dictionary<CommunicationType, WcfCommunicationStrategyProxy> _communicationStrageties;

        #region Class WcfCommunicationStrategyProxy ...

        [System.Diagnostics.DebuggerStepThrough]
        class WcfCommunicationStrategyProxy : IWcfCommunicationStrategy
        {
            public WcfCommunicationStrategyProxy(Type strategyType)
            {
                _strategyType = strategyType;
            }

            private readonly Type _strategyType;
            private IWcfCommunicationStrategy _strategy;

            private IWcfCommunicationStrategy _GetStrategy()
            {
                if (_strategy == null)
                    _strategy = (IWcfCommunicationStrategy)Activator.CreateInstance(_strategyType);

                return _strategy;
            }

            public Service.IWcfServiceInterface CreateServiceInterface(WcfConnection owner, CommunicationOption option)
            {
                return _GetStrategy().CreateServiceInterface(owner, option);
            }

            public ServiceHost CreateServiceHost(WcfListener listener, CommunicationOption option)
            {
                return _GetStrategy().CreateServiceHost(listener, option);
            }

            public EntityMessage Invoke(WcfListener listener, EntityMessage input)
            {
                return _GetStrategy().Invoke(listener, input);
            }

            public EntityMessage CreateMessage(CommunicationOption option, CommunicateContext context, CallingSettings settings)
            {
                return _GetStrategy().CreateMessage(option, context, settings);
            }

            public WcfConnectionBase CreateConnection(WcfListener listener, CommunicationOption option)
            {
                return _GetStrategy().CreateConnection(listener, option);
            }
        }

        #endregion

        /// <summary>
        /// 创建连接策略
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IWcfCommunicationStrategy CreateConnectionStrategy(CommunicationType type)
        {
            IWcfCommunicationStrategy strategy = _communicationStrageties.GetOrDefault(type);
            if (strategy == null)
                throw new NotSupportedException("不支持连接方式" + type);

            return strategy;
        }
    }
}
