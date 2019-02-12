using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.IO;
using Common.Contracts;

namespace Common.Package.Storage
{
    /// <summary>
    /// StreamTable创建器
    /// </summary>
    public class StreamTableWriter : MarshalByRefObject
    {
        public StreamTableWriter(string name = "", string desc = "")
        {
            Name = name;
            Desc = desc;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 创建一个新的表格
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="desc"> </param>
        public WritableStreamTable CreateTable(string tableName, StreamTableColumn[] columns, string desc = "")
        {
            Contract.Requires(tableName != null && columns != null);

            WritableStreamTable table = new WritableStreamTable(tableName, columns, desc);
            _AppendTable(tableName, table);
            return table;
        }

        private void _AppendTable(string tableName, StreamTable table)
        {
            if (_tablesDict.ContainsKey(tableName))
                throw new InvalidOperationException("该表已经存在:" + tableName);

            _tablesDict.Add(tableName, table);
            _tables.Add(table);
        }

        private readonly List<StreamTable> _tables = new List<StreamTable>();
        private readonly Dictionary<string, StreamTable> _tablesDict = new Dictionary<string, StreamTable>();

        /// <summary>
        /// 获取全部的表
        /// </summary>
        /// <returns></returns>
        public StreamTable[] GetTables()
        {
            return _tables.ToArray();
        }

        /// <summary>
        /// 将内容写入到流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="option"></param>
        /// <param name="version"></param>
        public void WriteToStream(Stream stream, StreamTableModelOption option = null, SVersion version = default(SVersion))
        {
            Contract.Requires(stream != null);

            IStreamTableStorage storage = StreamTableStorageBase.Create(version);

            if (!StreamTableModelOption.IsAuto(option))
            {
                storage.Write(this, option ?? StreamTableModelOption.Small, stream);
            }
            else  // 尝试以各种模式进行存储
            {
                for (int k = 0; k < _allStreamTableModelOptions.Length; k++)
                {
                    StreamTableModelOption model = _allStreamTableModelOptions[k];

                    try
                    {
                        storage.Write(this, model, stream);
                        break;
                    }
                    catch (OverflowException)
                    {
                        if (k >= _allStreamTableModelOptions.Length - 1)
                            throw;
                    }
                }
            }
        }

        private static readonly StreamTableModelOption[] _allStreamTableModelOptions = new[] { StreamTableModelOption.Small, StreamTableModelOption.Large };

        /// <summary>
        /// 将内容写入到文件中
        /// </summary>
        /// <param name="file"></param>
        /// <param name="option"></param>
        /// <param name="version"></param>
        public void WriteToFile(string file, StreamTableModelOption option = null, SVersion version = default(SVersion))
        {
            Contract.Requires(file != null);
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                WriteToStream(fs, option, version);
            }
        }

        /// <summary>
        /// 将内容写入到流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        /// <param name="version"></param>
        public void WriteToStream(Stream stream, StreamTableModel model, SVersion version = default(SVersion))
        {
            WriteToStream(stream, StreamTableModelOption.GetByModel(model), version);
        }

        /// <summary>
        /// 将内容写入到文件中
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <param name="version"></param>
        public void WriteToFile(string file, StreamTableModel model, SVersion version = default(SVersion))
        {
            Contract.Requires(file != null);
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                WriteToStream(fs, model, version);
            }
        }
    }
}
