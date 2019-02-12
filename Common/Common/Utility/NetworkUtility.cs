using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Management;

namespace Common.Utility
{
    /// <summary>
    /// 网络工具
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class NetworkUtility
    {
        public static NetworkAdapterInfo[] GetAllNetworkAdapterInfos()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            return moc.Cast<ManagementObject>().Select(_ConvertToNetworkAdapterInfo).WhereNotNull().ToArray();
        }

        private static NetworkAdapterInfo _ConvertToNetworkAdapterInfo(ManagementObject mo)
        {
            if (mo["IPAddress"] == null)
                return null;

            return new NetworkAdapterInfo(
                (string)mo["Caption"], (string)mo["ServiceName"], (string)mo["MACAddress"], (bool)mo["IPEnabled"],
                ((string[])mo["IPAddress"])._ToArray(ip => IPAddress.Parse(ip)),
                (string[])mo["IPSubnet"] ?? Array<string>.Empty,
                ((string[])mo["DefaultIPGateway"])._ToArray(ip => IPAddress.Parse(ip)),
                ((string[])mo["DNSServerSearchOrder"])._ToArray(ip => IPAddress.Parse(ip)));
        }

        private static TTarget[] _ToArray<TSource, TTarget>(this IList<TSource> source, Func<TSource, TTarget> converter)
        {
            if (source == null)
                return Array<TTarget>.Empty;

            return source.ToArray(converter);
        }

        /// <summary>
        /// 获取所有有效的IP4地址
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetAllEnableIP4Addresses()
        {
            return GetAllNetworkAdapterInfos().Where(v => v.Enabled).SelectMany(v => v.IPAddresses).Where(v => !v.IsIPv6LinkLocal).ToArray();
        }

        /// <summary>
        /// 获取计算机的名称
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }
    }

    /// <summary>
    /// 网卡的信息
    /// </summary>
    public class NetworkAdapterInfo
    {
        public NetworkAdapterInfo(string caption, string serviceName, string macAddress, bool enabled,
            IPAddress[] ipAddresses, string[] subnets, IPAddress[] gateways, IPAddress[] dnses)
        {
            Caption = caption;
            ServiceName = serviceName;
            MacAddress = macAddress;
            Enabled = enabled;
            IPAddresses = ipAddresses;
            Subnets = subnets;
            Gateways = gateways;
            Dnses = dnses;
        }

        public bool Enabled { get; private set; }

        public string Caption { get; private set; }

        public string ServiceName { get; private set; }

        public string MacAddress { get; private set; }

        public IPAddress[] IPAddresses { get; private set; }

        public string[] Subnets { get; private set; }

        public IPAddress[] Gateways { get; private set; }

        public IPAddress[] Dnses { get; private set; }
    }
}
