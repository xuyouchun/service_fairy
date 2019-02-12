using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.UI
{
    /// <summary>
    /// 指令管理器
    /// </summary>
    class AppCommandInfoManager
    {
        public AppCommandInfoManager()
        {

        }

        public AppCommandInfoList GetAppCommandInfos(ServiceDesc serviceDesc)
        {
            return null;
        }
    }

    /// <summary>
    /// 指令列表
    /// </summary>
    class AppCommandInfoList
    {
        public AppCommandInfoList(AppCommandInfo[] appCommandInfos)
        {
            Contract.Requires(appCommandInfos != null);

            AppCommandInfos = appCommandInfos;
        }

        /// <summary>
        /// 所有指令的描述
        /// </summary>
        public AppCommandInfo[] AppCommandInfos { get; private set; }
    }
}
