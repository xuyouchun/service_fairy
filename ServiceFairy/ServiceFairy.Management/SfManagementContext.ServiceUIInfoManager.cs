using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Master;
using Common.Contracts.Service;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Management
{
	partial class SfManagementContext
	{
        public class ServiceUIInfoManager : ManagementBase<ServiceUIInfoManager.Wrapper>
        {
            public ServiceUIInfoManager(SfManagementContext ctx)
                : base(ctx, TimeSpan.FromMinutes(30))
            {
                
            }

            public class Wrapper
            {
                public ServiceUIInfo[] ServiceUIInfos { get; set; }
                public Dictionary<ServiceDesc, ServiceUIInfo> Dict { get; set; }
            }

            private readonly Dictionary<ServiceDesc, ServiceUIInfo> _uiInfos = new Dictionary<ServiceDesc, ServiceUIInfo>();

            protected override ServiceUIInfoManager.Wrapper OnLoad()
            {
                ServiceDesc[] sds = MgrCtx.Invoker.Master.GetAllServices();
                _uiInfos.RemoveWhere(item => !sds.Contains(item.Key));
                ServiceDesc[] notExists = sds.Except(_uiInfos.Keys).ToArray();

                if (!notExists.IsNullOrEmpty())
                {
                    foreach (IEnumerable<ServiceDesc> group in sds.Split(5))
                    {
                        ServiceUIInfo[] infos = MgrCtx.Invoker.Master.DownloadServiceUIInfo(group.ToArray(), includeAssembly: false, includeResource: true);
                        _uiInfos.AddRange(infos.ToDictionary(info => info.ServiceDesc), ignoreDupKey: true);
                    }
                }

                return new Wrapper {
                    ServiceUIInfos = _uiInfos.Values.ToArray(),
                    Dict = _uiInfos.Values.ToDictionary(ui => ui.ServiceDesc, true),
                };
            }

            /// <summary>
            /// 获取全部
            /// </summary>
            /// <returns></returns>
            public ServiceUIInfo[] GetAll()
            {
                return Value.ServiceUIInfos;
            }

            /// <summary>
            /// 获取指定服务的UI信息
            /// </summary>
            /// <param name="serviceDesc"></param>
            /// <returns></returns>
            public ServiceUIInfo Get(ServiceDesc serviceDesc)
            {
                Contract.Requires(serviceDesc != null);

                return Value.Dict.GetOrDefault(serviceDesc);
            }

            /// <summary>
            /// 获取指定服务的标题
            /// </summary>
            /// <param name="serviceDesc"></param>
            /// <returns></returns>
            public string GetTitle(ServiceDesc serviceDesc)
            {
                Contract.Requires(serviceDesc != null);

                ServiceUIInfo uiInfo = Get(serviceDesc);
                return uiInfo == null ? null : uiInfo.Title;
            }
        }
	}
}
