using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.Entities
{
    /// <summary>
    /// 获取实体类的基础信息
    /// </summary>
    public class DbEntityTableInfo
    {
        public DbEntityTableInfo(string tableName, string defaultGroup, ColumnInfo primaryKeyColumnInfo, int initTableCount, 
            string tableGroupName, DbRouteType routeType, string routeArgs, ColumnInfo routeKeyColumnInfo)
        {
            Contract.Requires(tableName != null && primaryKeyColumnInfo != null);

            TableName = tableName;
            DefaultGroup = defaultGroup;
            PrimaryKeyColumnInfo = primaryKeyColumnInfo;
            InitTableCount = initTableCount;

            TableGroupName = tableGroupName;
            RouteType = routeType;
            RouteArgs = routeArgs;
            RouteKeyColumnInfo = routeKeyColumnInfo;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// 默认组
        /// </summary>
        public string DefaultGroup { get; private set; }

        /// <summary>
        /// 主键字段
        /// </summary>
        public ColumnInfo PrimaryKeyColumnInfo { get; private set; }

        /// <summary>
        /// 表的初始数量
        /// </summary>
        public int InitTableCount { get; private set; }

        /// <summary>
        /// 表组名
        /// </summary>
        public string TableGroupName { get; private set; }

        /// <summary>
        /// 路由类型
        /// </summary>
        public DbRouteType RouteType { get; private set; }

        /// <summary>
        /// 路由参数
        /// </summary>
        public string RouteArgs { get; private set; }

        /// <summary>
        /// 路由键列信息
        /// </summary>
        public ColumnInfo RouteKeyColumnInfo { get; private set; }
    }
}
