using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Configuration;
using System.ServiceProcess;
using System.Xml;
using System.IO;

namespace AssemblyHost.WindowsService
{
    static class Settings
    {
        static Settings()
        {
            _xDoc = new XmlDocument();

            try
            {
                string file = typeof(Settings).Assembly.Location;
                string xmlFile = file + ".install.xml";
                if (File.Exists(xmlFile))
                    _xDoc.Load(xmlFile);

                if (_xDoc.DocumentElement != null)
                {
                    foreach (XmlElement element in _xDoc.DocumentElement.ChildNodes.OfType<XmlElement>())
                    {
                        _configDict[element.Name] = element.InnerText;
                    }
                }
            }
            catch
            {

            }
        }

        private static readonly Dictionary<string, string> _configDict = new Dictionary<string, string>();

        private static readonly XmlDocument _xDoc;

        /// <summary>
        /// 服务名称
        /// </summary>
        public static string ServiceName
        {
            get { return _GetXmlConfig<string>("serviceName", "My Service"); }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public static string DisplayName
        {
            get { return _GetXmlConfig<string>("displayName", ""); }
        }

        /// <summary>
        /// 启动服务的帐号
        /// </summary>
        public static ServiceAccount ServiceAccount
        {
            get { return _GetXmlConfig<ServiceAccount>("serviceAccount", ServiceAccount.LocalService); }
        }

        /// <summary>
        /// 服务的描述
        /// </summary>
        public static string Description
        {
            get { return _GetXmlConfig<string>("description", ""); }
        }

        /// <summary>
        /// 延迟启动
        /// </summary>
        public static bool DelayAutoStart
        {
            get { return _GetXmlConfig<bool>("delayAutoStart", false); }
        }

        /// <summary>
        /// 服务启动方式
        /// </summary>
        public static ServiceStartMode ServiceStartMode
        {
            get { return _GetXmlConfig<ServiceStartMode>("serviceStartMode", ServiceStartMode.Manual); }
        }

        /// <summary>
        /// 依赖项
        /// </summary>
        public static string[] ServicesDependedOn
        {
            get { return _GetXmlConfig<string>("servicesDependedOn", "").Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries); }
        }

        /// <summary>
        /// 是否可以关闭
        /// </summary>
        public static bool CanShutDown
        {
            get { return _GetXmlConfig<bool>("canShutDown", false); }
        }

        /// <summary>
        /// 是否可以停止
        /// </summary>
        public static bool CanStop
        {
            get { return _GetXmlConfig<bool>("canStop", true); }
        }

        /// <summary>
        /// 是否可以暂停和恢复
        /// </summary>
        public static bool CanPauseAndContinue
        {
            get { return _GetXmlConfig<bool>("canPauseAndContinue", false); }
        }

        /// <summary>
        /// 是否可以响应电源事件
        /// </summary>
        public static bool CanHandlePowerEvent
        {
            get { return _GetXmlConfig<bool>("canHandlePowerEvent", false); }
        }

        /// <summary>
        /// 是否可以响应SessionChange事件
        /// </summary>
        public static bool CanHandleSessionChangeEvent
        {
            get { return _GetXmlConfig<bool>("canHandleSessionChangeEvent", false); }
        }

        /// <summary>
        /// 是否自动记录日志
        /// </summary>
        public static bool AutoLog
        {
            get { return _GetXmlConfig<bool>("autoLog", false); }
        }

        private static T _GetXmlConfig<T>(string name, T defaultValue)
        {
            Contract.Requires(name != null);

            string value;
            if (!_configDict.TryGetValue(name, out value) || value == null)
                return defaultValue;

            try
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), value, true);
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
