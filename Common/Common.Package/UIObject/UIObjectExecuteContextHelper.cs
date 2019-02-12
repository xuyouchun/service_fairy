using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Utility;
using Common.Contracts;

namespace Common.Package.UIObject
{
    public class UIObjectExecuteContextHelper
    {
        public UIObjectExecuteContextHelper(IUIObjectExecuteContext executeContext)
        {
            Contract.Requires(executeContext != null);

            _executeContext = executeContext;
        }

        private readonly IUIObjectExecuteContext _executeContext;

        /// <summary>
        /// 获取指定类型的服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public T GetService<T>(bool throwError = false)
            where T : class
        {
            return _executeContext.ServiceProvider.GetService<T>(throwError);
        }

        /// <summary>
        /// 获取当前对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetCurrent<T>()
            where T : class
        {
            var p = _executeContext.ServiceProvider.GetService<ICurrentObjectRecorder<T>>();
            if (p == null)
                return null;

            return p.GetCurrent();
        }

        /// <summary>
        /// 设置当前对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="executeContext"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IUIObjectExecuteContext SetCurrent<T>(IUIObjectExecuteContext executeContext, T item)
            where T : class
        {
            Contract.Requires(executeContext != null);

            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(ICurrentObjectRecorder<T>), new CurrentObjectRecorder<T>(item));

            return new UIObjectExecuteContextDecorate(executeContext, sp);
        }

        /// <summary>
        /// 当前的树节点
        /// </summary>
        interface ICurrentObjectRecorder<T>
            where T : class
        {
            T GetCurrent();
        }

        #region Class CurrentObjectRecorder ...

        class CurrentObjectRecorder<T> : MarshalByRefObjectEx, ICurrentObjectRecorder<T>
            where T : class
        {
            public CurrentObjectRecorder(T item)
            {
                _item = item;
            }

            private readonly T _item;

            public T GetCurrent()
            {
                return _item;
            }
        }

        #endregion

        #region Class UIObjectExecuteContextDecorate ...

        class UIObjectExecuteContextDecorate : MarshalByRefObjectEx, IUIObjectExecuteContext
        {
            public UIObjectExecuteContextDecorate(IUIObjectExecuteContext executeContext, IServiceProvider sp)
            {
                _serviceProvider = Common.Package.ServiceProvider.Combine(new IServiceProvider[] { executeContext.ServiceProvider, sp });
            }

            private readonly IServiceProvider _serviceProvider;

            public IServiceProvider ServiceProvider
            {
                get { return _serviceProvider; }
            }
        }

        #endregion

    }
}
