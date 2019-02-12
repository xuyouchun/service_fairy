using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package;
using Common.Utility;
using Common;
using System.Threading;

namespace Common.Package.Service
{
    /// <summary>
    /// 支持生存期管理的组件
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public abstract class AppComponentBase : AppComponent, IAppComponent
    {
        public AppComponentBase(object owner)
            : base(owner)
        {
            
        }

        private volatile IAppComponentControllerContext _controllerContext;
        private volatile IAppComponentController[] _controllers;

        /// <summary>
        /// 创建服务提供者
        /// </summary>
        /// <returns></returns>
        protected abstract IServiceProvider CreateServiceProvider();

        /// <summary>
        /// 创建控制器
        /// </summary>
        /// <returns></returns>
        protected abstract IAppComponentController[] CreateControllers();

        /// <summary>
        /// 创建控制器的上下文环境
        /// </summary>
        /// <returns></returns>
        protected abstract IAppComponentControllerContext CreateControllerContext();

        private IAppComponentController[] _CreateControllers()
        {
            if (_controllers != null)
                return _controllers;

            IAppComponentController[] controllers = CreateControllers();
            IAppComponentControllerContext controllerContext = _controllerContext ?? (_controllerContext = CreateControllerContext());
            foreach (IAppComponentController controller in controllers)
            {
                controller.Init(_controllerContext);
            }

            return _controllers = controllers;
        }

        protected override void OnStart()
        {
            base.OnStart();

            foreach (IAppComponentController controller in _CreateControllers())
            {
                controller.Apply();
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            foreach (IAppComponentController controller in _CreateControllers())
            {
                controller.Abolish();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void OnDispose()
        {
            if (_controllers != null)
            {
                foreach (IAppComponentController controller in _controllers)
                {
                    controller.Dispose();
                }

                _controllers = null;
            }
        }
    }
}
