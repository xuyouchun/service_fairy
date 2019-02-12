using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.File.UnionFile;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private FileInvoker _file;

        /// <summary>
        /// File Service
        /// </summary>
        public FileInvoker File
        {
            get { return _file ?? (_file = new FileInvoker(this)); }
        }


        /// <summary>
        /// File Service
        /// </summary>
        public class FileInvoker : Invoker
        {
            public FileInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 获取路径的路由信息
            /// </summary>
            /// <param name="paths"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<FileRouteInfo[]> GetRouteInfosSr(string[] paths, CallingSettings settings = null)
            {
                var sr = FileService.GetRouteInfo(Sc, new File_GetRouteInfo_Request() { Paths = paths }, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取路径的路由信息
            /// </summary>
            /// <param name="paths"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public FileRouteInfo[] GetRouteInfos(string[] paths, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetRouteInfosSr(paths, settings));
            }

            /// <summary>
            /// 获取路径的路由信息
            /// </summary>
            /// <param name="path"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<FileRouteInfo> GetRouteInfoSr(string path, CallingSettings settings = null)
            {
                var sr = GetRouteInfosSr(new[] { path }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取路径的路由信息
            /// </summary>
            /// <param name="path"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public FileRouteInfo GetRouteInfo(string path, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetRouteInfoSr(path, settings));
            }

            /// <summary>
            /// 开始上传文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<string> BeginUploadSr(string path, CallingSettings settings = null)
            {
                var sr = FileService.BeginUpload(Sc, new File_BeginUpload_Request() { Path = path }, settings);
                return CreateSr(sr, r => r.Token);
            }

            /// <summary>
            /// 开始上传文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public string BeginUpload(string path, CallingSettings settings = null)
            {
                return InvokeWithCheck(BeginUploadSr(path, settings));
            }

            /// <summary>
            /// 上传文件
            /// </summary>
            /// <param name="token"></param>
            /// <param name="buffer"></param>
            /// <param name="atEnd"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult UploadSr(string token, byte[] buffer, bool atEnd = false, CallingSettings settings = null)
            {
                return FileService.Upload(Sc,
                    new File_Upload_Request() { Buffer = buffer, Token = token, AtEnd = atEnd }, settings);
            }

            /// <summary>
            /// 上传文件
            /// </summary>
            /// <param name="token"></param>
            /// <param name="buffer"></param>
            /// <param name="atEnd"></param>
            /// <param name="settings"></param>
            public void Upload(string token, byte[] buffer, bool atEnd = false, CallingSettings settings = null)
            {
                InvokeWithCheck(UploadSr(token, buffer, atEnd, settings));
            }

            /// <summary>
            /// 一次上传全部文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="buffer"></param>
            /// <param name="settings"></param>
            public ServiceResult UploadAllSr(string path, byte[] buffer, CallingSettings settings = null)
            {
                return FileService.UploadAll(Sc, new File_UploadAll_Request() { Path = path, Buffer = buffer }, settings);
            }

            /// <summary>
            /// 一次上传全部文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="buffer"></param>
            /// <param name="settings"></param>
            public void UploadAll(string path, byte[] buffer, CallingSettings settings = null)
            {
                InvokeWithCheck(UploadAllSr(path, buffer, settings));
            }

            /// <summary>
            /// 开始下载文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="fileInfo"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<string> BeginDownloadSr(string path, out UnionFileInfo fileInfo, CallingSettings settings = null)
            {
                var sr = FileService.BeginDownload(Sc, new File_BeginDownload_Request() { Path = path }, settings);
                fileInfo = (sr != null && sr.Result != null) ? sr.Result.FileInfo : null;
                return CreateSr(sr, r => r.Token);
            }

            /// <summary>
            /// 开始下载文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="fileInfo"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public string BeginDownload(string path, out UnionFileInfo fileInfo, CallingSettings settings = null)
            {
                return InvokeWithCheck(BeginDownloadSr(path, out fileInfo, settings));
            }

            /// <summary>
            /// 下载文件
            /// </summary>
            /// <param name="token"></param>
            /// <param name="maxSize"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<byte[]> DownloadSr(string token, int maxSize, CallingSettings settings = null)
            {
                var sr = FileService.Download(Sc, new File_Download_Request() { Token = token, MaxSize = maxSize }, settings);
                return CreateSr(sr, r => r.Buffer);
            }

            /// <summary>
            /// 下载文件
            /// </summary>
            /// <param name="token"></param>
            /// <param name="maxSize"></param>
            /// <param name="atEnd"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public byte[] Download(string token, int maxSize, out bool atEnd, CallingSettings settings = null)
            {
                var sr = DownloadSr(token, maxSize, settings);
                atEnd = (sr.StatusCode == (int)ServiceStatusCode.Ok);
                return InvokeWithCheck(sr);
            }

            /// <summary>
            /// 一次下载全部文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="fileInfo"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<byte[]> DownloadAllSr(string path, out UnionFileInfo fileInfo, CallingSettings settings = null)
            {
                var sr = FileService.DownloadAll(Sc, new File_DownloadAll_Request() { Path = path }, settings);
                fileInfo = (sr != null && sr.Result != null) ? sr.Result.FileInfo : null;
                return CreateSr(sr, r => r.Buffer);
            }

            /// <summary>
            /// 一次下载全部文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="fileInfo"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public byte[] DownloadAll(string path, out UnionFileInfo fileInfo, CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadAllSr(path, out fileInfo, settings));
            }

            /// <summary>
            /// 直接下载文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="maxSize"></param>
            /// <param name="fileInfo"></param>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<byte[]> DownloadDirectSr(string path, int maxSize, out UnionFileInfo fileInfo, out string token, CallingSettings settings = null)
            {
                var sr = FileService.DownloadDirect(Sc, new File_DownloadDirect_Request() { MaxSize = maxSize, Path = path },
                    settings);

                if (sr != null && sr.Result != null)
                {
                    fileInfo = sr.Result.FileInfo;
                    token = sr.Result.Token;
                }
                else
                {
                    fileInfo = null;
                    token = null;
                }

                return CreateSr(sr, r => r.Bytes);
            }

            /// <summary>
            /// 直接下载文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="maxSize"></param>
            /// <param name="fileInfo"></param>
            /// <param name="token"></param>
            /// <param name="atEnd"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public byte[] DownloadDirect(string path, int maxSize, out UnionFileInfo fileInfo, out string token, out bool atEnd, CallingSettings settings = null)
            {
                var sr = DownloadDirectSr(path, maxSize, out fileInfo, out token, settings);
                atEnd = (sr.StatusCode == (int)ServiceStatusCode.Ok);
                return InvokeWithCheck(sr);
            }

            /// <summary>
            /// 直接上传文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="buffer"></param>
            /// <param name="atEnd"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<string> UploadDirectSr(string path, byte[] buffer, bool atEnd, CallingSettings settings = null)
            {
                var sr = FileService.UploadDirect(Sc, new File_UploadDirect_Request() { AtEnd = atEnd, Path = path, Buffer = buffer }, settings);
                return CreateSr(sr, r => r.Token);
            }

            /// <summary>
            /// 直接上传文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="buffer"></param>
            /// <param name="atEnd"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public string UploadDirect(string path, byte[] buffer, bool atEnd, CallingSettings settings = null)
            {
                return InvokeWithCheck(UploadDirectSr(path, buffer, atEnd, settings));
            }

            /// <summary>
            /// 删除文件
            /// </summary>
            /// <param name="paths"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult DeleteSr(string[] paths, CallingSettings settings = null)
            {
                return FileService.Delete(Sc, new File_Delete_Request() { Paths = paths }, settings);
            }

            /// <summary>
            /// 删除文件
            /// </summary>
            /// <param name="paths"></param>
            /// <param name="settings"></param>
            public void Delete(string[] paths, CallingSettings settings = null)
            {
                InvokeWithCheck(DeleteSr(paths, settings));
            }

            /// <summary>
            /// 删除文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult DeleteSr(string path, CallingSettings settings = null)
            {
                return DeleteSr(new[] { path }, settings);
            }

            /// <summary>
            /// 删除文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="settings"></param>
            public void Delete(string path, CallingSettings settings = null)
            {
                InvokeWithCheck(DeleteSr(path, settings));
            }

            /// <summary>
            /// 开始上传StreamTable
            /// </summary>
            /// <param name="path"></param>
            /// <param name="name"></param>
            /// <param name="desc"></param>
            /// <param name="tableInfo"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<string> BeginUploadStreamTableSr(string path, string name = "", string desc = "",
                NewStreamTableInfo tableInfo = null, CallingSettings settings = null)
            {
                var sr = FileService.BeginUploadStreamTable(Sc,
                    new File_BeginUploadStreamTable_Request() { Path = path, Name = name, Desc = desc, TableInfo = tableInfo }, settings);

                return CreateSr(sr, r => r.Token);
            }

            /// <summary>
            /// 开始上传StreamTable
            /// </summary>
            /// <param name="path"></param>
            /// <param name="name"></param>
            /// <param name="desc"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public string BeginUploadStreamTable(string path, string name = "", string desc = "",
                NewStreamTableInfo tableInfo = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(BeginUploadStreamTableSr(path, name, desc, tableInfo, settings));
            }

            /// <summary>
            /// 上传StreamTable数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="rows"></param>
            /// <param name="tableInfo"></param>
            /// <param name="atEnd"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult UploadStreamTableSr(string token, StreamTableRowData[] rows, bool atEnd = false,
                NewStreamTableInfo tableInfo = null, CallingSettings settings = null)
            {
                return FileService.UploadStreamTable(Sc,
                    new File_UploadStreamTable_Request() { Token = token, Rows = rows, AtEnd = atEnd, TableInfo = tableInfo }, settings);
            }

            /// <summary>
            /// 上传StreamTable数据
            /// </summary>
            /// <param name="token"></param>
            /// <param name="rows"></param>
            /// <param name="atEnd"></param>
            /// <param name="tableInfo"></param>
            /// <param name="settings"></param>
            public void UploadStreamTable(string token, StreamTableRowData[] rows, bool atEnd = false,
                NewStreamTableInfo tableInfo = null, CallingSettings settings = null)
            {
                InvokeWithCheck(UploadStreamTableSr(token, rows, atEnd, tableInfo, settings));
            }

            /// <summary>
            /// 开始下载StreamTable
            /// </summary>
            /// <param name="path"></param>
            /// <param name="basicInfo"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<string> BeginDownloadStreamTableSr(string path, out StreamTableBasicInfo basicInfo, out UnionFileInfo fileInfo, CallingSettings settings = null)
            {
                var sr = FileService.BeginDownloadStreamTable(Sc, new File_BeginDownloadStreamTable_Request() { Path = path }, settings);
                basicInfo = (sr != null && sr.Result != null) ? sr.Result.BasicInfo : null;
                fileInfo = (sr != null && sr.Result != null) ? sr.Result.FileInfo : null;
                return CreateSr(sr, r => r.Token);
            }

            /// <summary>
            /// 开始下载StreamTable
            /// </summary>
            /// <param name="path"></param>
            /// <param name="basicInfo"></param>
            /// <param name="fileInfo"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public string BeginDownloadStreamTable(string path, out StreamTableBasicInfo basicInfo, out UnionFileInfo fileInfo, CallingSettings settings = null)
            {
                return InvokeWithCheck(BeginDownloadStreamTableSr(path, out basicInfo, out fileInfo, settings));
            }

            /// <summary>
            /// 下载StreamTable
            /// </summary>
            /// <param name="token"></param>
            /// <param name="count"></param>
            /// <param name="start"></param>
            /// <param name="table"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<StreamTableRowData[]> DownloadStreamTableSr(string token, string table, int start, int count, CallingSettings settings = null)
            {
                var sr = FileService.DownloadStreamTable(Sc,
                    new File_DownloadStreamTable_Request() { Token = token, Table = table, Start = start, Count = count }, settings);

                return CreateSr(sr, r => r.Rows);
            }

            /// <summary>
            /// 下载StreamTable
            /// </summary>
            /// <param name="token"></param>
            /// <param name="table"></param>
            /// <param name="start"></param>
            /// <param name="count"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public StreamTableRowData[] DownloadStreamTable(string token, string table, int start, int count, CallingSettings settings = null)
            {
                return InvokeWithCheck(DownloadStreamTableSr(token, table, start, count, settings));
            }

            /// <summary>
            /// 结束上传或下载
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult EndSr(string[] tokens, CallingSettings settings = null)
            {
                return FileService.End(Sc, new File_End_Request() { Tokens = tokens }, settings);
            }

            /// <summary>
            /// 结束上传或下载
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="settings"></param>
            public void End(string[] tokens, CallingSettings settings = null)
            {
                InvokeWithCheck(EndSr(tokens, settings));
            }

            /// <summary>
            /// 结束上传或下载
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult EndSr(string token, CallingSettings settings = null)
            {
                return EndSr(new[] { token }, settings);
            }

            /// <summary>
            /// 结束上传或下载
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            public void End(string token, CallingSettings settings = null)
            {
                InvokeWithCheck(EndSr(token, settings));
            }

            /// <summary>
            /// 取消上传或下载
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult CancelSr(string[] tokens, CallingSettings settings = null)
            {
                return FileService.Cancel(Sc, new File_Cancel_Request() { Tokens = tokens }, settings);
            }

            /// <summary>
            /// 取消上传或下载
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="settings"></param>
            public void Cancel(string[] tokens, CallingSettings settings = null)
            {
                InvokeWithCheck(CancelSr(tokens, settings));
            }

            /// <summary>
            /// 取消上传或下载
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult CancelSr(string token, CallingSettings settings = null)
            {
                return CancelSr(new[] { token }, settings);
            }

            /// <summary>
            /// 取消上传或下载
            /// </summary>
            /// <param name="token"></param>
            /// <param name="settings"></param>
            public void Cancel(string token, CallingSettings settings = null)
            {
                InvokeWithCheck(CancelSr(token, settings));
            }

            /// <summary>
            /// 获取指定目录的信息及子目录、文件信息
            /// </summary>
            /// <param name="paths"></param>
            /// <param name="option"></param>
            /// <param name="pattern"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<DirectoryInfoItem[]> GetDirectoryInfosSr(string[] paths, GetDirectoryInfosOption option = GetDirectoryInfosOption.None,
                string pattern = null, CallingSettings settings = null)
            {
                var sr = FileService.GetDirectoryInfos(Sc, new File_GetDirectoryInfos_Request() {
                    Option = option, Paths = paths, Pattern = pattern
                }, settings);

                return CreateSr(sr, r => r.Items);
            }

            /// <summary>
            /// 获取指定目录的信息及子目录、文件信息
            /// </summary>
            /// <param name="paths"></param>
            /// <param name="option"></param>
            /// <param name="pattern"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public DirectoryInfoItem[] GetDirectoryInfos(string[] paths, GetDirectoryInfosOption option = GetDirectoryInfosOption.None,
                string pattern = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetDirectoryInfosSr(paths, option, pattern, settings));
            }
            
            /// <summary>
            /// 获取指定目录的信息及子目录、文件信息
            /// </summary>
            /// <param name="path"></param>
            /// <param name="option"></param>
            /// <param name="pattern"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<DirectoryInfoItem> GetDirectoryInfoSr(string path, GetDirectoryInfosOption option = GetDirectoryInfosOption.None,
                string pattern = null, CallingSettings settings = null)
            {
                var sr = GetDirectoryInfosSr(new[] { path }, option, pattern, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取指定目录的信息及子目录、文件信息
            /// </summary>
            /// <param name="path"></param>
            /// <param name="option"></param>
            /// <param name="pattern"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public DirectoryInfoItem GetDirectoryInfo(string path, GetDirectoryInfosOption option = GetDirectoryInfosOption.None,
                string pattern = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetDirectoryInfoSr(path, option, pattern, settings));
            }

            /// <summary>
            /// 获取指定文件的信息
            /// </summary>
            /// <param name="paths"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<UnionFileInfo[]> GetFileInfosSr(string[] paths, CallingSettings settings = null)
            {
                var sr = FileService.GetFileInfos(Sc, new File_GetFileInfos_Request() { Paths = paths }, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取指定文件的信息
            /// </summary>
            /// <param name="paths"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public UnionFileInfo[] GetFileInfos(string[] paths, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFileInfosSr(paths, settings));
            }

            /// <summary>
            /// 获取指定文件的信息
            /// </summary>
            /// <param name="path"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<UnionFileInfo> GetFileInfoSr(string path, CallingSettings settings = null)
            {
                var sr = GetFileInfosSr(new[] { path }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取指定文件的信息
            /// </summary>
            /// <param name="path"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public UnionFileInfo GetFileInfo(string path, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFileInfoSr(path, settings));
            }
        }
    }
}
