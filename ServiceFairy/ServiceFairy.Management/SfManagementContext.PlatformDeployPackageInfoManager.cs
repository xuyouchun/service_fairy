using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Master;
using Common.Package;
using Common.Utility;
using ServiceFairy.Entities;

namespace ServiceFairy.Management
{
    partial class SfManagementContext
    {
        public class PlatformDeployPackageInfoManager : ManagementBase<PlatformDeployPackageInfoManager.Wrapper>
        {
            public PlatformDeployPackageInfoManager(SfManagementContext ctx)
                : base(ctx)
            {
                
            }

            public class Wrapper
            {
                public PlatformDeployPackageInfo[] Infos;
                public Dictionary<Guid, PlatformDeployPackageInfo> Dict;
            }

            protected override PlatformDeployPackageInfoManager.Wrapper OnLoad()
            {
                PlatformDeployPackageInfo[] infos = MgrCtx.Invoker.Master.GetAllPlatformDeployPackageInfos();
                return new Wrapper {
                    Infos = infos,
                    Dict = infos.ToDictionary(info => info.Id),
                };   
            }

            /// <summary>
            /// 获取全部安装包信息
            /// </summary>
            /// <returns></returns>
            public PlatformDeployPackageInfo[] GetAll()
            {
                return Value.Infos;
            }

            /// <summary>
            /// 获取指定ID的安装包信息
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public PlatformDeployPackageInfo Get(Guid id)
            {
                return Value.Dict.GetOrDefault(id);
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
                PlatformDeployPackageInfo info = Get(id);
                return info != null ? info.Title : "";
            }
        }
    }
}
