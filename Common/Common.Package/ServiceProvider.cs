using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Collection;
using System.Collections;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Contracts.Service;

namespace Common.Package
{
    /// <summary>
    /// 服务的容器
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class ServiceProvider : MarshalByRefObjectEx, IEnumerable, IServiceProvider
    {
        public ServiceProvider()
        {

        }

        private readonly ThreadSafeDictionaryWrapper<Type, object> _dict = new ThreadSafeDictionaryWrapper<Type, object>(new Dictionary<Type, object>());

        /// <summary>
        /// 添加一个服务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="service"></param>
        public void AddService(Type type, object service)
        {
            Contract.Requires(type != null && service != null);

            _dict.Add(type, service);
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            Contract.Requires(serviceType != null);

            object svr;
            if (_dict.TryGetValue(serviceType, out svr))
                return svr;

            return null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dict.Values.GetEnumerator();
        }

        /// <summary>
        /// 组合多个服务
        /// </summary>
        /// <param name="sps"></param>
        /// <returns></returns>
        public static IServiceProvider Combine(params IServiceProvider[] sps)
        {
            return new ServiceProviderCombine(sps);
        }

        #region Class ServiceProviderCombine ...

        class ServiceProviderCombine : MarshalByRefObjectEx, IServiceProvider
        {
            public ServiceProviderCombine(IServiceProvider[] sps)
            {
                _sps = sps;
            }

            private readonly IServiceProvider[] _sps;

            public object GetService(Type serviceType)
            {
                for (int k = 0; k < _sps.Length; k++)
                {
                    object svr = _sps[k].GetService(serviceType);
                    if (svr != null)
                        return svr;
                }

                return null;
            }
        }

        #endregion

    }
}
