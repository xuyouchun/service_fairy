using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.SystemInvoke;
using Common.Data.UnionTable;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 数据库连接管理器
    /// </summary>
    [AppComponent("数据库连接管理器", "维护数据库连接", AppComponentCategory.System, "Sys_DbConnectionManager")]
    public class DbConnectionManagerAppComponent : AppComponent
    {
        public DbConnectionManagerAppComponent(TrayAppServiceBase service, UtTableGroupReviseInfo[] reviseInfos = null)
            : base(service)
        {
            _utConProvider = new RemoteUtConnectionProviderEx(SystemInvoker.FromServiceClient(service.Context.ServiceClient), reviseInfos);
        }

        public DbConnectionManagerAppComponent(TrayAppServiceBase service, UtTableGroupReviseInfo reviseInfo)
            : this(service, reviseInfo == null ? null : new[] { reviseInfo })
        {

        }

        private readonly IUtConnectionProvider _utConProvider;

        /// <summary>
        /// 默认数据库连接
        /// </summary>
        public IUtConnectionProvider Provider
        {
            get { return _utConProvider; }
        }

        class RemoteUtConnectionProviderEx : RemoteUtConnectionProvider
        {
            public RemoteUtConnectionProviderEx(SystemInvoker invoker, UtTableGroupReviseInfo[] reviseInfos)
                : base(invoker, reviseInfos)
            {

            }

            protected override void OnInitMetaData(UtTableGroupReviseInfo[] reviseInfos)
            {
                base.OnInitMetaData(reviseInfos);
            }
        }
    }
}
