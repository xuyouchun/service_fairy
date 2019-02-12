using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace IPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;

                string caption = (string)mo["Caption"];
                string serviceName = (string)mo["ServiceName"];
                string macAddress = (string)mo["MACAddress"];
                string[] addresses = (string[])mo["IPAddress"];
                string[] subnets = (string[])mo["IPSubnet"];
                string[] gateways = (string[])mo["DefaultIPGateway"];
                string[] dnses = (string[])mo["DNSServerSearchOrder"];

                continue;
            }
        }
    }
}
