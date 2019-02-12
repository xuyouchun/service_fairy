using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Master;
using Common.Contracts.Service;
using Common.Package;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common;
using ServiceFairy.Entities;

namespace ServiceFairy.Management
{
    partial class SfManagementContext
    {
        /// <summary>
        /// 服务安装包信息　
        /// </summary>
        public class ServiceDeployPackageInfoManager : ManagementBase<SfManagementContext.ServiceDeployPackageInfoManager.Wrapper>
        {
            public ServiceDeployPackageInfoManager(SfManagementContext ctx)
                : base(ctx)
            {
                
            }

            public class Wrapper
            {
                public ServiceDeployPackageInfo[] Infos;
                public Dictionary<Guid, ServiceDeployPackageInfo> Dict;
                public Dictionary<ServiceDesc, ServiceDeployPackageInfo[]> ServiceDescDict;
                public Dictionary<Guid, ServiceDeployPackageInfo> CurrentDict;
            }

            protected override Wrapper OnLoad()
            {
                ServiceDeployPackageInfo[] infos = MgrCtx.Invoker.Master.GetAllServiceDeployPackageInfos(false);
                return new Wrapper {
                    Infos = infos,
                    Dict = infos.ToDictionary(info => info.Id),
                    ServiceDescDict = infos.GroupBy(info => info.ServiceDesc).ToDictionary(g => g.Key, g => g.ToArray()),
                    CurrentDict = MgrCtx.Invoker.Master.GetAllServiceDeployPackageInfos(true).ToDictionary(info => info.Id),
                };   
            }

            /// <summary>
            /// 获取全部安装包信息
            /// </summary>
            /// <returns></returns>
            public ServiceDeployPackageInfo[] GetAll()
            {
                return Value.Infos;
            }

            /// <summary>
            /// 获取指定ID的安装包信息
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public ServiceDeployPackageInfo Get(Guid id)
            {
                return Value.Dict.GetOrDefault(id);
            }

            /// <summary>
            /// 获取指定服务的安装包信息
            /// </summary>
            /// <param name="serviceDesc"></param>
            /// <returns></returns>
            public ServiceDeployPackageInfo[] Get(ServiceDesc serviceDesc)
            {
                Contract.Requires(serviceDesc != null);
                return Value.ServiceDescDict.GetOrDefault(serviceDesc) ?? Array<ServiceDeployPackageInfo>.Empty;
            }

            /// <summary>
            /// 是否包含指定ID的安装包
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool Exists(Guid id)
            {
                return Get(id) != null;
            }

            /// <summary>
            /// 获取指定平台版本的名称
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public string GetTitle(Guid id)
            {
                ServiceDeployPackageInfo info = Get(id);
                return info != null ? info.Title : "";
            }

            /// <summary>
            /// 是否为当前版本
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool IsCurrentVersion(Guid id)
            {
                return Value.CurrentDict.ContainsKey(id);
            }
        }
    }
}
