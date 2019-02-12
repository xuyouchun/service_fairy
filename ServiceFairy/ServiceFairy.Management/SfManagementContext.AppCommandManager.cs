using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace ServiceFairy.Management
{
    public partial class SfManagementContext
    {
        public class AppCommandManager : ManagementBase<AppCommandManager.Wrapper>
        {
            private AppCommandManager(SfManagementContext ctx, Guid clientId, ServiceDesc serviceDesc)
                : base(ctx)
            {
                _clientId = clientId;
                _serviceDesc = serviceDesc;
            }

            private readonly Guid _clientId;
            private readonly ServiceDesc _serviceDesc;

            public class Wrapper
            {
                public AppCommandInfo[] Infos;
                public Dictionary<CommandDesc, AppCommandInfo> Dict;
            }

            protected override AppCommandManager.Wrapper OnLoad()
            {
                AppCommandInfo[] infos = MgrCtx.Invoker.Sys.GetAppCommandInfos(_clientId, _serviceDesc);
                return new Wrapper() {
                    Infos = infos, Dict = infos.ToDictionary(info => info.CommandDesc)
                };   
            }

            /// <summary>
            /// 获取指定名称的组件信息
            /// </summary>
            /// <param name="commandDesc"></param>
            /// <returns></returns>
            public AppCommandInfo Get(CommandDesc commandDesc)
            {
                Contract.Requires(commandDesc != null);

                return Value.Dict.GetOrDefault(commandDesc);
            }

            /// <summary>
            /// 获取所有组件信息
            /// </summary>
            /// <returns></returns>
            public AppCommandInfo[] GetAll()
            {
                return Value.Infos;
            }

            private static readonly Dictionary<Tuple<Guid, ServiceDesc>, AppCommandManager> _dict
                = new Dictionary<Tuple<Guid, ServiceDesc>, AppCommandManager>();

            /// <summary>
            /// 获取指定服务的组件管理器
            /// </summary>
            /// <param name="ctx"></param>
            /// <param name="clientId"></param>
            /// <param name="sd"></param>
            /// <returns></returns>
            public static AppCommandManager Get(SfManagementContext ctx, Guid clientId, ServiceDesc sd)
            {
                Contract.Requires(sd != null);
                return _dict.GetOrSet(new Tuple<Guid, ServiceDesc>(clientId, sd), (key) => new AppCommandManager(ctx, clientId, sd));
            }
        }
    }
}
