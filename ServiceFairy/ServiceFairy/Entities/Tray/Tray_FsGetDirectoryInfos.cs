using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Common.Contracts;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取文件系统目录信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_FsGetDirectoryInfos_Request : RequestEntity
    {
        /// <summary>
        /// 目录列表
        /// </summary>
        [DataMember]
        public string[] Directories { get; set; }

        /// <summary>
        /// 调用选项
        /// </summary>
        [DataMember]
        public FsGetDirectoryInfosOption Option { get; set; }

        /// <summary>
        /// 是否返回全路径
        /// </summary>
        [DataMember]
        public bool FullPath { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Directories);
        }
    }

    [Flags]
    public enum FsGetDirectoryInfosOption
    {
        /// <summary>
        /// 不返回子目录及文件
        /// </summary>
        None = 0x00,

        /// <summary>
        /// 返回所包含的文件信息
        /// </summary>
        Files = 0x01,

        /// <summary>
        /// 返回子目录信息
        /// </summary>
        SubDirectories = 0x02,

        /// <summary>
        /// 返回子目录及所包含的文件信息
        /// </summary>
        All = -1,
    }

    /// <summary>
    /// 获取文件系统目录信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_FsGetDirectoryInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 目录信息及子目录、文件信息集合
        /// </summary>
        [DataMember]
        public FsDirectoryInfoItem[] Items { get; set; }
    }

    /// <summary>
    /// 目录信息及子目录、文件的信息集合
    /// </summary>
    [Serializable, DataContract]
    public class FsDirectoryInfoItem
    {
        /// <summary>
        /// 目录
        /// </summary>
        [DataMember]
        public string Directory { get; set; }

        /// <summary>
        /// 目录信息
        /// </summary>
        [DataMember]
        public FsDirectoryInfo DirectoryInfo { get; set; }

        /// <summary>
        /// 子目录信息
        /// </summary>
        [DataMember]
        public FsDirectoryInfo[] SubDirectoriesInfos { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        [DataMember]
        public FsFileInfo[] FileInfos { get; set; }
    }

    /// <summary>
    /// 目录或文件信息的基类
    /// </summary>
    [Serializable, DataContract]
    public class FsPathInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DataMember]
        public DateTime LastModifyTime { get; set; }

        public override string ToString()
        {
            return Path;
        }
    }

    /// <summary>
    /// 文件系统的目录信息
    /// </summary>
    [Serializable, DataContract]
    public class FsDirectoryInfo : FsPathInfo
    {
        public FsDirectoryInfo()
        {
            SpecialFolder = Normal;
        }

        /// <summary>
        /// 特殊文件夹
        /// </summary>
        [DataMember]
        public Environment.SpecialFolder SpecialFolder { get; set; }

        public const Environment.SpecialFolder Normal = (Environment.SpecialFolder)int.MaxValue;
        public const Environment.SpecialFolder RunningPath = (Environment.SpecialFolder)(int.MaxValue - 10);
        public const Environment.SpecialFolder ServicePath = (Environment.SpecialFolder)(int.MaxValue - 30);
        public const Environment.SpecialFolder DataPath = (Environment.SpecialFolder)(int.MaxValue - 31);
        public const Environment.SpecialFolder PackagePath = (Environment.SpecialFolder)(int.MaxValue - 40);
        public const Environment.SpecialFolder LogPath = (Environment.SpecialFolder)(int.MaxValue - 50);
        public const Environment.SpecialFolder InstallPath = (Environment.SpecialFolder)(int.MaxValue - 60);

        private const int LogicDriver_Base = int.MaxValue - 2000;

        public const Environment.SpecialFolder LogicDriver_Unknown = (Environment.SpecialFolder)(LogicDriver_Base + (int)DriveType.Unknown);
        public const Environment.SpecialFolder LogicDriver_CDRom = (Environment.SpecialFolder)(LogicDriver_Base + (int)DriveType.CDRom);
        public const Environment.SpecialFolder LogicDriver_Fixed = (Environment.SpecialFolder)(LogicDriver_Base + (int)DriveType.Fixed);
        public const Environment.SpecialFolder LogicDriver_Network = (Environment.SpecialFolder)(LogicDriver_Base + (int)DriveType.Network);
        public const Environment.SpecialFolder LogicDriver_NoRootDirectory = (Environment.SpecialFolder)(LogicDriver_Base + (int)DriveType.NoRootDirectory);
        public const Environment.SpecialFolder LogicDriver_Ram = (Environment.SpecialFolder)(LogicDriver_Base + (int)DriveType.Ram);
        public const Environment.SpecialFolder LogicDriver_Removable = (Environment.SpecialFolder)(LogicDriver_Base + (int)DriveType.Removable);

        public static Environment.SpecialFolder GetLogicDriverType(DriveType type)
        {
            return (Environment.SpecialFolder)(LogicDriver_Base + (int)type);
        }

        public static bool IsLogicDriverType(Environment.SpecialFolder folder)
        {
            return (int)folder >= LogicDriver_Base && (int)folder <= LogicDriver_Base + 100;
        }

        public static bool IsAppServiceSystemDirectory(Environment.SpecialFolder folder)
        {
            return (int)folder > int.MaxValue - 1000 && folder != Normal;
        }

        public override int GetHashCode()
        {
            return (int)SpecialFolder ^ (Path ?? "").ToLower().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(FsDirectoryInfo))
                return false;

            FsDirectoryInfo info = (FsDirectoryInfo)obj;
            return info.SpecialFolder == SpecialFolder && string.Equals(info.Path, Path, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator ==(FsDirectoryInfo obj1, FsDirectoryInfo obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(FsDirectoryInfo obj1, FsDirectoryInfo obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public override string ToString()
        {
            return Path;
        }
    }

    /// <summary>
    /// 文件系统的文件信息
    /// </summary>
    [Serializable, DataContract]
    public class FsFileInfo : FsPathInfo
    {
        /// <summary>
        /// 长度
        /// </summary>
        [DataMember]
        public long Size { get; set; }

        public override int GetHashCode()
        {
            return (Path ?? "").ToLower().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(FsFileInfo))
                return false;

            FsFileInfo info = (FsFileInfo)obj;
            return string.Equals(info.Path, Path, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator ==(FsFileInfo obj1, FsFileInfo obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(FsFileInfo obj1, FsFileInfo obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
