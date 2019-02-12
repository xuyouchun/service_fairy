using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 用于获取表连接
    /// </summary>
    public interface IUtConnectionProvider
    {
        /// <summary>
        /// 创建表连接
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IUtConnection CreateUtConnection(string tableName);
    }

    /// <summary>
    /// 空的数据库表连接提供者
    /// </summary>
    public class UtConnectionProvider : IUtConnectionProvider
    {
        private UtConnectionProvider()
        {

        }

        public IUtConnection CreateUtConnection(string tableName)
        {
            return EmptyUtConnection.Instance;
        }

        public static readonly EmptyUtConnection Instance = EmptyUtConnection.Instance;
    }

    public class EmptyUtConnection : IUtConnection
    {
        private EmptyUtConnection()
        {

        }

        public DataList Select(object[] routeKeys, string[] columns, DbSearchParam param, UtInvokeSettings settings)
        {
            return DataList.Empty;
        }

        public int Insert(DataList data, bool autoUpdate, UtInvokeSettings settings)
        {
            return 0;
        }

        public int Update(DataList data, object[] routeKeys, string where, UtInvokeSettings settings)
        {
            return 0;
        }

        public int Delete(DataList data, string where, UtInvokeSettings settings)
        {
            return 0;
        }

        public int Merge(object routeKey, DataList data, string[] compareColumns, string where, UtConnectionMergeOption option, UtInvokeSettings settings)
        {
            return 0;
        }

        public static readonly EmptyUtConnection Instance = new EmptyUtConnection();
    }
}
