using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace ServiceFairy.Service.UI
{
    [SoInfo("接口", "网络通信接口"), UIObjectImage("CommandList")]
    class AppCommandKernelNode : ServiceObjectKernelTreeNodeBase
    {
        public AppCommandKernelNode(AppServiceInfo appServiceInfo, AppCommandInfo appCommandInfo)
        {
            Contract.Requires(appServiceInfo != null && appCommandInfo != null);

            AppServiceInfo = appServiceInfo;
            AppCommandInfo = appCommandInfo;
        }

        /// <summary>
        /// 服务信息
        /// </summary>
        public AppServiceInfo AppServiceInfo { get; private set; }

        /// <summary>
        /// 指令信息
        /// </summary>
        public AppCommandInfo AppCommandInfo { get; private set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [SoInfo("版本"), ServiceObjectProperty]
        public string Version
        {
            get { return AppCommandInfo.CommandDesc.Version.ToString(); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return AppCommandInfo.Desc; }
        }

        /// <summary>
        /// 输入参数
        /// </summary>
        [SoInfo("输入"), ServiceObjectProperty]
        public string InputParameter
        {
            get { return AppCommandInfo.InputParameter.ToStringIgnoreNull("(空)"); }
        }

        /// <summary>
        /// 输入参数
        /// </summary>
        [SoInfo("输出"), ServiceObjectProperty]
        public string OutputParameter
        {
            get { return AppCommandInfo.OutputParameter.ToStringIgnoreNull("(空)"); }
        }

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc, CommandDesc>(GetType(), AppServiceInfo.ServiceDesc, AppCommandInfo.CommandDesc);
        }
    }
}
