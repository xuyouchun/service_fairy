using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Contracts;

namespace Common.Package.Storage
{
    /// <summary>
    /// 流存储策略
    /// </summary>
    interface IStreamTableStorage
    {
        /// <summary>
        /// 将表存储在流中
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="option"></param>
        /// <param name="stream"></param>
        void Write(StreamTableWriter writer, StreamTableModelOption option, Stream stream);

        /// <summary>
        /// 从指定的流中读取
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="headerInfo"></param>
        StreamTable[] Read(Stream stream, out StreamTableHeaderInfo headerInfo);
    }

    abstract class StreamTableStorageBase : IStreamTableStorage
    {
        public abstract void Write(StreamTableWriter writer, StreamTableModelOption option, Stream stream);

        public abstract StreamTable[] Read(Stream stream, out StreamTableHeaderInfo headerInfo);

        public static IStreamTableStorage Create(SVersion version)
        {
            if (version == "1.0" || version.IsEmpty)
                return new StreamTableStorage_V1();

            throw new NotSupportedException("不支持该版本:" + version);
        }
    }
}
