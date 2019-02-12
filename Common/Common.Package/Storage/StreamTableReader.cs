using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Contracts;
using System.Data;

namespace Common.Package.Storage
{
    /// <summary>
    /// 读取器
    /// </summary>
    public class StreamTableReader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream"></param>
        public StreamTableReader(Stream stream)
        {
            Contract.Requires(stream != null);

            _stream = stream;
            _wrapper = new Lazy<Wrapper>(_LoadWrapper);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="file"></param>
        public StreamTableReader(string file)
            : this(_ReadStreamFromFile(file))
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="buffer"></param>
        public StreamTableReader(byte[] buffer)
            : this(new MemoryStream(buffer))
        {

        }

        private static Stream _ReadStreamFromFile(string file)
        {
            Contract.Requires(file != null);
            return new MemoryStream(File.ReadAllBytes(file));
        }

        private readonly Stream _stream;
        private Lazy<Wrapper> _wrapper;

        class Wrapper
        {
            public StreamTable[] Tables;
            public StreamTableHeaderInfo HeaderInfo;
            public Dictionary<string, StreamTable> TableDict;
        }

        private Wrapper _LoadWrapper()
        {
            if (_stream.ReadUInt32() != StreamTableSettings.SIGN)
                throw _CreateFormatException();

            SVersion version = new SVersion(_stream.ReadUInt32());
            IStreamTableStorage storage = StreamTableStorageBase.Create(version);
            StreamTableHeaderInfo headerInfo;
            StreamTable[] tables = storage.Read(_stream, out headerInfo);

            return new Wrapper() { HeaderInfo = headerInfo, Tables = tables, TableDict = tables.ToDictionary(table => table.Name) };
        }

        private Exception _CreateFormatException()
        {
            return new FormatException("流格式不正确");
        }

        /// <summary>
        /// 表数量
        /// </summary>
        public int TableCount
        {
            get { return _wrapper.Value.Tables.Length; }
        }

        /// <summary>
        /// 按索引读取表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StreamTable this[int index]
        {
            get { return _wrapper.Value.Tables[index]; }
        }

        /// <summary>
        /// 头部信息
        /// </summary>
        public StreamTableHeaderInfo HeaderInfo
        {
            get
            {
                return _wrapper.Value.HeaderInfo;
            }
        }

        /// <summary>
        /// 读取所有的表
        /// </summary>
        /// <returns></returns>
        public StreamTable[] GetAllTables()
        {
            return _wrapper.Value.Tables;
        }

        /// <summary>
        /// 读取指定的表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public StreamTable GetTable(string tableName)
        {
            Contract.Requires(tableName != null);
            return _wrapper.Value.TableDict.GetOrDefault(tableName);
        }

        /// <summary>
        /// 将StreamTableReader转换为DataSet
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DataSet ToDataSet()
        {
            StreamTableHeaderInfo headerInfo = HeaderInfo;
            DataSet ds = new DataSet(string.IsNullOrEmpty(headerInfo.Name) ? "stream_table" : headerInfo.Name);

            foreach (StreamTable st in GetAllTables())
            {
                ds.Tables.Add(st.ToDataTable());
            }

            return ds;
        }

        /// <summary>
        /// 从文件中读取DataSet
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataSet ReadDataSet(string file)
        {
            Contract.Requires(file != null);

            return new StreamTableReader(file).ToDataSet();
        }

        public static DataSet ReadDataSet(Stream stream)
        {
            Contract.Requires(stream != null);
            return new StreamTableReader(stream).ToDataSet();
        }

        public static DataSet ReadDataSet(byte[] buffer)
        {
            Contract.Requires(buffer != null);
            return new StreamTableReader(buffer).ToDataSet();
        }
    }
}
