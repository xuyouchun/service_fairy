using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Sys;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package.Service;
using Common;

namespace ServiceFairy.Management
{
    partial class SfManagementContext
    {
        /// <summary>
        /// 服务插件
        /// </summary>
        public class AppServiceAddinManager : ManagementBase<AppServiceAddinManager.Wrapper>
        {
            public AppServiceAddinManager(SfManagementContext ctx, Guid clientId, ServiceDesc servieDesc)
                : base(ctx)
            {
                _clientId = clientId;
                _serviceDesc = servieDesc;
                _addinSupportted = new Lazy<bool>(_LoadAddinSupportted);
            }

            private readonly Guid _clientId;
            private readonly ServiceDesc _serviceDesc;
            private readonly Lazy<bool> _addinSupportted;

            public class Wrapper
            {
                public AppServiceAddinInfoItem[] Infos;
                public Dictionary<AddinDesc, AppServiceAddinInfoItem[]> Dict;
            }

            private bool _LoadAddinSupportted()
            {
                return MgrCtx.Invoker.Sys.ExistsCommand(_serviceDesc + "/Sys_AddinCall");
            }

            protected override Wrapper OnLoad()
            {
                AppServiceAddinInfoItem[] infos = _addinSupportted.Value ?
                    MgrCtx.Invoker.Sys.GetAddinInfos(_clientId, _serviceDesc) : Array<AppServiceAddinInfoItem>.Empty;

                return new Wrapper() {
                    Infos = infos, Dict = infos.GroupBy(info => info.Info.AddinDesc).ToDictionary(g => g.Key, g => g.ToArray())
                };
            }

            /// <summary>
            /// 获取指定插件的信息
            /// </summary>
            /// <param name="addinDesc"></param>
            /// <returns></returns>
            public AppServiceAddinInfoItem[] Get(AddinDesc addinDesc)
            {
                Contract.Requires(addinDesc != null);

                return Value.Dict.GetOrDefault(addinDesc);
            }

            /// <summary>
            /// 获取指定插件的信息
            /// </summary>
            /// <returns></returns>
            public AppServiceAddinInfoItem[] GetAll()
            {
                return Value.Infos;
            }

            private static readonly Dictionary<Tuple<Guid, ServiceDesc>, AppServiceAddinManager> _dict
                = new Dictionary<Tuple<Guid, ServiceDesc>, AppServiceAddinManager>();

            /// <summary>
            /// 获取指定服务的组件管理器
            /// </summary>
            /// <param name="ctx"></param>
            /// <param name="clientId"></param>
            /// <param name="sd"></param>
            /// <returns></returns>
            public static AppServiceAddinManager Get(SfManagementContext ctx, Guid clientId, ServiceDesc sd)
            {
                Contract.Requires(sd != null);
                return _dict.GetOrSet(new Tuple<Guid, ServiceDesc>(clientId, sd), (key) => new AppServiceAddinManager(ctx, clientId, sd));
            }
        }
    }
}
