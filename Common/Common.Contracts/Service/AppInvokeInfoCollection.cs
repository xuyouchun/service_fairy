using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.Diagnostics.Contracts;
using Common.Collection;
using Common.Utility;
using Common;
using Common.Contracts.Service;

namespace Common.Contracts.Service
{
    /// <summary>
    /// ServerInfo的集合
    /// </summary>
    public class AppInvokeInfoCollection
    {
        public AppInvokeInfoCollection(AppInvokeInfo[] serverInfos)
        {
            foreach (AppInvokeInfo serverInfo in serverInfos ?? new AppInvokeInfo[0])
            {
                _serverDict[serverInfo.ClientID] = serverInfo;

                foreach (ServiceDesc serviceDesc in serverInfo.ServiceDescs ?? Array<ServiceDesc>.Empty)
                {
                    Dictionary<SVersion, Guid> versionDict;
                    if (!_serviceDict.TryGetValue(serviceDesc.Name, out versionDict))
                        _serviceDict.Add(serviceDesc.Name, versionDict = new Dictionary<SVersion, Guid>(1));

                    versionDict[serviceDesc.Version] = serverInfo.ClientID;
                }
            }
        }

        private readonly Dictionary<Guid, AppInvokeInfo> _serverDict = new Dictionary<Guid, AppInvokeInfo>();
        private readonly Dictionary<string, Dictionary<SVersion, Guid>> _serviceDict = new Dictionary<string, Dictionary<SVersion, Guid>>();

        /// <summary>
        /// 通过服务名称找到服务器信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public AppInvokeInfo GetServerInfoByServiceName(string serviceName, SVersion version)
        {
            Contract.Requires(serviceName != null);

            Dictionary<SVersion, Guid> versionDict;
            if (!_serviceDict.TryGetValue(serviceName, out versionDict))
                return null;

            Guid serverId;
            if (!version.IsEmpty)
            {
                if (!versionDict.TryGetValue(version, out serverId))
                    return null;
            }
            else
            {
                if (versionDict.Count == 1)
                    serverId = versionDict.First().Value;
                else
                    serverId = versionDict[versionDict.Keys.Max()];
            }

            return _serverDict[serverId];
        }

        /// <summary>
        /// 通过服务名称获取各个版本的服务器信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public IDictionary<SVersion, AppInvokeInfo> GetServerInfosByServiceName(string serviceName)
        {
            Contract.Requires(serviceName != null);

            Dictionary<SVersion, Guid> versionDict;
            if (!_serviceDict.TryGetValue(serviceName, out versionDict))
                return null;

            return versionDict.ToDictionary(v => v.Key, v => _serverDict[v.Value]);
        }
    }
}
