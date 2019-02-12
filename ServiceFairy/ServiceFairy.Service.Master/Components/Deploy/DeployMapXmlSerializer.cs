using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml;
using Common.Utility;
using Common.Framework.TrayPlatform;
using Common.Contracts.Service;
using Common.Package.Serializer;
using Common.Contracts;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.Master.Components.Deploy
{
    static class DeployMapXmlSerializer
    {
        /// <summary>
        /// 序列化为XML
        /// </summary>
        /// <param name="deployMap"></param>
        /// <returns></returns>
        public static string Serialize(AppClientDeployMap deployMap)
        {
            Contract.Requires(deployMap != null);

            return XmlUtility.SerializeToString(new DeployMapElement(deployMap), false, Formatting.Indented);
        }

        /// <summary>
        /// 将XML反序列化为AppClientDeployMap
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static AppClientDeployMap Deserialize(string xml)
        {
            Contract.Requires(xml != null);

            DeployMapElement deployMapElement = XmlUtility.Deserialize<DeployMapElement>(xml, false);
            return deployMapElement.ToDeployMap();
        }
    }
}
