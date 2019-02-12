using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Contracts.Service;

namespace ServiceFairy.Install
{
    /// <summary>
    /// Service的运行时配置
    /// </summary>
    [DataContract]
    public class RunningConfiguration
    {
        /// <summary>
        /// 服务的基目录
        /// </summary>
        [DataMember(Name = "serviceBasePath")]
        public string ServiceBasePath { get; set; }

        /// <summary>
        /// 需要启动的服务
        /// </summary>
        [DataMember(Name = "servicesToStart")]
        public string ServicesToStart { get; set; }

        /// <summary>
        /// 要开启的端口
        /// </summary>
        [DataMember(Name = "communicationsToOpen")]
        public string CommunicationsToOpen { get; set; }

        /// <summary>
        /// 中心服务器地址
        /// </summary>
        [DataMember(Name = "masterCommunications")]
        public string MasterCommunications { get; set; }

        /// <summary>
        /// 运行模式
        /// </summary>
        public RunningModel RunningModel
        {
            get
            {
                RunningModel m;
                Enum.TryParse<RunningModel>(_runningModel, out m);
                return m;
            }
            set { _runningModel = value.ToString(); }
        }

        [DataMember(Name = "runningModel")]
        private string _runningModel;

        /// <summary>
        /// 终端ID
        /// </summary>
        [DataMember(Name = "clientId")]
        public Guid ClientID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember(Name = "clientTitle")]
        public string ClientTitle { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember(Name = "clientDesc")]
        public string ClientDesc { get; set; }

        /// <summary>
        /// 保存到文件中
        /// </summary>
        /// <param name="file"></param>
        public void SaveToFile(string file)
        {
            Contract.Requires(file != null);

            InstallUtility.SaveToFile(file, this);
        }

        /// <summary>
        /// 从文件加载
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static RunningConfiguration LoadFromFile(string file)
        {
            Contract.Requires(file != null);

            return InstallUtility.ReadFromFile<RunningConfiguration>(file);
        }

        /// <summary>
        /// 从文件中加载，如果出错则返回默认值
        /// </summary>
        /// <param name="file"></param>
        /// <param name="defaultCfg"></param>
        /// <returns></returns>
        public static RunningConfiguration LoadFromFileOrDefault(string file, RunningConfiguration defaultCfg = null)
        {
            if (file != null)
            {
                try
                {
                    return LoadFromFile(file);
                }
                catch { }
            }

            RunningConfiguration cfg = defaultCfg ?? CreateDefault();
            cfg.SaveToFile(file);
            return cfg;
        }

        /// <summary>
        /// 创建默认的配置
        /// </summary>
        /// <returns></returns>
        public static RunningConfiguration CreateDefault()
        {
            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ServiceFairy");

            return new RunningConfiguration() {
                ServiceBasePath = basePath,
                ClientID = Guid.NewGuid(),
                ServicesToStart = "",
                CommunicationsToOpen = "",
                RunningModel = RunningModel.Normal,
                MasterCommunications = "localhost:8090 Tcp",
            };
        }
    }
}
