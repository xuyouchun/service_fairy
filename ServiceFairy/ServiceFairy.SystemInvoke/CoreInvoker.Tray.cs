using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Tray;
using System.Diagnostics.Contracts;

namespace ServiceFairy.SystemInvoke
{
	partial class CoreInvoker
	{
        private TrayInvoker _tray;

        /// <summary>
        /// Tray Service
        /// </summary>
        public TrayInvoker Tray
        {
            get { return _tray ?? (_tray = new TrayInvoker(this)); }
        }

        public class TrayInvoker : Invoker
        {
            public TrayInvoker(CoreInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 获取所有服务
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ServiceDesc[]> GetAllServicesSr(CallingSettings settings = null)
            {
                var sr = TrayService.GetAllServices(Sc, settings);
                return CreateSr(sr, r => r.Services);
            }

            /// <summary>
            /// 获取所有服务
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceDesc[] GetAllServices(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllServicesSr(settings));
            }

            /// <summary>
            /// 获取所有服务
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<ServiceDesc[]> GetAllServicesSr(Guid clientId, Sid sid = default(Sid))
            {
                return GetAllServicesSr(CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取所有服务
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceDesc[] GetAllServices(Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAllServicesSr(clientId, sid));
            }

            /// <summary>
            /// 判断指定的服务是否正在运行
            /// </summary>
            /// <param name="service">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<bool> ExistsServiceSr(ServiceDesc service, CallingSettings settings = null)
            {
                var sr = GetAllServicesSr(settings);
                return CreateSr(sr, r => r.Any(sd => sd == service));
            }

            /// <summary>
            /// 判断指定的服务是否正在运行
            /// </summary>
            /// <param name="service">服务</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public bool ExistsService(ServiceDesc service, CallingSettings settings = null)
            {
                return InvokeWithCheck(ExistsServiceSr(service, settings));
            }

            /// <summary>
            /// 判断指定的服务是否正在运行
            /// </summary>
            /// <param name="service">服务</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<bool> ExistsServiceSr(ServiceDesc service, Guid clientId, Sid sid = default(Sid))
            {
                return ExistsServiceSr(service, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 判断指定的服务是否正在运行
            /// </summary>
            /// <param name="service">服务</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public bool ExistsService(ServiceDesc service, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(ExistsServiceSr(service, clientId, sid));
            }

            /// <summary>
            /// 判断指定的服务是否正在运行
            /// </summary>
            /// <param name="endpoint">服务终端</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<bool> ExistsServiceSr(ServiceEndPoint endpoint, Sid sid = default(Sid))
            {
                Contract.Requires(endpoint != null);
                return ExistsServiceSr(endpoint.ServiceDesc, endpoint.ClientId, sid);
            }

            /// <summary>
            /// 判断指定的服务是否正在运行
            /// </summary>
            /// <param name="endpoint">服务终端</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public bool ExistsService(ServiceEndPoint endpoint, Sid sid = default(Sid))
            {
                return InvokeWithCheck(ExistsServiceSr(endpoint, sid));
            }

            /// <summary>
            /// 获取指定组下的子日志组
            /// </summary>
            /// <param name="parentGroup">父日志组</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<LogItemGroup[]> GetLocalLogGroupsSr(string parentGroup = "", CallingSettings settings = null)
            {
                var sr = TrayService.GetLocalLogGroups(Sc,
                    new Tray_GetLocalLogGroups_Request() { ParentGroup = parentGroup }, settings);

                return CreateSr(sr, r => r.Groups);
            }

            /// <summary>
            /// 获取指定组下的子日志组
            /// </summary>
            /// <param name="parentGroup">父日志组</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public LogItemGroup[] GetLocalLogGroups(string parentGroup = "", CallingSettings settings = null)
            {
                return InvokeWithCheck(GetLocalLogGroupsSr(parentGroup, settings));
            }

            /// <summary>
            /// 获取指定组下的子日志组
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <param name="parentGroup">父日志组</param>
            /// <returns></returns>
            public ServiceResult<LogItemGroup[]> GetLocalLogGroupsSr(Guid clientId, Sid sid = default(Sid), string parentGroup = "")
            {
                Contract.Requires(clientId != default(Guid));
                return GetLocalLogGroupsSr(parentGroup, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取指定组上的子日志组
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <param name="parentGroup">父日志组</param>
            /// <returns></returns>
            public LogItemGroup[] GetLocalLogGroups(Guid clientId, Sid sid = default(Sid), string parentGroup = "")
            {
                return InvokeWithCheck(GetLocalLogGroupsSr(clientId, sid, parentGroup));
            }

            /// <summary>
            /// 获取指定组的日志
            /// </summary>
            /// <param name="group">日志组</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<LogItem[]> GetLocalLogItemsSr(string group = "", CallingSettings settings = null)
            {
                var sr = TrayService.GetLocalLogItems(Sc,
                    new Tray_GetLocalLogItems_Request() { Group = group }, settings);

                return CreateSr(sr, r => StLogItem.ToLogItems(r.LogItems, r.StringTable));
            }

            /// <summary>
            /// 获取指定组的日志
            /// </summary>
            /// <param name="group">日志组</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public LogItem[] GetLocalLogItems(string group = "", CallingSettings settings = null)
            {
                return InvokeWithCheck(GetLocalLogItemsSr(group, settings));
            }

            /// <summary>
            /// 获取指定组的日志
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <param name="group">日志组</param>
            /// <returns></returns>
            public ServiceResult<LogItem[]> GetLocalLogItemsSr(Guid clientId, Sid sid = default(Sid), string group = "")
            {
                Contract.Requires(clientId != default(Guid));
                return GetLocalLogItemsSr(group, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获以指定组的日志
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <param name="group">日志组</param>
            /// <returns></returns>
            public LogItem[] GetLocalLogItems(Guid clientId, Sid sid = default(Sid), string group = "")
            {
                return InvokeWithCheck(GetLocalLogItemsSr(clientId, sid, group));
            }

            /// <summary>
            /// 根据时间获取本地日志
            /// </summary>
            /// <param name="startTime">起始时间</param>
            /// <param name="endTime">结束时间</param>
            /// <param name="maxCount">最大数量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<LogItem[]> GetLocalLogItemsByTimeSr(DateTime startTime, DateTime endTime, int maxCount = 0, CallingSettings settings = null)
            {
                var request = new Tray_GetLocalLogItemsByTime_Request { StartTime = startTime, EndTime = endTime, MaxCount = maxCount };
                var sr = TrayService.GetLocalLogItemsByTime(Sc, request, settings);
                return CreateSr(sr, r => StLogItem.ToLogItems(r.LogItems, r.StringTable));
            }

            /// <summary>
            /// 根据时间获取本地日志
            /// </summary>
            /// <param name="startTime">起始时间</param>
            /// <param name="endTime">结束时间</param>
            /// <param name="maxCount">最大数量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public LogItem[] GetLocalLogItemsByTime(DateTime startTime, DateTime endTime, int maxCount = 0, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetLocalLogItemsByTimeSr(startTime, endTime, maxCount, settings));
            }

            /// <summary>
            /// 根据时间获取本地日志
            /// </summary>
            /// <param name="startTime">起始时间</param>
            /// <param name="endTime">结束时间</param>
            /// <param name="maxCount">最大数量</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<LogItem[]> GetLocalLogItemsByTimeSr(DateTime startTime, DateTime endTime, int maxCount, Guid clientId, Sid sid = default(Sid))
            {
                return GetLocalLogItemsByTimeSr(startTime, endTime, maxCount, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 根据时间获取本地日志
            /// </summary>
            /// <param name="startTime">起始时间</param>
            /// <param name="endTime">结束时间</param>
            /// <param name="maxCount">最大数量</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public LogItem[] GetLocalLogItemsByTime(DateTime startTime, DateTime endTime, int maxCount, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetLocalLogItemsByTimeSr(startTime, endTime, maxCount, clientId, sid));
            }

            /// <summary>
            /// 删除本地日志组
            /// </summary>
            /// <param name="groups">日志组</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult DeleteLocalLogGroupsSr(string[] groups, CallingSettings settings = null)
            {
                Tray_DeleteLocalLogGroups_Request req = new Tray_DeleteLocalLogGroups_Request { Groups = groups };
                return TrayService.DeleteLocalLogGroups(Sc, req, settings);
            }

            /// <summary>
            /// 删除本地日志组
            /// </summary>
            /// <param name="groups">日志组</param>
            /// <param name="settings">调用设置</param>
            public void DeleteLocalLogGroups(string[] groups, CallingSettings settings = null)
            {
                InvokeWithCheck(DeleteLocalLogGroupsSr(groups, settings));
            }

            /// <summary>
            /// 删除本地日志组
            /// </summary>
            /// <param name="groups">日志组</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult DeleteLocalLogGroupsSr(string[] groups, Guid clientId, Sid sid = default(Sid))
            {
                return DeleteLocalLogGroupsSr(groups, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 删除本地日志组
            /// </summary>
            /// <param name="groups">日志组</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public void DeleteLocalLogGroups(string[] groups, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(DeleteLocalLogGroupsSr(groups, clientId, sid));
            }

            /// <summary>
            /// 删除本地日志组
            /// </summary>
            /// <param name="group">日志组</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult DeleteLocalLogGroupSr(string group, CallingSettings settings = null)
            {
                Contract.Requires(group != null);
                return DeleteLocalLogGroupsSr(new[] { group }, settings);
            }

            /// <summary>
            /// 删除本地日志组
            /// </summary>
            /// <param name="group">日志组</param>
            /// <param name="settings">调用设置</param>
            public void DeleteLocalLogGroup(string group, CallingSettings settings = null)
            {
                InvokeWithCheck(DeleteLocalLogGroupSr(group, settings));
            }

            /// <summary>
            /// 删除本地日志组
            /// </summary>
            /// <param name="group">日志组</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult DeleteLocalLogGroupSr(string group, Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(group != null);
                return DeleteLocalLogGroupsSr(new[] { group }, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 删除本地日志组
            /// </summary>
            /// <param name="group">日志组</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            public void DeleteLocalLogGroup(string group, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(DeleteLocalLogGroupSr(group, clientId, sid));
            }

            /// <summary>
            /// 获取系统日志的分组
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<SystemLogGroup[]> GetSystemLogGroupsSr(CallingSettings settings = null)
            {
                var sr = TrayService.GetSystemLogGroups(Sc, settings);
                return CreateSr(sr, r => r.Groups);
            }

            /// <summary>
            /// 获取系统日志的分组
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public SystemLogGroup[] GetSystemLogGroups(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetSystemLogGroupsSr(settings));
            }

            /// <summary>
            /// 获取系统日志的分组
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<SystemLogGroup[]> GetSystemLogGroupsSr(Guid clientId, Sid sid = default(Sid))
            {
                return GetSystemLogGroupsSr(CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取系统日志的分组
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public SystemLogGroup[] GetSystemLogGroups(Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetSystemLogGroupsSr(clientId, sid));
            }

            /// <summary>
            /// 获取系统日志
            /// </summary>
            /// <param name="group">组</param>
            /// <param name="start">起始</param>
            /// <param name="count">数量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Tray_GetSystemLogItems_Reply> GetSystemLogItemsSr(string group, int start, int count, CallingSettings settings = null)
            {
                Tray_GetSystemLogItems_Request req = new Tray_GetSystemLogItems_Request { Group = group, Start = start, Count = count };
                return TrayService.GetSystemLogItems(Sc, req, settings);
            }

            /// <summary>
            /// 获取系统日志
            /// </summary>
            /// <param name="group">组</param>
            /// <param name="start">起始</param>
            /// <param name="count">数量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Tray_GetSystemLogItems_Reply GetSystemLogItems(string group, int start, int count, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetSystemLogItemsSr(group, start, count, settings));
            }

            /// <summary>
            /// 获取系统日志
            /// </summary>
            /// <param name="group">组</param>
            /// <param name="start">起始</param>
            /// <param name="count">数量</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<Tray_GetSystemLogItems_Reply> GetSystemLogItemsSr(string group, int start, int count, Guid clientId, Sid sid = default(Sid))
            {
                return GetSystemLogItemsSr(group, start, count, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取系统日志
            /// </summary>
            /// <param name="group">组</param>
            /// <param name="start">起始</param>
            /// <param name="count">数量</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public Tray_GetSystemLogItems_Reply GetSystemLogItems(string group, int start, int count, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetSystemLogItemsSr(group, start, count, clientId, sid));
            }

            /// <summary>
            /// 获取文件系统顶级目录的信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<FsDirectoryInfo[]> FsGetRootDirectoryInfosSr(CallingSettings settings = null)
            {
                var sr = TrayService.FsGetRootDirectoryInfos(Sc, settings);

                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取文件系统顶级目录的信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public FsDirectoryInfo[] FsGetRootDirectoryInfos(CallingSettings settings = null)
            {
                return InvokeWithCheck(FsGetRootDirectoryInfosSr(settings));
            }

            /// <summary>
            /// 获取文件系统顶级目录的信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<FsDirectoryInfo[]> FsGetRootDirectoryInfosSr(Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid));

                return FsGetRootDirectoryInfosSr(CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取文件系统顶级目录的信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public FsDirectoryInfo[] FsGetRootDirectoryInfos(Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(FsGetRootDirectoryInfosSr(clientId, sid: sid));
            }

            /// <summary>
            /// 获取指定目录的子目录及文件信息
            /// </summary>
            /// <param name="directories">目录</param>
            /// <param name="option">选项</param>
            /// <param name="fullPath">是否以全路径形式返回</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<FsDirectoryInfoItem[]> FsGetDirectoryInfosSr(string[] directories,
                FsGetDirectoryInfosOption option, bool fullPath = false, CallingSettings settings = null)
            {
                var sr = TrayService.FsGetDirectoryInfos(Sc,
                    new Tray_FsGetDirectoryInfos_Request() { Directories = directories, Option = option, FullPath = fullPath }, settings);

                return CreateSr(sr, r => r.Items);
            }

            /// <summary>
            /// 获取指定目录的子目录及文件信息
            /// </summary>
            /// <param name="directories">目录</param>
            /// <param name="option">选项</param>
            /// <param name="fullPath">是否以全路径形式返回</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public FsDirectoryInfoItem[] FsGetDirectoryInfos(string[] directories,
                FsGetDirectoryInfosOption option, bool fullPath = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(FsGetDirectoryInfosSr(directories, option, fullPath, settings));
            }

            /// <summary>
            /// 获取指定目录的子目录及文件信息
            /// </summary>
            /// <param name="directory">目录</param>
            /// <param name="option">选项</param>
            /// <param name="fullPath">是否以全路径形式返回</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<FsDirectoryInfoItem> FsGetDirectoryInfoSr(string directory,
                FsGetDirectoryInfosOption option, bool fullPath = false, CallingSettings settings = null)
            {
                var sr = FsGetDirectoryInfosSr(new[] { directory }, option, fullPath, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取指定目录的子目录及文件信息
            /// </summary>
            /// <param name="directory">目录</param>
            /// <param name="option">选项</param>
            /// <param name="fullPath">是否以全路径形式返回</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public FsDirectoryInfoItem FsGetDirectoryInfo(string directory,
                FsGetDirectoryInfosOption option, bool fullPath = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(FsGetDirectoryInfoSr(directory, option, fullPath, settings));
            }

            /// <summary>
            /// 获取指定目录的子目录及文件信息
            /// </summary>
            /// <param name="directories">目录</param>
            /// <param name="option">选项</param>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="fullPath">是否以全路径形式返回</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<FsDirectoryInfoItem[]> FsGetDirectoryInfosSr(string[] directories,
                FsGetDirectoryInfosOption option, bool fullPath, Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid));

                var sr = TrayService.FsGetDirectoryInfos(Sc,
                    new Tray_FsGetDirectoryInfos_Request() { Directories = directories, Option = option, FullPath = fullPath },
                    CallingSettings.FromTarget(clientId, sid: sid));

                return CreateSr(sr, r => r.Items);
            }

            /// <summary>
            /// 获取指定目录的子目录及文件信息
            /// </summary>
            /// <param name="directories">目录</param>
            /// <param name="option">选项</param>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="fullPath">是否以全路径形式返回</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public FsDirectoryInfoItem[] FsGetDirectoryInfos(string[] directories,
                FsGetDirectoryInfosOption option, bool fullPath, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(FsGetDirectoryInfosSr(directories, option, fullPath, clientId, sid));
            }

            /// <summary>
            /// 获取指定目录的子目录及文件信息
            /// </summary>
            /// <param name="directory">目录</param>
            /// <param name="option">选项</param>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="fullPath">是否以全路径形式返回</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<FsDirectoryInfoItem> FsGetDirectoryInfoSr(string directory,
                FsGetDirectoryInfosOption option, bool fullPath, Guid clientId, Sid sid = default(Sid))
            {
                var sr = FsGetDirectoryInfosSr(new[] { directory }, option, fullPath, clientId, sid);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取指定目录的子目录及文件信息
            /// </summary>
            /// <param name="directory">目录</param>
            /// <param name="option">选项</param>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="fullPath">是否以全路径形式返回</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public FsDirectoryInfoItem FsGetDirectoryInfo(string directory,
                FsGetDirectoryInfosOption option, bool fullPath, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(FsGetDirectoryInfoSr(directory, option, fullPath, clientId, sid));
            }

            /// <summary>
            /// 下载文件系统中的文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="fileInfo">文件信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<byte[]> FsDownloadFileSr(string path, out FsFileInfo fileInfo, CallingSettings settings = null)
            {
                var sr = TrayService.FsDownloadFile(Sc,
                    new Tray_FsDownloadFile_Request() { Path = path }, settings);

                fileInfo = (sr != null && sr.Result != null) ? sr.Result.Info : null;
                return CreateSr(sr, r => r.Content);
            }

            /// <summary>
            /// 下载文件系统中的文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="fileInfo">文件信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public byte[] FsDownloadFile(string path, out FsFileInfo fileInfo, CallingSettings settings = null)
            {
                return InvokeWithCheck(FsDownloadFileSr(path, out fileInfo, settings));
            }

            /// <summary>
            /// 下载文件系统中的文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="fileInfo">文件信息</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<byte[]> FsDownloadFileSr(string path, out FsFileInfo fileInfo, Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid));
                return FsDownloadFileSr(path, out fileInfo, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 下载文件系统中的文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="fileInfo">文件信息</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public byte[] FsDownloadFile(string path, out FsFileInfo fileInfo, Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid));
                return InvokeWithCheck(FsDownloadFileSr(path, out fileInfo, clientId, sid));
            }

            /// <summary>
            /// 下载文件系统中的文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<byte[]> FsDownloadFileSr(string path, CallingSettings settings = null)
            {
                FsFileInfo fileInfo;
                return FsDownloadFileSr(path, out fileInfo, settings);
            }

            /// <summary>
            /// 下载文件系统中的文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public byte[] FsDownloadFile(string path, CallingSettings settings = null)
            {
                return InvokeWithCheck(FsDownloadFileSr(path, settings));
            }

            /// <summary>
            /// 下载文件系统中的文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<byte[]> FsDownloadFileSr(string path, Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid));
                return FsDownloadFileSr(path, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 下载文件系统中的文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public byte[] FsDownloadFile(string path, Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid));
                return InvokeWithCheck(FsDownloadFileSr(path, clientId, sid));
            }

            /// <summary>
            /// 删除文件系统的文件或目录
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult FsDeletePathSr(string[] paths, CallingSettings settings = null)
            {
                return TrayService.FsDeletePath(Sc,
                    new Tray_FsDeletePath_Request() { Paths = paths }, settings);
            }

            /// <summary>
            /// 删除文件系统的文件或目录
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="settings">调用设置</param>
            public void FsDeletePath(string[] paths, CallingSettings settings = null)
            {
                InvokeWithCheck(FsDeletePathSr(paths, settings));
            }

            /// <summary>
            /// 删除文件系统的文件或目录
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult FsDeletePathSr(string[] paths, Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid));
                return FsDeletePathSr(paths, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 删除文件系统的文件或目录
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            public void FsDeletePath(string[] paths, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(FsDeletePathSr(paths, clientId, sid));
            }

            /// <summary>
            /// 删除文件系统的文件或目录
            /// </summary>
            /// <param name="paths">路径</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult FsDeletePathSr(string path, CallingSettings settings = null)
            {
                return TrayService.FsDeletePath(Sc,
                    new Tray_FsDeletePath_Request() { Paths = new[] { path } }, settings);
            }

            /// <summary>
            /// 删除文件系统的文件或目录
            /// </summary>
            /// <param name="paths">路径</param>
            /// <param name="settings">调用设置</param>
            public void FsDeletePath(string path, CallingSettings settings = null)
            {
                InvokeWithCheck(FsDeletePathSr(new[] { path }, settings));
            }

            /// <summary>
            /// 删除文件系统的文件或目录
            /// </summary>
            /// <param name="paths">路径</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult FileSystemDeletePathSr(string path, Guid clientId, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid));
                return FsDeletePathSr(path, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 删除文件系统的文件或目录
            /// </summary>
            /// <param name="paths">路径</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            public void FsDeletePath(string path, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(FileSystemDeletePathSr(path, clientId, sid));
            }

            /// <summary>
            /// 获取进程列表
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ProcessInfo[]> GetProcessesSr(CallingSettings settings = null)
            {
                var sr = TrayService.GetProcesses(Sc, settings);
                return CreateSr(sr, r => r.ProcessInfos);
            }

            /// <summary>
            /// 获取进程列表
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ProcessInfo[] GetProcesses(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetProcessesSr(settings));
            }

            /// <summary>
            /// 获取进程列表
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<ProcessInfo[]> GetProcessesSr(Guid clientId, Sid sid = default(Sid))
            {
                CallingSettings settings = CallingSettings.FromTarget(clientId, sid: sid);
                return GetProcessesSr(settings);
            }

            /// <summary>
            /// 获取进程列表
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ProcessInfo[] GetProcesses(Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetProcessesSr(clientId, sid));
            }

            /// <summary>
            /// 批量结束进程
            /// </summary>
            /// <param name="processIds">进程ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult KillProcessesSr(int[] processIds, CallingSettings settings = null)
            {
                return TrayService.KillProcesses(Sc, new Tray_KillProcesses_Request { ProcessIds = processIds }, settings);
            }
            
            /// <summary>
            /// 批量结束进程
            /// </summary>
            /// <param name="processIds">进程ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void KillProcesses(int[] processIds, CallingSettings settings = null)
            {
                InvokeWithCheck(KillProcessesSr(processIds, settings));
            }

            /// <summary>
            /// 批量结束进程
            /// </summary>
            /// <param name="processIds">进程ID</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult KillProcessesSr(int[] processIds, Guid clientId, Sid sid = default(Sid))
            {
                return KillProcessesSr(processIds, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 批量结束进程
            /// </summary>
            /// <param name="processIds">进程ID</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public void KillProcesses(int[] processIds, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(KillProcessesSr(processIds, clientId, sid));
            }

            /// <summary>
            /// 批量结束进程
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult KillProcessSr(int processId, CallingSettings settings = null)
            {
                return KillProcessesSr(new[] { processId }, settings);
            }

            /// <summary>
            /// 批量结束进程
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void KillProcess(int processId, CallingSettings settings = null)
            {
                InvokeWithCheck(KillProcessSr(processId, settings));
            }

            /// <summary>
            /// 批量结束进程
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult KillProcessSr(int processId, Guid clientId, Sid sid = default(Sid))
            {
                return KillProcessSr(processId, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 批量结束进程
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public void KillProcess(int processId, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(KillProcessSr(processId, clientId, sid));
            }

            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="startInfos">进程启动信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult StartProcessesSr(ProcessStartInfo[] startInfos, CallingSettings settings = null)
            {
                var req = new Tray_StartProcesses_Request { StartInfos = startInfos };
                return TrayService.StartProcesses(Sc, req, settings);
            }

            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="startInfos">进程启动信息</param>
            /// <param name="settings">调用设置</param>
            public void StartProcesses(ProcessStartInfo[] startInfos, CallingSettings settings = null)
            {
                InvokeWithCheck(StartProcessesSr(startInfos, settings));
            }

            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="startInfos">进程启动信息</param>
            /// <param name="clientId">服务终端</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult StartProcessesSr(ProcessStartInfo[] startInfos, Guid clientId, Sid sid = default(Sid))
            {
                return StartProcessesSr(startInfos, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="startInfos">进程启动信息</param>
            /// <param name="clientId">服务终端</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public void StartProcesses(ProcessStartInfo[] startInfos, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(StartProcessesSr(startInfos, clientId, sid));
            }

            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="startInfo">进程启动信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult StartProcessSr(ProcessStartInfo startInfo, CallingSettings settings = null)
            {
                Contract.Requires(startInfo != null);
                return StartProcessesSr(new[] { startInfo }, settings);
            }

            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="startInfo">进程启动信息</param>
            /// <param name="settings">调用设置</param>
            public void StartProcess(ProcessStartInfo startInfo, CallingSettings settings = null)
            {
                InvokeWithCheck(StartProcessSr(startInfo, settings));
            }

            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="startInfo">进程启动信息</param>
            /// <param name="clientId">服务终端</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult StartProcessSr(ProcessStartInfo startInfo, Guid clientId, Sid sid = default(Sid))
            {
                return StartProcessSr(startInfo, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="startInfo">进程启动信息</param>
            /// <param name="clientId">服务终端</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public void StartProcess(ProcessStartInfo startInfo, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(StartProcessSr(startInfo, clientId, sid));
            }

            /// <summary>
            /// 获取线程列表
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ThreadInfoCollection[]> GetThreadsSr(int[] processIds, CallingSettings settings = null)
            {
                var sr = TrayService.GetThreads(Sc, new Tray_GetThreads_Request() { ProcessIds = processIds }, settings);
                return CreateSr(sr, r => r.ThreadInfoCollections);
            }

            /// <summary>
            /// 获取线程列表
            /// </summary>
            /// <param name="processIds">进程ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ThreadInfoCollection[] GetThreads(int[] processIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetThreadsSr(processIds, settings));
            }

            /// <summary>
            /// 获取线程列表
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<ThreadInfoCollection> GetThreadsSr(int processId, CallingSettings settings = null)
            {
                var sr = GetThreadsSr(new[] { processId }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取线程列表
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ThreadInfoCollection GetThreads(int processId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetThreadsSr(processId, settings));
            }

            /// <summary>
            /// 获取线程列表
            /// </summary>
            /// <param name="processIds">进程ID</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<ThreadInfoCollection[]> GetThreadsSr(int[] processIds, Guid clientId, Sid sid = default(Sid))
            {
                return GetThreadsSr(processIds, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取线程列表
            /// </summary>
            /// <param name="processIds">进程ID</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ThreadInfoCollection[] GetThreads(int[] processIds, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetThreadsSr(processIds, clientId, sid));
            }

            /// <summary>
            /// 获取线程列表
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<ThreadInfoCollection> GetThreadsSr(int processId, Guid clientId, Sid sid = default(Sid))
            {
                return GetThreadsSr(processId, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取线程列表
            /// </summary>
            /// <param name="processId">进程ID</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ThreadInfoCollection GetThreads(int processId, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetThreadsSr(processId, clientId, sid));
            }

            /// <summary>
            /// 获取系统属性
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<SystemProperty[]> GetSystemPropertiesSr(string[] names, CallingSettings settings = null)
            {
                Tray_GetSystemProperties_Request req = new Tray_GetSystemProperties_Request() { Names = names };
                var sr = TrayService.GetSystemProperties(Sc, req, settings);
                return CreateSr(sr, r => r.Properties);
            }

            /// <summary>
            /// 获取系统属性
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public SystemProperty[] GetSystemProperties(string[] names, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetSystemPropertiesSr(names, settings));
            }

            /// <summary>
            /// 获取系统属性
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<SystemProperty[]> GetSystemPropertiesSr(string[] names, Guid clientId, Sid sid = default(Sid))
            {
                return GetSystemPropertiesSr(names, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取系统属性
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public SystemProperty[] GetSystemProperties(string[] names, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetSystemPropertiesSr(names, clientId, sid));
            }

            /// <summary>
            /// 获取全部系统属性
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<SystemProperty[]> GetAllSystemPropertiesSr(CallingSettings settings = null)
            {
                return GetSystemPropertiesSr(null, settings);
            }

            /// <summary>
            /// 获取全部系统属性
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public SystemProperty[] GetAllSystemProperties(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllSystemPropertiesSr(settings));
            }

            /// <summary>
            /// 获取全部系统属性
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<SystemProperty[]> GetAllSystemPropertiesSr(Guid clientId, Sid sid = default(Sid))
            {
                return GetAllSystemPropertiesSr(CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 获取全部系统属性
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public SystemProperty[] GetAllSystemProperties(Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAllSystemPropertiesSr(clientId, sid));
            }

            /// <summary>
            /// 读取环境变量
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<SystemEnvironmentVariable[]> GetSystemEnvironmentVariablesSr(string[] names, CallingSettings settings = null)
            {
                Tray_GetSystemEnvironmentVariables_Request req = new Tray_GetSystemEnvironmentVariables_Request { Names = names };
                var sr = TrayService.GetSystemEnvironmentVariables(Sc, req, settings);
                return CreateSr(sr, r => r.Variables);
            }

            /// <summary>
            /// 读取环境变量
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public SystemEnvironmentVariable[] GetSystemEnvironmentVariables(string[] names, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetSystemEnvironmentVariablesSr(names, settings));
            }

            /// <summary>
            /// 读取全部环境变量
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<SystemEnvironmentVariable[]> GetAllSystemEnvironmentVariablesSr(CallingSettings settings = null)
            {
                return GetSystemEnvironmentVariablesSr(null, settings);
            }

            /// <summary>
            /// 读取全部环境变量
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public SystemEnvironmentVariable[] GetAllSystemEnvironmentVariables(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllSystemEnvironmentVariablesSr(settings));
            }

            /// <summary>
            /// 读取环境变量
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<SystemEnvironmentVariable[]> GetSystemEnvironmentVariablesSr(string[] names, Guid clientId, Sid sid = default(Sid))
            {
                return GetSystemEnvironmentVariablesSr(names, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 读取环境变量
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public SystemEnvironmentVariable[] GetSystemEnvironmentVariables(string[] names, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetSystemEnvironmentVariablesSr(names, clientId, sid));
            }

            /// <summary>
            /// 读取全部环境变量
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<SystemEnvironmentVariable[]> GetAllSystemEnvironmentVariablesSr(Guid clientId, Sid sid = default(Sid))
            {
                return GetSystemEnvironmentVariablesSr(null, clientId, sid);
            }

            /// <summary>
            /// 读取全部环境变量
            /// </summary>
            /// <param name="names">名称</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public SystemEnvironmentVariable[] GetAllSystemEnvironmentVariables(Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAllSystemEnvironmentVariablesSr(clientId, sid));
            }

            /// <summary>
            /// 设置环境变量
            /// </summary>
            /// <param name="variables">环境变量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetSystemEnvironmentVariablesSr(SystemEnvironmentVariable[] variables, CallingSettings settings = null)
            {
                var req = new Tray_SetSystemEnvironmentVariables_Request { Variables = variables };
                return TrayService.SetSystemEnvironmentVariables(Sc, req, settings);
            }

            /// <summary>
            /// 设置环境变量
            /// </summary>
            /// <param name="variables">环境变量</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void SetSystemEnvironmentVariables(SystemEnvironmentVariable[] variables, CallingSettings settings = null)
            {
                InvokeWithCheck(SetSystemEnvironmentVariablesSr(variables, settings));
            }

            /// <summary>
            /// 设置环境变量
            /// </summary>
            /// <param name="variables">环境变量</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult SetSystemEnvironmentVariablesSr(SystemEnvironmentVariable[] variables, Guid clientId, Sid sid = default(Sid))
            {
                return SetSystemEnvironmentVariablesSr(variables, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 设置环境变量
            /// </summary>
            /// <param name="variables">环境变量</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public void SetSystemEnvironmentVariables(SystemEnvironmentVariable[] variables, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(SetSystemEnvironmentVariablesSr(variables, clientId, sid));
            }

            /// <summary>
            /// 设置环境变量
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">值</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetSystemEnvironmentVariablesSr(string name, string value, CallingSettings settings = null)
            {
                return SetSystemEnvironmentVariablesSr(new[] { new SystemEnvironmentVariable { Name = name, Value = value } }, settings);
            }

            /// <summary>
            /// 设置环境变量
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">值</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void SetSystemEnvironmentVariables(string name, string value, CallingSettings settings = null)
            {
                InvokeWithCheck(SetSystemEnvironmentVariablesSr(name, value, settings));
            }

            /// <summary>
            /// 设置环境变量
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">值</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult SetSystemEnvironmentVariablesSr(string name, string value, Guid clientId, Sid sid = default(Sid))
            {
                return SetSystemEnvironmentVariablesSr(name, value, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 设置环境变量
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">值</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public void SetSystemEnvironmentVariables(string name, string value, Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(SetSystemEnvironmentVariablesSr(name, value, clientId, sid));
            }
        }
	}
}
