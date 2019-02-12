using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common.Framework.TrayPlatform;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Utility;
using System.Xml.Serialization;
using System.IO;
using Common.Communication.Wcf;
using ServiceFairy.Entities;


namespace ServiceFairy.Service.Master.Components.Deploy
{
    /// <summary>
    /// 与部署地图相关的操作
    /// </summary>
    static class DeployUtility
    {
        /// <summary>
        /// 从配置文件中加载部署地图
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static AppClientDeployMap LoadDeployMapFromConfig(string sectionName = "systemSettings")
        {
            DeployMapConfiguration cfg = (DeployMapConfiguration)System.Configuration.ConfigurationManager.GetSection(sectionName);
            if (cfg == null)
                throw new ServiceException(ServiceStatusCode.ServerError, "部署地图加载失败");

            AppClientDeployMap deployMap = new AppClientDeployMap();
            foreach (DeployMapConfiguration.ClientConfigurationElement client in cfg.Clients)
            {
                deployMap.AddDeployInfo(new AppClientDeployInfo() {
                    ClientId = Guid.Parse(client.ClientID),
                    Services = client.Services.Cast<DeployMapConfiguration.ServiceConfigurationElement>().ToArray(v => new AppServiceDeployInfo(new ServiceDesc(v.Name, v.Version))),
                    CommunicateOptions = client.Communicates.Cast<DeployMapConfiguration.CommunicateConfigurationElement>().Select(v =>
                        new CommunicationOption(new ServiceAddress(v.Address, int.Parse(v.Port)), (CommunicationType)Enum.Parse(typeof(CommunicationType), v.Type, true))
                    ).ToArray()
                });
            }

            return deployMap;
        }

        public static AppClientDeployMap LoadDeployMapFromXmlFile(string file)
        {
            string xml;
            if (!File.Exists(file) || string.IsNullOrWhiteSpace(xml = File.ReadAllText(file)))
                return null;

            return DeployMapXmlSerializer.Deserialize(xml);
        }

        /// <summary>
        /// 保存部署地图到XML文件中
        /// </summary>
        /// <param name="file"></param>
        /// <param name="deployMap"></param>
        public static void SaveDeployMapToXmlFile(string file, AppClientDeployMap deployMap)
        {
            string s = DeployMapXmlSerializer.Serialize(deployMap);

            PathUtility.CreateDirectoryForFile(file);
            if (File.Exists(file))
            {
                DateTime now = DateTime.Now;
                string bkFile = PathUtility.AddPrefix(file, "DeployMaps\\" + now.ToString(@"yyyyMMdd\\HH-"));
                PathUtility.CreateDirectoryForFile(bkFile);
                if (!PathUtility.IsReadOnly(file))
                    File.Copy(file, bkFile, true);
            }

            File.WriteAllText(file, s, Encoding.UTF8);
        }

        class AppClientDeployInfoCollection : List<AppClientDeployInfo>
        {
            public int Index { get; set; }
        }

        /// <summary>
        /// 使负载均衡
        /// </summary>
        /// <param name="deployMap"></param>
        public static void ReviseForBalance(Service service, AppClientDeployMap deployMap)
        {
            Contract.Requires(deployMap != null);

            AppClientDeployInfo[] infos = deployMap.GetAll();
            
            // 建立一个服务与终端的对应关系表
            Dictionary<ServiceDesc, AppClientDeployInfoCollection> dict = new Dictionary<ServiceDesc, AppClientDeployInfoCollection>();
            foreach (AppClientDeployInfo dInfo in infos.Where(info => service.AppClientManager.IsAvaliable(info.ClientId)))
            {
                foreach (ServiceDesc serviceDesc in dInfo.Services.ToArray(v => v.ServiceDesc))
                {
                    AppClientDeployInfoCollection list;
                    if (!dict.TryGetValue(serviceDesc, out list))
                        dict.Add(serviceDesc, list = new AppClientDeployInfoCollection());

                    list.Add(dInfo);
                }
            }

            // 查看还缺哪些服务
            foreach (AppClientDeployInfo dInfo in infos.Where(info => service.AppClientManager.IsAvaliable(info.ClientId)))
            {
                InvokeListBuilder lb = new InvokeListBuilder();
                foreach (KeyValuePair<ServiceDesc, AppClientDeployInfoCollection> item in dict)
                {
                    ServiceDesc serviceDesc = item.Key;
                    if (dInfo.Services.Any(srv => srv.ServiceDesc == serviceDesc))
                        continue;

                    AppClientDeployInfoCollection c = item.Value;
                    AppClientDeployInfo d = c[c.Index++ % c.Count];
                    lb.Add(d, d.ClientId, serviceDesc);
                }

                dInfo.InvokeInfos = lb.GetInvokeList();
                dInfo.UpdateVersion();
            }
        }

        #region Class InvokeListBuilder ...

        class InvokeListBuilder
        {
            class Wrapper
            {
                public CommunicationOption[] Communications;
                public List<ServiceDesc> ServiceDescs;
            }

            private readonly Dictionary<Guid, Wrapper> _dict = new Dictionary<Guid, Wrapper>();

            public void Add(AppClientDeployInfo dInfo, Guid clientId, ServiceDesc serviceDesc)
            {
                Wrapper w;
                if (!_dict.TryGetValue(clientId, out w))
                    _dict.Add(clientId, w = new Wrapper() { Communications = dInfo.CommunicateOptions, ServiceDescs = new List<ServiceDesc>() });

                w.ServiceDescs.Add(serviceDesc);
            }

            public AppInvokeInfo[] GetInvokeList()
            {
                return _dict.Select(item => new AppInvokeInfo() {
                    CommunicateOptions = item.Value.Communications,
                    ClientID = item.Key, ServiceDescs = item.Value.ServiceDescs.ToArray()
                }).ToArray();
            }
        }

        #endregion

    }
}
