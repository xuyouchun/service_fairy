using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common;
using Common.Utility;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management
{
    partial class SfManagementContext
    {
        /// <summary>
        /// 服务的部署信息管理器
        /// </summary>
        public class ServiceDeployInfoManager : ManagementBase<ServiceDeployInfoManager.Wrapper>
        {
            public ServiceDeployInfoManager(SfManagementContext ctx)
                : base(ctx)
            {
                
            }

            public class Wrapper
            {
                public Dictionary<ServiceDesc, ServiceDeployInfo> Dict;
                public ServiceDeployInfo[] Infos;
            }

            protected override ServiceDeployInfoManager.Wrapper OnLoad()
            {
                ServiceDeployInfo[] infos = MgrCtx.Invoker.Master.GetAllServiceDeployInfos() ?? Array<ServiceDeployInfo>.Empty;
                return new Wrapper() {
                    Infos = infos,
                    Dict = infos.ToDictionary(item => item.ServiceDesc)
                };   
            }

            /// <summary>
            /// 获取服务终端的部署信息
            /// </summary>
            /// <param name="serviceDesc"></param>
            /// <returns></returns>
            public ServiceDeployInfo Get(ServiceDesc serviceDesc)
            {
                Contract.Requires(serviceDesc != null);
                return Value.Dict.GetOrDefault(serviceDesc);
            }

            /// <summary>
            /// 获取所有服务终端的部署信息
            /// </summary>
            /// <returns></returns>
            public ServiceDeployInfo[] GetAll()
            {
                return Value.Infos;
            }

            /// <summary>
            /// 是否包含服务终端的部署信息
            /// </summary>
            /// <param name="serviceDesc"></param>
            /// <returns></returns>
            public bool Exist(ServiceDesc serviceDesc)
            {
                return Get(serviceDesc) != null;
            }
        }
    }
}
