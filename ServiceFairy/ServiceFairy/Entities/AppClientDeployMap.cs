using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common;
using Common.Utility;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 部署表
    /// </summary>
    public class AppClientDeployMap
    {
        public AppClientDeployMap()
        {
            UpdateVersion();
        }

        private readonly Dictionary<Guid, AppClientDeployInfo> _deployClients = new Dictionary<Guid, AppClientDeployInfo>();

        /// <summary>
        /// 最近更新的时间
        /// </summary>
        public DateTime LastUpdate { get; private set; }

        /// <summary>
        /// 添加一个终端
        /// </summary>
        /// <param name="deployInfo"></param>
        public void AddDeployInfo(AppClientDeployInfo deployInfo)
        {
            Contract.Requires(deployInfo != null && deployInfo.ClientId != null);
            _deployClients[deployInfo.ClientId] = deployInfo;

            UpdateVersion();
        }

        /// <summary>
        /// 批量添加终端
        /// </summary>
        /// <param name="deployInfos"></param>
        /// <param name="adjust"></param>
        public void AddDeployInfos(AppClientDeployInfo[] deployInfos, bool adjust)
        {
            Contract.Requires(deployInfos != null);

            bool modified = false;
            foreach (AppClientDeployInfo dInfo in deployInfos)
            {
                AppClientDeployInfo oldInfo = _deployClients.GetOrDefault(dInfo.ClientId);
                if (oldInfo == null || !adjust)
                    _deployClients[dInfo.ClientId] = dInfo;
                else if (oldInfo.Combine(dInfo, true))
                    modified = true;
            }

            if (modified)
                UpdateVersion();
        }

        public void UpdateVersion()
        {
            LastUpdate = DateTime.UtcNow;

            _RaiseLastUpdateChangedEvent();
        }
        
        /// <summary>
        /// 删除一个终端
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool RemoveDeployInfo(Guid clientId)
        {
            bool r = _deployClients.Remove(clientId);
            UpdateVersion();

            return r;
        }

        /// <summary>
        /// 获取一个终端
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public AppClientDeployInfo GetDeployInfo(Guid clientId)
        {
            Contract.Requires(clientId != null);

            return _deployClients.GetOrDefault(clientId);
        }

        /// <summary>
        /// 终端数量
        /// </summary>
        public int Count
        {
            get { return _deployClients.Count; }
        }

        /// <summary>
        /// 获取全部终端
        /// </summary>
        /// <returns></returns>
        public AppClientDeployInfo[] GetAll()
        {
            return _deployClients.Values.ToArray();
        }

        /// <summary>
        /// 最后更新时间变化通知
        /// </summary>
        public event EventHandler LastUpdateChanged;

        private void _RaiseLastUpdateChangedEvent()
        {
            var eh = LastUpdateChanged;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }
    }
}
