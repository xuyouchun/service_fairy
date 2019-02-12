using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.Entities;
using Common;
using Common.Communication.Wcf;

namespace ServiceFairy.Deploy
{
    /// <summary>
    /// 部署策略
    /// </summary>
    public interface IDeployStrategy : IDisposable
    {
        /// <summary>
        /// 实始化
        /// </summary>
        void Init();

        /// <summary>
        /// 获取部署地图
        /// </summary>
        /// <returns></returns>
        AppClientDeployMap GetDeployMap();

        /// <summary>
        /// 部署方式发生变化
        /// </summary>
        /// <param name="changedInfos"></param>
        void OnChanged(DeployChangedInfo[] changedInfos);

        /// <summary>
        /// 新服务终端
        /// </summary>
        /// <param name="deployInfos"></param>
        void OnNewClient(AppClientDeployInfo[] deployInfos);

        /// <summary>
        /// 修改部署方式
        /// </summary>
        /// <param name="adjustInfos"></param>
        bool Adjust(AppClientAdjustInfo[] adjustInfos);

        /// <summary>
        /// 获取部署信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        AppClientDeployInfo GetDeployInfo(Guid clientId);

        /// <summary>
        /// 获取所有部署信息
        /// </summary>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        AppClientDeployInfo[] GetAllDeployInfos(out DateTime lastUpdate);

        /// <summary>
        /// 获取读写锁
        /// </summary>
        object SyncLocker { get; }

        /// <summary>
        /// 设置有效状态
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="type"></param>
        /// <param name="avaliable"></param>
        void SetAvaliable(Guid[] clientIds, AppClientAvaliable type, bool avaliable);

        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="connected"></param>
        void SetConnected(Guid[] clientIds, bool connected);

        /// <summary>
        /// 最后更新时间
        /// </summary>
        /// <returns></returns>
        DateTime GetLastUpdate();

        /// <summary>
        /// 更新平台版本号
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="deployPackageId"></param>
        void UpdatePlatformDeployId(Guid[] clientIds, Guid deployPackageId);

        /// <summary>
        /// 最后更新时间变化
        /// </summary>
        event EventHandler Modified;
    }

    public class DeployChangedInfo
    {
        public DeployChangedInfo(Guid clientId,
            ServiceDesc[] servicesStarted, ServiceDesc[] servicesStopped,
            CommunicationOption[] communicationsOpened, CommunicationOption[] communicationsClosed
            )
        {
            ClientId = clientId;
            ServicesStarted = servicesStarted;
            ServicesStopped = servicesStopped;
            CommunicationsOpened = communicationsOpened;
            CommunicationsClosed = communicationsClosed;
        }

        public Guid ClientId { get; private set; }

        public ServiceDesc[] ServicesStarted { get; private set; }

        public ServiceDesc[] ServicesStopped { get; private set; }

        public CommunicationOption[] CommunicationsOpened { get; private set; }

        public CommunicationOption[] CommunicationsClosed { get; private set; }

        public bool HasChanged()
        {
            return !ServicesStarted.IsNullOrEmpty() || !ServicesStopped.IsNullOrEmpty() || !CommunicationsOpened.IsNullOrEmpty() || !CommunicationsClosed.IsNullOrEmpty();
        }
    }
}
