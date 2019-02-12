using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Package.Service
{
    /// <summary>
    /// AppService插件的基类
    /// </summary>
    public abstract class AppServiceAddinBase : MarshalByRefObjectEx, IAppServiceAddin
    {
        public AppServiceAddinBase()
        {
            Communicate = new AppServiceAddinCommunicate(this);
        }

        /// <summary>
        /// 获取插件的信息
        /// </summary>
        /// <returns></returns>
        public AppServiceAddinInfo GetInfo()
        {
            return _addinInfo ?? (_addinInfo = OnGetInfo());
        }

        private volatile AppServiceAddinInfo _addinInfo;

        protected virtual AppServiceAddinInfo OnGetInfo()
        {
            AppServiceAddinAttribute attr = this.GetType().GetAttribute<AppServiceAddinAttribute>();
            if (attr == null)
                throw new InvalidOperationException("无法获取插件的信息，可以通过重载OnGetInfo方法或用AppServiceAddinAttribute来指定插件的信息");

            return attr.ToAddinInfo();
        }

        /// <summary>
        /// 调用指定的接口
        /// </summary>
        /// <param name="context">执行环境</param>
        /// <param name="method">接口名称</param>
        /// <param name="data">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns>应答结果</returns>
        protected abstract CommunicateData OnCall(CommunicateContext context, string method, CommunicateData data, CallingSettings settings);

        /// <summary>
        /// 通信策略
        /// </summary>
        public ICommunicate Communicate { get; private set; }

        #region Class AppServiceAddinCommunicate ...

        class AppServiceAddinCommunicate : MarshalByRefObjectEx, ICommunicate
        {
            public AppServiceAddinCommunicate(AppServiceAddinBase owner)
            {
                _owner = owner;
            }

            private readonly AppServiceAddinBase _owner;

            public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
            {
                return _owner.OnCall(context, method, data, settings);
            }

            public void Dispose()
            {
                
            }
        }

        #endregion

        public virtual void Dispose()
        {
            Communicate.Dispose();
        }
    }
}
