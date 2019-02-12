using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;

namespace Common.Utility
{
    /// <summary>
    /// 系统工具
    /// </summary>
    public static class SystemUtility
    {
        static SystemUtility()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        /// <summary>
        /// 获取所有逻辑驱动器的信息
        /// </summary>
        /// <returns></returns>
        public static LogicDriverInfo[] GetAllLogicDriverInfos()
        {
            List<LogicDriverInfo> infos = new List<LogicDriverInfo>();

            SelectQuery query = new SelectQuery("Select * From Win32_LogicalDisk ");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementBaseObject disk in searcher.Get())
            {
                infos.Add(new LogicDriverInfo((string)disk["Name"], 
                    (DriveType)disk["DriveType"].ToType<int>(), (string)disk["VolumeName"]));
            }

            return infos.ToArray();
        }

        /// <summary>
        /// 应用程序退出事件
        /// </summary>
        public static event EventHandler ApplicationExit
        {
            add { _applicationExit += value; }
            remove { _applicationExit -= value; }
        }

        private static EventHandler _applicationExit;

        // 应用程序退出事件
        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            EventHandler eh = _applicationExit;
            if (eh != null)
            {
                foreach (EventHandler eh0 in eh.GetInvocationList())
                {
                    try { eh0(sender, e); }
                    catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// 获取驱动器的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetDesc(this DriveType type)
        {
            switch (type)
            {
                case DriveType.CDRom:
                    return "光盘驱动器";

                case DriveType.Fixed:
                    return "本地磁盘";

                case DriveType.Network:
                    return "网络驱动器";

                case DriveType.NoRootDirectory:
                    return "无根路径";

                case DriveType.Ram:
                    return "RAM磁盘";

                case DriveType.Removable:
                    return "可移动存储设备";

                case DriveType.Unknown:
                    return "未知";

                default:
                    return "未知";
            }
        }
    }

    /// <summary>
    /// 逻辑驱动器的信息
    /// </summary>
    public class LogicDriverInfo
    {
        public LogicDriverInfo(string name, DriveType type, string volumeName)
        {
            Name = name;
            Type = type;
            VolumeName = volumeName;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 类型
        /// </summary>
        public DriveType Type { get; set; }

        /// <summary>
        /// 卷标
        /// </summary>
        public string VolumeName { get; set; }
    }
}
