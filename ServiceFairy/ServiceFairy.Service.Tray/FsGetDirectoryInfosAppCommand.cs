using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;
using Common.Utility;
using ServiceFairy.Service.Tray.Components;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取指定目录的信息及子目录、文件信息
    /// </summary>
    [AppCommand("FsGetDirectoryInfos", title: "获取指定目录的信息及子目录、文件信息", SecurityLevel = SecurityLevel.Admin)]
    class FsGetDirectoryInfosAppCommand : ACS<Service>.Func<Tray_FsGetDirectoryInfos_Request, Tray_FsGetDirectoryInfos_Reply>
    {
        protected override Tray_FsGetDirectoryInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_FsGetDirectoryInfos_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            var fs = srv.FileSystem;

            FsDirectoryInfoItem[] items = req.Directories.ToArray(d => _CreateDirectoryInfoItem(fs, d, req.Option, req.FullPath));

            return new Tray_FsGetDirectoryInfos_Reply() { Items = items };
        }

        private FsDirectoryInfoItem _CreateDirectoryInfoItem(FileSystemManagerAppComponent fs, string directory, FsGetDirectoryInfosOption option, bool fullPath)
        {
            return new FsDirectoryInfoItem() {
                Directory = directory,
                DirectoryInfo = fs.GetDirectoryInfo(directory),
                SubDirectoriesInfos = (option & FsGetDirectoryInfosOption.SubDirectories) != 0 ? _Revise(fs.GetSubDirectoryInfos(directory), directory, fullPath) : null,
                FileInfos = (option & FsGetDirectoryInfosOption.Files) != 0 ? _Revise(fs.GetFileInfos(directory), directory, fullPath) : null
            };
        }

        private T[] _Revise<T>(T[] infos, string directory, bool fullPath) where T : FsPathInfo
        {
            if (!fullPath)
                infos.ForEach(info => info.Path = info.Path.Substring(directory.Length).TrimStart('\\'));

            return infos;
        }
    }
}
