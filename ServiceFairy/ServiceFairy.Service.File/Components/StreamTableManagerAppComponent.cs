using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Package.Storage;
using System.Diagnostics.Contracts;
using System.IO;
using ServiceFairy.Entities.File;
using Common.File.UnionFile;
using Common.Utility;

namespace ServiceFairy.Service.File.Components
{
    /// <summary>
    /// 上传或下载StreamTable
    /// </summary>
    [AppComponent("上传或下载StreamTable")]
    class StreamTableManagerAppComponent : AppComponent
    {
        public StreamTableManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
            _fs = service.FileSystemManager;
        }

        private readonly Service _service;
        private readonly FileSystemManagerAppComponent _fs;

        /// <summary>
        /// 开始上传
        /// </summary>
        /// <param name="path"></param>
        /// <param name="desc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string BeginUpload(string path, string name, string desc)
        {
            Contract.Requires(path != null);

            string token = _fs.BeginUpload(path);
            WriteFileToken ft = _fs.GetToken(token) as WriteFileToken;
            StreamTableWriter stw = new StreamTableWriter(name ?? "", desc ?? "");
            ft.Tag = new WritableStreamTableTag() { Writer = stw, FileToken = ft };

            return token;
        }

        #region Class WritableStreamTableTag ...

        class WritableStreamTableTag : IFileTokenTag
        {
            public FileToken FileToken { get; set; }

            public StreamTableWriter Writer { get; set; }
            public WritableStreamTable CurrentTable
            {
                get
                {
                    if (_currentTable == null)
                        throw Utility.CreateBusinessException(FileStatusCode.StreamTableNoTable);

                    return _currentTable;
                }
                set { _currentTable = value; }
            }

            private WritableStreamTable _currentTable;

            public void End()
            {
                Writer.WriteToStream(FileToken.Stream);
            }

            public void Dispose()
            {
                
            }

            public void Cancel()
            {
                
            }
        }

        #endregion

        private TTag _GetTag<TTag>(string token, FileStatusCode errorReason) where TTag : class
        {
            FileToken ft = _fs.GetToken(token);
            TTag tag = ft.Tag as TTag;
            if (tag == null)
                throw Utility.CreateBusinessException(errorReason);

            return tag;
        }

        /// <summary>
        /// 创建新表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="info"></param>
        public void CreateNewTable(string token, NewStreamTableInfo info)
        {
            Contract.Requires(token != null && info != null);

            WritableStreamTableTag tag = _GetTag<WritableStreamTableTag>(token, FileStatusCode.WriteNotSupported);
            tag.CurrentTable = tag.Writer.CreateTable(info.Name, info.Columns, info.Desc);
        }

        /// <summary>
        /// 上传StreamTable
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="token"></param>
        public void Upload(string token, StreamTableRowData[] rows)
        {
            Contract.Requires(token != null && rows != null);

            WritableStreamTableTag tag = _GetTag<WritableStreamTableTag>(token, FileStatusCode.WriteNotSupported);
            tag.CurrentTable.AppendRows(rows.Select(r => r.Datas).ToArray());
        }

        /// <summary>
        /// 结束上传StreamTable
        /// </summary>
        /// <param name="token"></param>
        public void EndUpload(string token)
        {
            WriteFileToken ft = _fs.GetToken(token) as WriteFileToken;
            _fs.End(token);
        }

        #region Class ReadableStreamTableTag ...

        /// <summary>
        /// 支持读取的StreamTable上下文
        /// </summary>
        class ReadableStreamTableTag : IFileTokenTag
        {
            /// <summary>
            /// 事务标识
            /// </summary>
            public ReadFileToken Token { get; set; }

            private StreamTableBasicInfo _basicInfo;

            /// <summary>
            /// 表的基础信息
            /// </summary>
            public StreamTableBasicInfo BasicInfo
            {
                get { return _basicInfo ?? (_basicInfo = _ReadBasicInfo()); }
            }

            private StreamTableBasicInfo _ReadBasicInfo()
            {
                List<StreamTableBasicTableInfo> infos = new List<StreamTableBasicTableInfo>();

                foreach (StreamTable st in TableReader.GetAllTables())
                {
                    infos.Add(new StreamTableBasicTableInfo() {
                        Name = st.Name, Desc = st.Desc, RowCount = st.RowCount, Columns = st.Columns
                    });
                }

                return new StreamTableBasicInfo() { TableInfos = infos.ToArray() };
            }

            private StreamTableReader _tableReader;

            /// <summary>
            /// 读的读取器
            /// </summary>
            public StreamTableReader TableReader
            {
                get
                {
                    if (_tableReader == null)
                        _tableReader = new StreamTableReader(Token.Stream);

                    return _tableReader;
                }
            }

            public void End()
            {
                
            }

            public void Dispose()
            {
                
            }


            public void Cancel()
            {
                
            }
        }

        #endregion

        /// <summary>
        /// 开始下载StreamTable
        /// </summary>
        /// <param name="path"></param>
        /// <param name="basicInfo"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public string BeginDownload(string path, out StreamTableBasicInfo basicInfo, out UnionFileInfo fileInfo)
        {
            Contract.Requires(path != null);

            string token = _fs.BeginDownload(path, out fileInfo);
            ReadFileToken ft = _fs.GetToken(token) as ReadFileToken;
            if (ft == null)
                throw Utility.CreateBusinessException(FileStatusCode.ReadNotSupported);

            ReadableStreamTableTag tag = new ReadableStreamTableTag() { Token = ft };
            ft.Tag = tag;
            basicInfo = tag.BasicInfo;

            return token;
        }

        /// <summary>
        /// 下载StreamTable
        /// </summary>
        /// <param name="token"></param>
        /// <param name="table"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public StreamTableRowData[] Download(string token, string table, int start, int count)
        {
            Contract.Requires(token != null);

            ReadableStreamTableTag tag = _GetTag<ReadableStreamTableTag>(token, FileStatusCode.ReadNotSupported);
            if (tag == null)
                throw Utility.CreateBusinessException(FileStatusCode.ReadNotSupported);

            StreamTable st = tag.TableReader.GetTable(table);
            if (st == null)
                throw Utility.CreateBusinessException(FileStatusCode.StreamTableNotFound);

            return st.GetRows(start, count).ToArray(r => new StreamTableRowData() { Datas = r.GetValueArray().ToArray(v => v.ToStringIgnoreNull()) });
        }
    }
}
