using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Collection;

namespace ServiceFairy.Management
{
    partial class SfManagementContext
    {
        public class AppFileManager : ManagementBase<AppFileManager.Wrapper>
        {
            private AppFileManager(SfManagementContext ctx, Guid clientId, ServiceDesc serviceDesc)
                : base(ctx)
            {
                _clientId = clientId;
                _serviceDesc = serviceDesc;
            }

            private readonly Guid _clientId;
            private readonly ServiceDesc _serviceDesc;

            public class Wrapper
            {
                public AppFileInfo[] Infos;
                public IgnoreCaseDictionary<AppFileInfo> Dict;
            }

            protected override AppFileManager.Wrapper OnLoad()
            {
                AppFileInfo[] infos = MgrCtx.Invoker.Sys.GetAppServiceFileInfos(_clientId, _serviceDesc);
                return new Wrapper() {
                    Infos = infos, Dict = infos.ToIgnoreCaseDictionary(info => info.FileName, true)
                };
            }

            /// <summary>
            /// 获取指定名称的组件信息
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public AppFileInfo Get(string fileName)
            {
                Contract.Requires(fileName != null);

                return Value.Dict.GetOrDefault(fileName);
            }

            /// <summary>
            /// 获取所有组件信息
            /// </summary>
            /// <returns></returns>
            public AppFileInfo[] GetAll()
            {
                return Value.Infos;
            }

            private static readonly Dictionary<Tuple<Guid, ServiceDesc>, AppFileManager> _dict
                = new Dictionary<Tuple<Guid, ServiceDesc>, AppFileManager>();

            /// <summary>
            /// 获取指定服务的组件管理器
            /// </summary>
            /// <param name="ctx"></param>
            /// <param name="clientId"></param>
            /// <param name="sd"></param>
            /// <returns></returns>
            public static AppFileManager Get(SfManagementContext ctx, Guid clientId, ServiceDesc sd)
            {
                Contract.Requires(sd != null);
                return _dict.GetOrSet(new Tuple<Guid, ServiceDesc>(clientId, sd), (key) => new AppFileManager(ctx, clientId, sd));
            }
        }
    }
}
