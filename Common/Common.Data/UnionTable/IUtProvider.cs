using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.MsSql;
using Common.Data.UnionTable.Metadata;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 元数据的提供者
    /// </summary>
    public interface IUtProvider : IUtConnectionProvider
    {
        /// <summary>
        /// 连接点管理器
        /// </summary>
        IConnectionPointManager ConnectionPointManager { get; }

        /// <summary>
        /// 元数据管理器
        /// </summary>
        IMetadataManager MetadataManager { get; }

        /// <summary>
        /// 获取所有表的执行策略
        /// </summary>
        /// <returns></returns>
        UtContext[] GetTableContexts();

        /// <summary>
        /// 获取指定表的执行策略
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        UtContext GetTableContext(string tableName);
    }

    /// <summary>
    /// 表修正过程的回调
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="table"></param>
    /// <param name="error"></param>
    public delegate void UtTableReviseCallback(string groupName, string table, Exception error);

    static class TableMetaDataProviderFactory
    {
        public static IUtProvider Create(string metaConStr, string conType)
        {
            return new MsSqlUtProvider(new ConnectionPointFactory(), metaConStr);
        }
    }
}
