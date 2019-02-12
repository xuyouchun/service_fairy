using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;
using System.IO;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package;

namespace ServiceFairy.Service.Tray.Components
{
    /// <summary>
    /// 文件系统管理器
    /// </summary>
    [AppComponent("文件系统管理器", "负责文件系统读取、写入、删除等操作")]
    class FileSystemManagerAppComponent : AppComponent
    {
        public FileSystemManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;

            _rootInfos = new AutoLoad<FsDirectoryInfo[]>(() => _GetFileSystemRootDirectoryInfos().Distinct().ToArray());
        }

        private readonly Service _service;

        /// <summary>
        /// 获取路径信息
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public FsDirectoryInfo GetDirectoryInfo(string directory)
        {
            Contract.Requires(directory != null);

            try
            {
                DirectoryInfo info = new DirectoryInfo(directory);
                return new FsDirectoryInfo() {
                    CreationTime = info.CreationTimeUtc, Path = directory,
                    LastModifyTime = info.LastWriteTimeUtc, Name = Path.GetFileName(directory)
                };
            }
            catch (DirectoryNotFoundException)
            {
                throw TrayUtility.CreateException(TrayStatusCode.DirectoryNotFound, "路径“" + directory + "”不存在");
            }
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FsFileInfo GetFileInfo(string file)
        {
            Contract.Requires(file != null);

            try
            {
                FileInfo info = new FileInfo(file);
                return new FsFileInfo() {
                    CreationTime = info.CreationTimeUtc,
                    LastModifyTime = info.LastWriteTimeUtc,
                    Name = Path.GetFileName(file),
                    Size = info.Length,
                    Path = file,
                };
            }
            catch (FileNotFoundException)
            {
                throw TrayUtility.CreateException(TrayStatusCode.FileNotFound, "文件“" + file + "”不存在");
            }
        }

        /// <summary>
        /// 获取指定路径中所有子路径的信息
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public FsDirectoryInfo[] GetSubDirectoryInfos(string directory)
        {
            Contract.Requires(directory != null);

            return Directory.GetDirectories(directory).ToArray(GetDirectoryInfo);
        }

        /// <summary>
        /// 获取指定路径中所有的信息
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public FsFileInfo[] GetFileInfos(string directory)
        {
            Contract.Requires(directory != null);

            return Directory.GetFiles(directory).ToArray(GetFileInfo);
        }

        /// <summary>
        /// 获取文件顶级路径的信息
        /// </summary>
        /// <returns></returns>
        public FsDirectoryInfo[] GetFileSystemRootDirectoryInfos()
        {
            return _rootInfos.Value;
        }

        private readonly AutoLoad<FsDirectoryInfo[]> _rootInfos;

        private FsDirectoryInfo _TryGetDirectoryInfo(string directory)
        {
            try
            {
                return GetDirectoryInfo(directory);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return null;
            }
        }

        private IEnumerable<FsDirectoryInfo> _GetFileSystemRootDirectoryInfos()
        {
            foreach (LogicDriverInfo driverInfo in SystemUtility.GetAllLogicDriverInfos())
            {
                FsDirectoryInfo info = _TryGetDirectoryInfo(driverInfo.Name + "\\");
                if (info == null)
                    continue;

                info.Name = driverInfo.Name;
                info.SpecialFolder = FsDirectoryInfo.GetLogicDriverType(driverInfo.Type);
                info.Title = StringUtility.GetFirstNotNullOrWhiteSpaceString(driverInfo.VolumeName, driverInfo.Type.GetDesc());
                info.Desc = driverInfo.Type.GetDesc();

                yield return info;
            }

            var servicePaths = new [] {
                new { Name = "Running", Path =　SFSettings.GetAppClientRunningPath(_service.RunningBasePath, _service.Context.ClientID), SpecialFolder = FsDirectoryInfo.ServicePath, Title = "服务运行路径", Desc = "服务的安装包被解压到该文件夹中加载运行" },
                new { Name = "Data", Path = _service.DataPath, SpecialFolder = FsDirectoryInfo.DataPath, Title = "数据路径", Desc = "存放服务运行时产生的数据" },
                new { Name = "Service", Path = _service.ServiceBasePath, SpecialFolder = FsDirectoryInfo.ServicePath, Title = "当前版本服务路径", Desc = "存放当前版本服务的程序集" },
                new { Name = "Log", Path = _service.LogPath, SpecialFolder = FsDirectoryInfo.LogPath, Title = "日志路径", Desc = "存放服务及平台的日志" },
                new { Name = "Install", Path = _service.InstallPath, SpecialFolder = FsDirectoryInfo.InstallPath, Title = "安装路径", Desc = "该系统的安装路径" },
            };

            foreach (var item in servicePaths)
            {
                if (!Directory.Exists(item.Path))
                    continue;

                FsDirectoryInfo info = GetDirectoryInfo(item.Path);
                info.SpecialFolder = item.SpecialFolder; 
                info.Title = item.Title;
                info.Name = item.Name;
                info.Desc = item.Desc; // _GetSpecialFolderDesc(item.SpecialFolder);

                yield return info;
            }

            foreach (Environment.SpecialFolder folder in Enum.GetValues(typeof(Environment.SpecialFolder)))
            {
                string path = Environment.GetFolderPath(folder);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    FsDirectoryInfo info = GetDirectoryInfo(path);
                    info.Title = _GetSpecialFolderTitle(folder);
                    info.Desc = _GetSpecialFolderDesc(folder);
                    info.Name = _GetSpecialFolderName(folder);
                    info.SpecialFolder = folder;

                    yield return info;
                }
            }
        }

        private string _GetSpecialFolderName(Environment.SpecialFolder folder)
        {
            switch (folder)
            {
                case Environment.SpecialFolder.CDBurning:
                    return "CD Burning";
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in folder.ToString())
            {
                if (char.IsUpper(c) && sb.Length > 0)
                    sb.Append(" ");

                sb.Append(c);
            }

            return sb.ToString().Replace("X86", "x86");
        }

        private string _GetSpecialFolderDesc(Environment.SpecialFolder folder)
        {
            switch (folder)
            {
                case Environment.SpecialFolder.AdminTools:
                    return "管理工具路径";

                case Environment.SpecialFolder.ApplicationData:
                    return "当前漫游用户的应用程序特定数据的公共储存库";

                case Environment.SpecialFolder.CDBurning:
                    return "光盘刻录文件的暂存路径";

                case Environment.SpecialFolder.CommonAdminTools:
                    return "存储各个用户的管理工具";

                case Environment.SpecialFolder.CommonApplicationData:
                    return "所有用户使用的应用程序特定数据的公共储存库";

                case Environment.SpecialFolder.CommonDesktopDirectory:
                    return "包含在所有用户桌面上出现的文件和文件夹";

                case Environment.SpecialFolder.CommonDocuments:
                    return "包含所有用户共有的文档";

                case Environment.SpecialFolder.CommonMusic:
                    return "包含所有用户共有的音乐";

                case Environment.SpecialFolder.CommonOemLinks:
                    return "为了实现向后兼容，Windows Vista中可以识别此值，但该特殊文件夹本身已不再使用";

                case Environment.SpecialFolder.CommonPictures:
                    return "包含所有用户共有的图片";

                case Environment.SpecialFolder.CommonProgramFiles:
                    return "应用程序安装路径";

                case Environment.SpecialFolder.CommonProgramFilesX86:
                    return "x86应用程序安装路径";

                case Environment.SpecialFolder.CommonPrograms:
                    return "存放多个应用程序共享的组件";

                case Environment.SpecialFolder.CommonStartMenu:
                    return "包含所有用户的“开始”菜单上都出现的程序和文件夹";

                case Environment.SpecialFolder.CommonStartup:
                    return "包含所有用户的“启动”文件夹中都出现的程序";

                case Environment.SpecialFolder.CommonTemplates:
                    return "包含所有用户都可以使用的模板";

                case Environment.SpecialFolder.CommonVideos:
                    return "包含所有用户共有的视频";

                case Environment.SpecialFolder.Cookies:
                    return "Internet Cookie 的公共储存库的路径";

                case Environment.SpecialFolder.Desktop:
                    return "逻辑桌面，而不是物理文件系统位置";

                case Environment.SpecialFolder.DesktopDirectory:
                    return "用户物理上存储桌面上的文件对象的路径";

                case Environment.SpecialFolder.Favorites:
                    return "用户收藏夹项的公共储存库的路径";

                case Environment.SpecialFolder.Fonts:
                    return "字体路径";

                case Environment.SpecialFolder.History:
                    return "Internet历史记录项的公共存储库路径";

                case Environment.SpecialFolder.InternetCache:
                    return "Internet临时文件的公共存储库路径";

                case Environment.SpecialFolder.LocalApplicationData:
                    return "当前非漫游用户使用的应用程序特定数据的公共储存库";

                case Environment.SpecialFolder.LocalizedResources:
                    return "包括用户本地化资源数据";

                case Environment.SpecialFolder.MyComputer:
                    return "我的电脑";

                case Environment.SpecialFolder.MyDocuments:
                    return "“我的文档”文件夹";

                case Environment.SpecialFolder.MyMusic:
                    return "“我的音乐”文件夹";

                case Environment.SpecialFolder.MyPictures:
                    return "“我的图片”文件夹";

                case Environment.SpecialFolder.MyVideos:
                    return "“我的视频”文件夹";

                case Environment.SpecialFolder.NetworkShortcuts:
                    return "“网上邻居”虚拟文件夹中可能存在的链接对象";

                /*case Environment.SpecialFolder.Personal:
                    break;*/

                case Environment.SpecialFolder.PrinterShortcuts:
                    return "“打印机”虚拟文件夹中可能存在的链接对象";

                case Environment.SpecialFolder.ProgramFiles:
                    return "应用程序安装路径";

                case Environment.SpecialFolder.ProgramFilesX86:
                    return "x86应用程序安装路径";

                case Environment.SpecialFolder.Programs:
                    return "包含用户程序组的路径";

                case Environment.SpecialFolder.Recent:
                    return "包含最近使用过的文档的路径";

                case Environment.SpecialFolder.Resources:
                    return "包含资源数据";

                case Environment.SpecialFolder.SendTo:
                    return "包含“发送”菜单项的路径";

                case Environment.SpecialFolder.StartMenu:
                    return "包含“开始”菜单项的路径";

                case Environment.SpecialFolder.Startup:
                    return "用户“启动”程序组的路径";

                case Environment.SpecialFolder.System:
                    return "系统路径";

                case Environment.SpecialFolder.SystemX86:
                    return "x86系统路径";

                case Environment.SpecialFolder.Templates:
                    return "文档模板的公共存储库路径";

                case Environment.SpecialFolder.UserProfile:
                    return "包含用户配置文件的文件夹";

                case Environment.SpecialFolder.Windows:
                    return "Windows安装路径";

                default:
                    break;
            }

            return "";
        }

        private string _GetSpecialFolderTitle(Environment.SpecialFolder folder)
        {
            switch (folder)
            {
                case Environment.SpecialFolder.AdminTools:
                    return "管理工具路径";

                case Environment.SpecialFolder.ApplicationData:
                    return "应用程序数据";

                case Environment.SpecialFolder.CDBurning:
                    return "光盘刻录文件暂存路径";

                case Environment.SpecialFolder.CommonAdminTools:
                    return "公共管理工具路径";

                case Environment.SpecialFolder.CommonApplicationData:
                    return "公共应用程序数据";

                case Environment.SpecialFolder.CommonDesktopDirectory:
                    return "公共桌面";

                case Environment.SpecialFolder.CommonDocuments:
                    return "公共文档";

                case Environment.SpecialFolder.CommonMusic:
                    return "公共音乐";

                case Environment.SpecialFolder.CommonOemLinks:
                    return "公共OEM链接";

                case Environment.SpecialFolder.CommonPictures:
                    return "公共图片";

                case Environment.SpecialFolder.CommonProgramFiles:
                    return "公共应用程序安装路径";

                case Environment.SpecialFolder.CommonProgramFilesX86:
                    return "公共X86应用程序安装路径";

                case Environment.SpecialFolder.CommonPrograms:
                    return "公共应用程序";

                case Environment.SpecialFolder.CommonStartMenu:
                    return "公共菜单";

                case Environment.SpecialFolder.CommonStartup:
                    return "公共启动路径";

                case Environment.SpecialFolder.CommonTemplates:
                    return "公共模板";

                case Environment.SpecialFolder.CommonVideos:
                    return "公共视频";

                case Environment.SpecialFolder.Cookies:
                    return "Internal Cookies";

                case Environment.SpecialFolder.Desktop:
                    return "桌面";

                case Environment.SpecialFolder.DesktopDirectory:
                    return "桌面";

                case Environment.SpecialFolder.Favorites:
                    return "收藏夹";

                case Environment.SpecialFolder.Fonts:
                    return "字体";

                case Environment.SpecialFolder.History:
                    return "历史记录";

                case Environment.SpecialFolder.InternetCache:
                    return "网络临时文件";

                case Environment.SpecialFolder.LocalApplicationData:
                    return "本地应用程序数据";

                case Environment.SpecialFolder.LocalizedResources:
                    return "本地化资源数据";

                case Environment.SpecialFolder.MyComputer:
                    return "我的电脑";

                case Environment.SpecialFolder.MyDocuments:
                    return "我的文档";

                case Environment.SpecialFolder.MyMusic:
                    return "我的音乐";

                case Environment.SpecialFolder.MyPictures:
                    return "我的图片";

                case Environment.SpecialFolder.MyVideos:
                    return "我的视频";

                case Environment.SpecialFolder.NetworkShortcuts:
                    return "网上邻居路径";

                /*case Environment.SpecialFolder.Personal:
                    break;*/

                case Environment.SpecialFolder.PrinterShortcuts:
                    return "打印机路径";

                case Environment.SpecialFolder.ProgramFiles:
                    return "应用程序安装路径";

                case Environment.SpecialFolder.ProgramFilesX86:
                    return "x86应用程序安装路径";

                case Environment.SpecialFolder.Programs:
                    return "应用程序安装路径";

                case Environment.SpecialFolder.Recent:
                    return "最近使用的文档";

                case Environment.SpecialFolder.Resources:
                    return "资源路径";

                case Environment.SpecialFolder.SendTo:
                    return "发送到";

                case Environment.SpecialFolder.StartMenu:
                    return "开始菜单路径";

                case Environment.SpecialFolder.Startup:
                    return "启动路径";

                case Environment.SpecialFolder.System:
                    return "系统路径";

                case Environment.SpecialFolder.SystemX86:
                    return "x86系统路径";

                case Environment.SpecialFolder.Templates:
                    return "模板";

                case Environment.SpecialFolder.UserProfile:
                    return "用户配置文件";

                case Environment.SpecialFolder.Windows:
                    return "Windows安装路径";

                default:
                    break;
            }

            return "";
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] ReadAllBytes(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (FileNotFoundException)
            {
                throw TrayUtility.CreateException(TrayStatusCode.FileNotFound, "文件“" + path + "”不存在");
            }
        }

        /// <summary>
        /// 删除文件或目录
        /// </summary>
        /// <param name="paths"></param>
        public void Delete(string[] paths)
        {
            Contract.Requires(paths != null);

            foreach (string path in paths)
            {
                PathUtility.DeleteIfExists(path);
            }
        }
    }
}
