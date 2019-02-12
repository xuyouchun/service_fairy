using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using System.Diagnostics.Contracts;
using Common.Data.UnionTable.MsSql;
using Common.Package;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 连接点管理器
    /// </summary>
    public interface IConnectionPointManager
    {
        /// <summary>
        /// 获取指定表的连接点
        /// </summary>
        /// <param name="usageType">用途</param>
        /// <param name="fullTableName">表名</param>
        /// <param name="index">索引号</param>
        /// <returns></returns>
        IConnectionPoint GetTableConnectionPoint(string fullTableName, int index, DbConnectionUsageType usageType);

        /// <summary>
        /// 获取指定表的所有连接点
        /// </summary>
        /// <param name="fullTableName"></param>
        /// <param name="usageType"></param>
        /// <returns></returns>
        IConnectionPoint[] GetAllTableConnectionPoints(string fullTableName, DbConnectionUsageType usageType);

        /// <summary>
        /// 获取元数据的连接点
        /// </summary>
        /// <returns></returns>
        IConnectionPoint GetMetadataConnectionPoint();
    }

    /// <summary>
    /// 连接点
    /// </summary>
    public interface IConnectionPoint
    {
        /// <summary>
        /// 分表索引号
        /// </summary>
        int PartialTableIndex { get; }

        /// <summary>
        /// 验证连接点的有效性
        /// </summary>
        void Check();

        /// <summary>
        /// 表编辑器
        /// </summary>
        IUtEditor CreateUtEditor();

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        IDbQuery CreateDbQuery();

        /// <summary>
        /// 创建表连接
        /// </summary>
        /// <param name="utCtx"></param>
        /// <returns></returns>
        IUtConnection CreateTableConnection(UtContext utCtx);
    }
}

