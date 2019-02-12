using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace ServiceFairy
{
    public static class SFNames
    {
        #region Class ServiceNames ...

        /// <summary>
        /// 服务名称
        /// </summary>
        public static class ServiceNames
        {
            public const string Tray = "Core.Tray";

            public const string Master = "Core.Master";

            public const string Station = "Core.Station";

            public const string Configuration = "Core.Configuration";

            public const string Deploy = "Core.Deploy";

            public const string Security = "Core.Security";

            public const string Log = "Sys.Log";

            public const string Proxy = "Sys.Proxy";

            public const string Navigation = "Sys.Navigation";

            public const string Watch = "Sys.Watch";

            public const string Cache = "Sys.Cache";

            public const string Share = "Sys.Share";

            public const string Database = "Sys.Database";

            public const string DatabaseCenter = "Sys.DatabaseCenter";

            public const string User = "Sys.User";

            public const string UserCenter = "Sys.UserCenter";

            public const string Queue = "Sys.Queue";

            public const string Sms = "Sys.Sms";

            public const string File = "Sys.File";

            public const string FileCenter = "Sys.FileCenter";

            public const string Message = "Sys.Message";

            public const string MessageCenter = "Sys.MessageCenter";

            public const string Group = "Sys.Group";

            public const string Test = "Sys.Test";

            public const string Email = "Sys.Email";

            public const string Session = "Sys.Session";

            public const string RemoteProxy = "Sys.RemoteProxy";

            public const string Statistics = "Sys.Statistics";

            /// <summary>
            /// 判断是否为核心服务
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public static bool IsCoreService(string name)
            {
                return name == Tray || name == Master || name == Station || name == Configuration || name == Deploy || name == Security;
            }

            public static bool IsCoreService(ServiceDesc sd)
            {
                Contract.Requires(sd != null);
                return IsCoreService(sd.Name);
            }
        }

        #endregion

        #region 参数的键名 ...

        public static class DataKeys
        {
            /// <summary>
            /// master服务器的通信方式
            /// </summary>
            public const string MASTER_COMMUNICATION = "MASTER_COMMUNICATION";

            /// <summary>
            /// 运行基路径
            /// </summary>
            public const string RUNNING_BASE_PATH = "RUNNING_BASE_PATH";

            /// <summary>
            /// 服务基路径
            /// </summary>
            public const string SERVICE_BASE_PATH = "SERVICE_BASE_PATH";

            /// <summary>
            /// 安装包路径
            /// </summary>
            public const string DEPLOY_PACKAGE_PATH = "DEPLOY_PACKAGE_PATH";

            /// <summary>
            /// 数据的路径
            /// </summary>
            public const string DATA_PATH = "DATA_PATH";

            /// <summary>
            /// 日志的路径
            /// </summary>
            public const string LOG_PATH = "LOG_PATH";

            /// <summary>
            /// 安装路径
            /// </summary>
            public const string INSTALL_PATH = "INSTALL_PATH";

            /// <summary>
            /// 客户端标题
            /// </summary>
            public const string CLIENT_TITLE = "CLIENT_TITLE";

            /// <summary>
            /// 客户端描述
            /// </summary>
            public const string CLIENT_DESC = "CLIENT_DESC";
        }

        #endregion

        /// <summary>
        /// 是否为MasterService
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static bool IsMasterService(this ServiceDesc sd)
        {
            return sd != null && sd.Name == SFNames.ServiceNames.Master;
        }

        /// <summary>
        /// 是否为TrayService
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static bool IsTrayService(this ServiceDesc sd)
        {
            return sd != null && sd.Name == SFNames.ServiceNames.Tray;
        }
    }
}
