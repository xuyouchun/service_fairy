using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.File.UnionFile;
using ServiceFairy.Entities.File;
using Common.Contracts.Service;
using System.IO;
using Common.Utility;
using Common.Package;
using Common.Package.Storage;
using Common;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 对文件操作的封装
    /// </summary>
    public class FileClient
    {
        public FileClient(IServiceClient serviceClient)
            : this(SystemInvoker.FromServiceClient(serviceClient))
        {

        }

        public FileClient(SystemInvoker invoker)
        {
            Contract.Requires(invoker != null);
            _invoker = invoker;
        }

        private readonly SystemInvoker _invoker;

        /// <summary>
        /// 创建指定路径的调用设置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public CallingSettings CreateCallingSettings(string path)
        {
            Contract.Requires(path != null);

            TargetContext ctx = _targetCache.GetOrAddOfRelative(path.ToLower(), TimeSpan.FromSeconds(30), (key) =>
                new TargetContext() { ClientID = _invoker.File.GetRouteInfo(path).ClientID });

            return CallingSettings.FromTarget(ctx.ClientID);
        }

        class TargetContext
        {
            public Guid ClientID { get; set; }
        }

        private readonly Cache<string, TargetContext> _targetCache = new Cache<string, TargetContext>();

        /// <summary>
        /// 下载指定文件的全部字节流
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public byte[] DownloadAllBytes(string path, int maxSize = 1 * 1024 * 1024)
        {
            Contract.Requires(path != null);

            CallingSettings settings = CreateCallingSettings(path);

            UnionFileInfo fileInfo;
            string token;
            bool atEnd;

            byte[] buffer = _invoker.File.DownloadDirect(path, maxSize, out fileInfo, out token, out atEnd, settings);
            if (atEnd)
                return buffer;

            MemoryStream ms = new MemoryStream();
            ms.Write(buffer);

            do
            {
                buffer = _invoker.File.Download(token, maxSize, out atEnd, settings);
                ms.Write(buffer);

            } while (!atEnd);

            return ms.ToArray();
        }

        const int _bufferLength = 1024 * 1024;
        private static readonly ObjectPool<byte[]> _pool = new ObjectPool<byte[]>(() => new byte[_bufferLength], 10);

        /// <summary>
        /// 上传全部字节流
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        /// <param name="maxSize"></param>
        public void UploadAllBytes(string path, byte[] bytes, int maxSize = _bufferLength)
        {
            Contract.Requires(path != null && bytes != null);

            CallingSettings settings = CreateCallingSettings(path);

            if (bytes.Length <= maxSize)
            {
                _invoker.File.UploadAll(path, bytes, settings);
            }
            else
            {
                byte[] buffer;
                IDisposable dis = null;
                if (maxSize == _bufferLength)
                    dis = _pool.Accquire(out buffer);
                else
                    buffer = new byte[maxSize];

                int pos = 0;
                string token = null;
                bool atEnd;

                try
                {
                    do
                    {
                        if (bytes.Length - pos >= buffer.Length)
                        {
                            BufferUtility.Copy(buffer, bytes, 0, pos, buffer.Length);
                            pos += buffer.Length;
                            atEnd = (pos >= bytes.Length);
                        }
                        else
                        {
                            buffer = new byte[bytes.Length - pos];
                            BufferUtility.Copy(buffer, bytes, 0, pos, buffer.Length);
                            atEnd = true;
                        }

                        if (token == null)
                            token = _invoker.File.UploadDirect(path, buffer, atEnd, settings);
                        else
                            _invoker.File.Upload(token, buffer, atEnd, settings);

                    } while (!atEnd);
                }
                finally
                {
                    if (dis != null)
                        dis.Dispose();
                }
            }
        }

        /// <summary>
        /// 上传StreamTable
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public FileClientUploadStreamTableHandle UploadStreamTable(string path, string name, string desc = "")
        {
            CallingSettings settings = CreateCallingSettings(path);
            string token = _invoker.File.BeginUploadStreamTable(path, name, desc, settings: settings);
            return new FileClientUploadStreamTableHandle(_invoker, token, settings);
        }

        /// <summary>
        /// 下载StreamTable
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileClientDownloadStreamTableHandle DownloadStreamTable(string path)
        {
            CallingSettings settings = CreateCallingSettings(path);
            StreamTableBasicInfo basicInfo;
            UnionFileInfo fileInfo;
            string token = _invoker.File.BeginDownloadStreamTable(path, out basicInfo, out fileInfo, settings);
            return new FileClientDownloadStreamTableHandle(_invoker, token, basicInfo, fileInfo, settings);
        }
    }

    #region Class FileClientUploadStreamTableHandle ...

    /// <summary>
    /// 上传StreamTable文件操作的句柄
    /// </summary>
    public class FileClientUploadStreamTableHandle : IDisposable
    {
        internal FileClientUploadStreamTableHandle(SystemInvoker invoker, string token, CallingSettings settings)
        {
            _invoker = invoker;
            _token = token;
            _settings = settings;
        }

        private readonly SystemInvoker _invoker;
        private readonly string _token;
        private readonly CallingSettings _settings;
        private bool _atEnd;

        private NewStreamTableInfo _stInfo;

        /// <summary>
        /// 创建一个表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columns"></param>
        /// <param name="desc"></param>
        public void CreateTable(string name, StreamTableColumn[] columns, string desc = "")
        {
            Contract.Requires(name != null && columns != null);

            if (_stInfo != null || _rows.Count > 0)
            {
                Upload(false);
            }

            _stInfo = new NewStreamTableInfo() { Name = name, Columns = columns, Desc = desc };
        }

        private readonly List<object[]> _rows = new List<object[]>();

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="atEnd"></param>
        public void AppendRows(IEnumerable<string[]> rows)
        {
            Contract.Requires(rows != null);

            _rows.AddRange(rows);
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="row"></param>
        public void AppendRow(object[] row)
        {
            Contract.Requires(row != null);

            _rows.Add(row);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="atEnd"></param>
        public void Upload(bool atEnd = false)
        {
            if (_rows.Count == 0 && _stInfo == null && !atEnd)
                return;

            _invoker.File.UploadStreamTable(_token,
                _rows.ToArray(r => new StreamTableRowData() { Datas = r.ToArray(d => d.ToStringIgnoreNull()) })
                , atEnd, _stInfo, _settings);

            _atEnd = atEnd;
            _stInfo = null;
            _rows.Clear();
        }

        public void Dispose()
        {
            if (!_atEnd || _stInfo != null || _rows.Count > 0)
                Upload(true);
        }
    }

    #endregion

    #region Class FileClientDownloadStreamTableHandle ...

    public class FileClientDownloadStreamTableHandle : IDisposable
    {
        internal FileClientDownloadStreamTableHandle(SystemInvoker invoker, string token, 
            StreamTableBasicInfo basicInfo, UnionFileInfo fileInfo, CallingSettings settings)
        {
            _invoker = invoker;
            _token = token;
            _basicInfo = basicInfo;
            _settings = settings;
            _fileInfo = fileInfo;
        }

        private readonly SystemInvoker _invoker;
        private readonly string _token;
        private readonly StreamTableBasicInfo _basicInfo;
        private readonly UnionFileInfo _fileInfo;
        private readonly CallingSettings _settings;

        /// <summary>
        /// 基础信息
        /// </summary>
        public StreamTableBasicInfo BasicInfo
        {
            get { return _basicInfo; }
        }

        /// <summary>
        /// 文件信息
        /// </summary>
        public UnionFileInfo FileInfo
        {
            get { return _fileInfo; }
        }

        /// <summary>
        /// 获取指定表的指定范围的行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public object[][] GetRowDatas(string table, int start = 0, int count = int.MaxValue)
        {
            StreamTableRowData[] rows = _invoker.File.DownloadStreamTable(_token, table, start, count, _settings);
            return rows.ToArray(row => row.Datas);
        }

        /// <summary>
        /// 获取指定索引的行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public object[] GetRowData(string table, int index)
        {
            return GetRowDatas(table, index, 1).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _invoker.File.End(_token, _settings);
        }
    }

    #endregion

}
