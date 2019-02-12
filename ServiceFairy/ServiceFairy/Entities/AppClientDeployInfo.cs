using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Communication.Wcf;
using System.Diagnostics;
using ServiceFairy.Entities;
using Common.Utility;
using System.Xml.Serialization;
using Common.Contracts;
using Common;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 表明一个需要部署的终端
    /// </summary>
    [Serializable, DataContract, DebuggerDisplay("ServiceCount = {ServiceDescs.Length} CommuicationOptionCount={CommunicateOptions.Length} LastUpdate={LastUpdate}")]
    public class AppClientDeployInfo
    {
        public AppClientDeployInfo()
        {
            Avaliable = AppClientAvaliable.Avaliable;
            UpdateVersion();
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        [DataMember]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 需要运行的服务
        /// </summary>
        [DataMember]
        public AppServiceDeployInfo[] Services { get; set; }

        /// <summary>
        /// 需要开启的端口
        /// </summary>
        [DataMember]
        public CommunicationOption[] CommunicateOptions { get; set; }

        /// <summary>
        /// 服务调用信息
        /// </summary>
        [DataMember]
        public AppInvokeInfo[] InvokeInfos { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// 平台安装包ID
        /// </summary>
        [DataMember]
        public Guid PlatformDeployPackageId { get; set; }

        /// <summary>
        /// 是否处于连接状态
        /// </summary>
        [DataMember]
        public bool Connected { get; set; }

        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="connected"></param>
        /// <param name="updateVersion"></param>
        public bool SetConnected(bool connected, bool updateVersion = false)
        {
            if (connected != Connected)
            {
                Connected = connected;
                if (updateVersion)
                    UpdateVersion();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否处于有效状态（监控服务设置的状态）
        /// </summary>
        [DataMember]
        public AppClientAvaliable Avaliable { get; set; }

        /// <summary>
        /// 设置有效状态
        /// </summary>
        /// <param name="type"></param>
        /// <param name="avaliable"></param>
        /// <param name="updateVersion"></param>
        public bool SetAvaliable(AppClientAvaliable type, bool avaliable, bool updateVersion = false)
        {
            AppClientAvaliable old = Avaliable;
            if (avaliable)
                Avaliable |= type;
            else
                Avaliable &= ~type;

            if (old != Avaliable)
            {
                if (updateVersion)
                    UpdateVersion();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取有效状态
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool GetAvaliable(AppClientAvaliable type)
        {
            return (Avaliable & type) == type;
        }

        /// <summary>
        /// 是否为有效状态
        /// </summary>
        /// <returns></returns>
        public bool IsAvaliable()
        {
            return Connected && GetAvaliable(AppClientAvaliable.Avaliable);
        }

        /// <summary>
        /// 更新版本号
        /// </summary>
        public void UpdateVersion()
        {
            LastUpdate = DateTime.UtcNow;
        }

        /// <summary>
        /// 修改部署地图
        /// </summary>
        /// <param name="info">修改信息</param>
        /// <param name="updateVersion">是否修改版本号</param>
        /// <returns>是否有所改动</returns>
        public bool Adjust(AppClientAdjustInfo info, bool updateVersion = false)
        {
            if (info == null)
                return false;

            bool modified = _AdjustServiceDeployInfos(info.ServicesToStart, info.ServicesToStop)
                || _AdjustCommunicationInfos(info.CommunicationsToOpen, info.CommunicationsToClose);

            if (modified && updateVersion)
                UpdateVersion();

            return modified;
        }

        private bool _AdjustCommunicationInfos(CommunicationOption[] toOpen, ServiceAddress[] toClose)
        {
            if (toOpen.IsNullOrEmpty() && toClose.IsNullOrEmpty())
                return false;

            bool modified = false;
            CommunicateOptions = CommunicateOptions.Adjust(toOpen,
                    _ConvertCommunication(toClose), CommunicationOption.Comparer, (v, t) => modified = true).ToArray();

            return modified;
        }

        private bool _AdjustServiceDeployInfos(ServiceDesc[] toStart, ServiceDesc[] toStop)
        {
            if (Services == null)
                Services = Array<AppServiceDeployInfo>.Empty;

            if (toStart.IsNullOrEmpty() && toStop.IsNullOrEmpty())
                return false;

            Dictionary<ServiceDesc, AppServiceDeployInfo> dict = Services.ToDictionary(v => v.ServiceDesc);

            bool modified = false;
            ServiceDesc[] sds = Services.Select(v => v.ServiceDesc).Adjust(toStart, toStop, null, (v, t) => modified = true).ToArray();
            Services = sds.ToArray(sd => dict.GetOrDefault(sd) ?? new AppServiceDeployInfo(sd));

            return modified;
        }

        // 将ServiceAddress转换为CommunicationOption，提供给Adjust方法作为参数
        private static IEnumerable<CommunicationOption> _ConvertCommunication(ServiceAddress[] serviceAddress)
        {
            if (serviceAddress.IsNullOrEmpty())
                return null;

            return serviceAddress.Select(sa => new CommunicationOption(sa));
        }

        /// <summary>
        /// 合并两个部署地图
        /// </summary>
        /// <param name="dInfo"></param>
        /// <param name="updateVersion"></param>
        /// <returns></returns>
        public bool Combine(AppClientDeployInfo dInfo, bool updateVersion = false)
        {
            if (dInfo == null)
                return false;

            if(dInfo.ClientId != this.ClientId && dInfo.ClientId!=Guid.Empty)
                throw new ArgumentException("需要合并的两个部署地图需要具有相同的ClientID，或者留空");

            AppClientAdjustInfo info = new AppClientAdjustInfo {
                CommunicationsToOpen = dInfo.CommunicateOptions,
                ServicesToStart = dInfo.Services.ToArray(v => v.ServiceDesc),
            };

            return Adjust(info, updateVersion);
        }
    }

    [Flags]
    public enum AppClientAvaliable
    {
        /// <summary>
        /// 人工设置的有效状态
        /// </summary>
        [Desc("人工设置的有效状态")]
        ByManual = 1,

        /// <summary>
        /// 终端自己设置的有效状态
        /// </summary>
        [Desc("终端自己设置的有效状态")]
        ByClient = 1 << 1,

        /// <summary>
        /// 监控服务设置的有效状态
        /// </summary>
        [Desc("监控服务设置的有效状态")]
        ByWatch = 1 << 2,

        Avaliable = ByManual | ByClient | ByWatch,
    }
}
