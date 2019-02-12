using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Package.Serializer;
using Common.Utility;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Collections;
using Common.Collection;
using System.Collections.Concurrent;
using Common.Communication.Wcf;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Common.Package;
using Common.Contracts;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;
using ServiceFairy.Entities.Navigation;

namespace FreeTest
{
    unsafe class Program
    {
        static void Main(string[] args)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                foreach (PropertyData pd in mo.Properties)
                {
                    Console.WriteLine("{0} = {1}", pd.Name, pd.Value);
                }

                Console.WriteLine("\r\n");
            }
        }
    }
}
