using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Common.Framework.TrayPlatform;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using Common;
using Common.Utility;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.Master.Components.Deploy
{
    [XmlRoot("deployMap", Namespace = "ServiceFairy/Service/Master")]
    public class DeployMapElement
    {
        public DeployMapElement()
        {
            DeployInfos = Array<DeployInfoElement>.Empty;
        }

        public DeployMapElement(AppClientDeployMap deployMap)
            : this()
        {
            AppClientDeployInfo[] dInfos;
            if (deployMap != null && (dInfos = deployMap.GetAll()) != null)
            {
                DeployInfos = dInfos.ToArray(dInfo => new DeployInfoElement(dInfo));
            }
        }

        [XmlArray("deployInfos"), XmlArrayItem("deployInfo")]
        public DeployInfoElement[] DeployInfos { get; set; }

        public AppClientDeployMap ToDeployMap()
        {
            AppClientDeployMap deployMap = new AppClientDeployMap();
            deployMap.AddDeployInfos(DeployInfos.ToArray(dInfo => dInfo.ToDeployInfo()), false);
            return deployMap;
        }
    }

    [XmlRoot("deployInfo")]
    public class DeployInfoElement
    {
        public DeployInfoElement()
        {
            Enable = true;
            Services = Array<DeployAppServiceElement>.Empty;
            Communications = Array<DeployCommunicationOptionElement>.Empty;
        }

        public DeployInfoElement(AppClientDeployInfo info)
            : this()
        {
            if (info != null)
            {
                ClientId = info.ClientId;
                Enable = info.GetAvaliable(AppClientAvaliable.ByManual);
                Services = (info.Services ?? Array<AppServiceDeployInfo>.Empty).ToArray(deployInfo => new DeployAppServiceElement(deployInfo));
                Communications = (info.CommunicateOptions ?? Array<CommunicationOption>.Empty).ToArray(op => new DeployCommunicationOptionElement(op));
            }
        }

        /// <summary>
        /// 终端标识
        /// </summary>
        [XmlAttribute("clientId")]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 服务
        /// </summary>
        [XmlArray("services"), XmlArrayItem("service")]
        public DeployAppServiceElement[] Services { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        /// <summary>
        /// 信道
        /// </summary>
        [XmlArray("communications"), XmlArrayItem("communication")]
        public DeployCommunicationOptionElement[] Communications { get; set; }

        public AppClientDeployInfo ToDeployInfo()
        {
            var info = new AppClientDeployInfo() {
                Services = Services.ToArray(service => service.ToDeployInfo()),
                CommunicateOptions = Communications.ToArray(communication => communication.ToCommunicationOption()),
                ClientId = ClientId,
            };

            info.SetAvaliable(AppClientAvaliable.ByManual, Enable);
            return info;
        }
    }

    [XmlRoot("service")]
    public class DeployAppServiceElement
    {
        public DeployAppServiceElement()
        {

        }

        public DeployAppServiceElement(AppServiceDeployInfo deployInfo)
            : this()
        {
            if (deployInfo != null)
            {
                Name = deployInfo.ServiceDesc.Name;
                Version = deployInfo.ServiceDesc.Version.ToString();
            }
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        public AppServiceDeployInfo ToDeployInfo()
        {
            return new AppServiceDeployInfo(new ServiceDesc(Name, Version));
        }
    }

    [XmlRoot("communication")]
    public class DeployCommunicationOptionElement
    {
        public DeployCommunicationOptionElement()
        {

        }

        public DeployCommunicationOptionElement(CommunicationOption op)
            : this()
        {
            if (op != null)
            {
                Address = (op.Address == null) ? "" : op.Address.ToString();
                Type = op.Type;
                Duplex = op.Duplex;
            }
        }

        /// <summary>
        /// 终端
        /// </summary>
        [XmlAttribute("address")]
        public string Address { get; set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        [XmlAttribute("type")]
        public CommunicationType Type { get; set; }

        /// <summary>
        /// 是否支持双向通信
        /// </summary>
        [XmlAttribute("duplex")]
        public bool Duplex { get; set; }

        public CommunicationOption ToCommunicationOption()
        {
            return new CommunicationOption(ServiceAddress.Parse(Address), Type, Duplex);
        }
    }
}
