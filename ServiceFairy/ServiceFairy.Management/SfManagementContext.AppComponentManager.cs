using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace ServiceFairy.Management
{
	partial class SfManagementContext
	{
        public class AppComponentManager : ManagementBase<AppComponentManager.Wrapper>
        {
            private AppComponentManager(SfManagementContext ctx, Guid clientId, ServiceDesc serviceDesc)
                : base(ctx)
            {
                _clientId = clientId;
                _serviceDesc = serviceDesc;
            }

            private readonly Guid _clientId;
            private readonly ServiceDesc _serviceDesc;

            public class Wrapper
            {
                public AppComponentInfo[] Infos;
                public Dictionary<string, AppComponentInfo> Dict;
            }

            protected override AppComponentManager.Wrapper OnLoad()
            {
                AppComponentInfo[] infos = MgrCtx.Invoker.Sys.GetAppComponentInfos(_clientId, _serviceDesc);
                return new Wrapper() {
                    Infos = infos, Dict = infos.ToDictionary(info => info.Name)
                };   
            }

            /// <summary>
            /// 获取指定名称的组件信息
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public AppComponentInfo Get(string name)
            {
                Contract.Requires(name != null);

                return Value.Dict.GetOrDefault(name);
            }

            /// <summary>
            /// 获取所有组件信息
            /// </summary>
            /// <returns></returns>
            public AppComponentInfo[] GetAll()
            {
                return Value.Infos;
            }

            private static readonly Dictionary<Tuple<Guid, ServiceDesc>, AppComponentManager> _dict
                = new Dictionary<Tuple<Guid, ServiceDesc>, AppComponentManager>();

            /// <summary>
            /// 获取指定服务的组件管理器
            /// </summary>
            /// <param name="ctx"></param>
            /// <param name="clientId"></param>
            /// <param name="sd"></param>
            /// <returns></returns>
            public static AppComponentManager Get(SfManagementContext ctx, Guid clientId, ServiceDesc sd)
            {
                Contract.Requires(sd != null);
                return _dict.GetOrSet(new Tuple<Guid, ServiceDesc>(clientId, sd), (key) => new AppComponentManager(ctx, clientId, sd));
            }
        }
	}
}
