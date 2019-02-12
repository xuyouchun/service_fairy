using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common;
using Common.Package;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Package.Service
{
    /// <summary>
    /// 组件的管理器
    /// </summary>
    [AppComponent("组件管理器", "管理所有的组件的生命周期", AppComponentCategory.System, "Sys_AppComponentManager")]
    public class AppComponentManager : AppComponent, IDisposable
    {
        public AppComponentManager(IAppService service)
            : base(service)
        {
            this.Add(this);
        }

        private readonly Dictionary<string, IAppComponent> _components = new Dictionary<string, IAppComponent>();

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <param name="component"></param>
        public void Add(IAppComponent component)
        {
            Contract.Requires(component != null);

            lock (_components)
            {
                _Add(component);
            }
        }

        private void _Add(IAppComponent component)
        {
            string name = component.GetInfo().Name;
            if (_components.ContainsKey(name))
                throw new ServiceException(ServiceStatusCode.ServerError, string.Format("组件“{0}”被重复添加进管理器", name));

            _components.Add(component.GetInfo().Name, component);
        }

        /// <summary>
        /// 批量添加组件
        /// </summary>
        /// <param name="components"></param>
        public void AddRange(IEnumerable<IAppComponent> components)
        {
            Contract.Requires(components != null);

            foreach (IAppComponent component in components)
            {
                _Add(component);
            }
        }

        /// <summary>
        /// 删除一个组件
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool Remove(IAppComponent component)
        {
            Contract.Requires(component != null);

            return Remove(component.GetInfo().Name);
        }

        /// <summary>
        /// 删除一个组件
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public bool Remove(string componentName)
        {
            Contract.Requires(componentName != null);

            lock (_components)
            {
                return _components.Remove(componentName);
            }
        }

        /// <summary>
        /// 获取全部组件
        /// </summary>
        /// <returns></returns>
        public IAppComponent[] GetAll()
        {
            lock (_components)
            {
                return _components.Values.ToArray();
            }
        }

        /// <summary>
        /// 获取指定名称的组件
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public IAppComponent Get(string componentName)
        {
            Contract.Requires(componentName!=null);

            lock (_components)
            {
                return _components.GetOrDefault(componentName);
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            lock (_components)
            {
                foreach (IAppComponent component in _components.Values)
                {
                    try
                    {
                        if (!_IsThis(component))
                            component.Running = true;
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }
                }
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            lock (_components)
            {
                foreach (IAppComponent component in _components.Values)
                {
                    try
                    {
                        if (!_IsThis(component))
                            component.Running = false;
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }
                }
            }
        }

        private bool _IsThis(IAppComponent component)
        {
            return object.ReferenceEquals(component, this);
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            lock (_components)
            {
                foreach (IAppComponent component in _components.Values)
                {
                    try
                    {
                        if (!_IsThis(component))
                            component.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }
                }
            }
        }
    }
}
