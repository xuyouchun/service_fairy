using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using System.Xml;
using Common.Package;
using Common.Utility;
using Common.Communication.Wcf;

namespace ServiceFairy.Service.Tray
{
    static class ClientUtility
    {
        /// <summary>
        /// 获取初始要启动的服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static ServiceDesc[] LoadInitServices(Service service)
        {
            return _LoadInitServices(service.Context.ConfigReader.Get("initServices")).ToArray();
        }

        private static readonly char[] _separator = new char[] { ',', ';' };
        private static string[] _Split(string s)
        {
            return s.Trim(_separator).Split(_separator, StringSplitOptions.RemoveEmptyEntries).ToArray(s0 => s0.Trim());
        }

        private static IEnumerable<ServiceDesc> _LoadInitServices(string configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration))
                yield break;

            foreach (string item in _Split(configuration))
            {
                ServiceDesc sd = ServiceDesc.Parse(item);
                if (sd.Version.IsEmpty)
                    sd = new ServiceDesc(sd.Name, "1.0");
            }
        }

        /// <summary>
        /// 获取初始的信道
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static CommunicationOption[] LoadInitCommunications(Service service)
        {
            return _LoadInitCommunications(service.Context.ConfigReader.Get("initCommunications")).ToArray();
        }

        private static IEnumerable<CommunicationOption> _LoadInitCommunications(string configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration))
                yield break;

            foreach (string item in _Split(configuration))
            {
                yield return CommunicationOption.Parse(item);
            }
        }
    }
}
