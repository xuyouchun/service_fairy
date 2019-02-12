using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities.Tray;
using N = ServiceFairy.Entities.Tray.SystemPropertyNames;

namespace ServiceFairy.Service.Tray.Components
{
    /// <summary>
    /// 系统信息管理器
    /// </summary>
    [AppComponent("系统信息管理器", "收集内存、硬盘、系统信息等数据供查阅")]
    class SystemInformationManagerAppComponent : AppComponent
    {
        public SystemInformationManagerAppComponent(Service service)
            : base(service)
        {
                
        }

        /// <summary>
        /// 获取全部系统属性
        /// </summary>
        /// <returns></returns>
        public SystemProperty[] GetAllSystemProperties()
        {
            List<SystemProperty> list = new List<SystemProperty>();
            foreach (N.PropertyItem item in N.AllItems)
            {
                SystemProperty p = _GetSystemProperty(item);
                if (p != null)
                    list.Add(p);
            }

            return list.ToArray();
        }

        private SystemProperty _GetSystemProperty(N.PropertyItem item)
        {
            string value = "";
            switch (item.Name)
            {
                case N.OpSysName:  // 操作系统
                    value = _GetValue(() => Environment.OSVersion.ToString());
                    break;

                case N.PhysicalMemorySize:  // 物理内存
                    value = _GetValue(() => StringUtility.GetSizeString(_GetPhysicalMemorySize()));
                    break;

                case N.AvaliableMemorySize:  // 可用内存
                    value = _GetValue(() => StringUtility.GetSizeString(_GetAvaliableMemorySize()));
                    break;

                case N.HdSize:  // 硬盘容量
                    value = _GetValue(() => StringUtility.GetSizeString(_GetHdSize()));
                    break;

                case N.HdFreeSize:  // 硬盘可用空间大小
                    value = _GetValue(() => StringUtility.GetSizeString(_GetHdFreeSpace()));
                    break;

                case N.CpuInfo:  // CPU信息
                    value = _GetValue(() => _GetCpuInfo());
                    break;
            }

            return new SystemProperty() { Name = item.Name, Desc = item.Desc, Value = value };
        }

        private string _GetValue(Func<string> func, string def = "")
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return def;
            }
        }

        // 获取物理内存大小
        private static long _GetPhysicalMemorySize()
        {
            using (ManagementClass cimobject = new ManagementClass("Win32_PhysicalMemory"))
            using (ManagementObjectCollection moc = cimobject.GetInstances())
            {
                long size = 0L;
                foreach (ManagementObject mo in moc)
                {
                    size += long.Parse(mo.Properties["Capacity"].Value.ToString());
                }

                return size;
            }
        }

        // 获取可用内存大小
        private static long _GetAvaliableMemorySize()
        {
            using(ManagementClass cimobject = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory"))
            using (ManagementObjectCollection moc = cimobject.GetInstances())
            {
                long size = 0L;
                foreach (ManagementObject mo in moc)
                {
                    size += long.Parse(mo.Properties["AvailableMBytes"].Value.ToString()) * 1024 * 1024;
                }

                return size;
            }
        }

        // 获取硬盘容量
        private static long _GetHdSize()
        {
            long size = 0;
            foreach (DriveInfo dInfo in DriveInfo.GetDrives())
            {
                if (dInfo.DriveType == DriveType.Fixed)
                {
                    size += dInfo.TotalSize;
                }
            }

            return size;
        }

        // 获取硬盘可用空间大小
        private static long _GetHdFreeSpace()
        {
            long size = 0;
            foreach (DriveInfo dInfo in DriveInfo.GetDrives())
            {
                if (dInfo.DriveType == DriveType.Fixed)
                {
                    size += dInfo.AvailableFreeSpace;
                }
            }

            return size;
        }

        // CPU信息
        private static string _GetCpuInfo()
        {
            using(ManagementClass mc = new ManagementClass("Win32_Processor"))
            using (ManagementObjectCollection moc = mc.GetInstances())
            {
                foreach (ManagementObject mo in moc)
                {
                    return string.Format("{0}; {1} cores; {2} processors; {3} bits",
                        mo["Name"], mo["NumberOfCores"], mo["NumberOfLogicalProcessors"], mo["AddressWidth"]);
                }
            }

            return "";
        }

        /// <summary>
        /// 获取指定名称的系统属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SystemProperty GetSystemProperty(string name)
        {
            Contract.Requires(name != null);

            N.PropertyItem item = N.Get(name);
            if (item == null)
                return null;

            return _GetSystemProperty(item);
        }

        /// <summary>
        /// 读取所有环境变量
        /// </summary>
        /// <returns></returns>
        public SystemEnvironmentVariable[] GetAllEnvironmentVariables()
        {
            List<SystemEnvironmentVariable> values = new List<SystemEnvironmentVariable>();
            IDictionary dict = Environment.GetEnvironmentVariables();

            foreach (DictionaryEntry item in dict)
            {
                object key = item.Key, value = item.Value;
                if (key != null)
                {
                    values.Add(new SystemEnvironmentVariable { Name = key.ToString(), Value = value.ToStringIgnoreNull() });
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// 读取指定名称的环境变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SystemEnvironmentVariable GetEnvironmentVariable(string name)
        {
            string value;
            if (name == null || (value = Environment.GetEnvironmentVariable(name)) == null)
                return null;

            return new SystemEnvironmentVariable { Name = name, Value = value };
        }

        /// <summary>
        /// 设置指定名称的环境变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetEnvironmentVariable(string name, string value)
        {
            Contract.Requires(name != null);
            Environment.SetEnvironmentVariable(name, value);
        }
    }
}
