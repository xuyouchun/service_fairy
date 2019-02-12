using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package.Service;
using Common.Algorithms;
using ServiceFairy.SystemInvoke;
using Common.Framework.TrayPlatform;
using Common.Utility;
using Common.Collection;
using Common.Package;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 一致性哈希算法服务节点管理器
    /// </summary>
    [AppComponent("一致性哈希算法服务节点管理器", "用于实时更新服务节点，作为一致性哈希算法的参数", AppComponentCategory.Application)]
    public class ServiceConsistentNodeManagerAppComponent : TimerAppComponentBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">服务</param>
        /// <param name="serviceDesc"></param>
        public ServiceConsistentNodeManagerAppComponent(CoreAppServiceBase service, ServiceDesc serviceDesc = null)
            : base(service, TimeSpan.FromSeconds(5))
        {
            Contract.Requires(service != null);

            _service = service;
            _serviceDesc = serviceDesc;
        }

        private readonly CoreAppServiceBase _service;
        private readonly ServiceDesc _serviceDesc;
        private readonly ConsistentHashing<ServiceConsistentNode> _ch = new ConsistentHashing<ServiceConsistentNode>();

        /// <summary>
        /// 根据哈希码获取节点
        /// </summary>
        /// <param name="hashcode"></param>
        /// <returns></returns>
        public ServiceConsistentNode GetNode(int hashcode)
        {
            if (_ch.GetAllNodes().Length == 0)
                return null;

            return _ch.GetNode(hashcode);
        }

        protected override void OnExecuteTask(string taskName)
        {
            AppInvokeInfo[] invokeInfos = _service.CloudManager.SearchServices(_serviceDesc ?? _service.Context.ServiceDesc);
            _ch.InsteadOf(invokeInfos.Select(v => _ConvertToConsistentNode(v)).ToArray());
        }

        private ServiceConsistentNode _ConvertToConsistentNode(AppInvokeInfo info)
        {
            return new ServiceConsistentNode(info.ClientID);
        }

        /// <summary>
        /// 获取所有终端ID
        /// </summary>
        /// <returns></returns>
        public Guid[] GetAllClientIds()
        {
            return _ch.GetAllNodes().ToArray(node => node.ClientID);
        }

        /// <summary>
        /// 将对象按路由位置分组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="hashcodeSelector"></param>
        /// <returns></returns>
        public IGrouping<Guid, T>[] Group<T>(IEnumerable<T> items, Func<T, int> hashcodeSelector = null)
        {
            Contract.Requires(items != null);

            if (hashcodeSelector == null)
                hashcodeSelector = (item) => item.GetHashCode();

            var list = from item in items
                       let node = GetNode(hashcodeSelector(item))
                       where node != null
                       group item by node.ClientID;

            return list.ToArray();
        }

        /// <summary>
        /// 对指定的对象集合按路由位置分组，然后应用不同的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="localAction"></param>
        /// <param name="remoteAction"></param>
        /// <param name="hashcodeSelector"></param>
        public void Apply<T>(IEnumerable<T> items, Action<T[]> localAction, Action<Guid, T[]> remoteAction,
            Func<T, int> hashcodeSelector = null)
        {
            foreach (var group in Group(items, hashcodeSelector))
            {
                Guid clientId = group.Key;
                if (clientId == _service.Context.ClientID)
                {
                    if (localAction != null)
                        localAction(group.ToArray());
                }
                else
                {
                    if (remoteAction != null)
                        remoteAction(clientId, group.ToArray());
                }
            }
        }

        /// <summary>
        /// 对指定的对象集合按路由位置分组，收集运算结果
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <typeparam name="TRet"></typeparam>
        /// <param name="items"></param>
        /// <param name="localLoader"></param>
        /// <param name="remoteLoader"></param>
        /// <param name="hashcodeSelector"></param>
        public TRet[] Collect<TArg, TRet>(IEnumerable<TArg> items, Func<TArg[], TRet[]> localLoader = null,
            Func<Guid, TArg[], TRet[]> remoteLoader = null, Func<TArg, int> hashcodeSelector = null)
        {
            List<TRet> rets = new List<TRet>();
            foreach (var group in Group(items, hashcodeSelector))
            {
                Guid clientId = group.Key;
                if (clientId == _service.Context.ClientID)
                {
                    if (localLoader != null)
                        rets.AddRange(localLoader(group.ToArray()));
                }
                else
                {
                    if (remoteLoader != null)
                        rets.AddRange(remoteLoader(clientId, group.ToArray()));
                }
            }

            return rets.ToArray();
        }

        /// <summary>
        /// 对指定的对象集合按路由位置分组，收集运算结果，并以哈希表的形式返回
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <typeparam name="TRet"></typeparam>
        /// <param name="items"></param>
        /// <param name="localLoader"></param>
        /// <param name="remoteLoader"></param>
        /// <param name="hashcodeSelector"></param>
        /// <returns></returns>
        public IDictionary<Guid, TRet[]> CollectDict<TArg, TRet>(IEnumerable<TArg> items, Func<TArg[], TRet[]> localLoader = null,
            Func<Guid, TArg[], TRet[]> remoteLoader = null, Func<TArg, int> hashcodeSelector = null)
        {
            Dictionary<Guid, TRet[]> rets = new Dictionary<Guid, TRet[]>();
            foreach (var group in Group(items, hashcodeSelector))
            {
                Guid clientId = group.Key;
                if (clientId == _service.Context.ClientID)
                {
                    if (localLoader != null)
                        rets.Add(clientId, localLoader(group.ToArray()));
                }
                else
                {
                    if (remoteLoader != null)
                        rets.Add(clientId, remoteLoader(clientId, group.ToArray()));
                }
            }

            return rets;
        }

        /// <summary>
        /// 收集所有终端上的数据
        /// </summary>
        /// <typeparam name="TRet"></typeparam>
        /// <param name="localLoader"></param>
        /// <param name="remoteLoader"></param>
        /// <returns></returns>
        public TRet[] Collect<TRet>(Func<TRet[]> localLoader = null, Func<Guid, TRet[]> remoteLoader = null)
        {
            List<TRet> rets = new List<TRet>();
            foreach (Guid clientId in GetAllClientIds())
            {
                if (clientId == _service.Context.ClientID)
                {
                    if (localLoader != null)
                        rets.AddRange(localLoader());
                }
                else
                {
                    if (remoteLoader != null)
                        rets.AddRange(remoteLoader(clientId));
                }
            }

            return rets.ToArray();
        }
    }
}
