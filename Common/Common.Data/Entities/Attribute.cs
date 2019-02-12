using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.Entities
{
    /// <summary>
    /// 标注表的实体
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DbEntityAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="defaultGroup"></param>
        /// <param name="initTableCount"></param>
        public DbEntityAttribute(string tableName, string defaultGroup, int initTableCount)
        {
            Contract.Requires(tableName != null && initTableCount >= 0);

            TableName = tableName;
            InitTableCount = initTableCount;
            DefaultGroup = defaultGroup;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName"></param>
        public DbEntityAttribute(string tableName, string defaultGroup)
            : this(tableName, defaultGroup, 1)
        {

        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 初始表数量
        /// </summary>
        public int InitTableCount { get; set; }

        /// <summary>
        /// 默认组名
        /// </summary>
        public string DefaultGroup { get; set; }
    }

    /// <summary>
    /// 标注表组实体
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DbTableGroupEntityAttribute : Attribute
    {
        public DbTableGroupEntityAttribute(string tableGroup, DbRouteType routeType, string routeArgs)
        {
            TableGroupName = tableGroup;
            RouteType = routeType;
            RouteArgs = routeArgs;
        }

        public DbTableGroupEntityAttribute(string tableGroup)
            : this(tableGroup, DbRouteType.Mod, "")
        {

        }

        /// <summary>
        /// 表组实体
        /// </summary>
        public string TableGroupName { get; set; }

        /// <summary>
        /// 路由类型
        /// </summary>
        public DbRouteType RouteType { get; set; }

        /// <summary>
        /// 路由参数
        /// </summary>
        public string RouteArgs { get; set; }
    }

    /// <summary>
    /// 字段的类型
    /// </summary>
    [Flags]
    public enum DbColumnKind
    {
        Normal = 0x00,

        /// <summary>
        /// 主键
        /// </summary>
        PrimaryKey = 0x01,

        /// <summary>
        /// 路由键
        /// </summary>
        RouteKey = 0x02,
    }

    /// <summary>
    /// 标注字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class DbColumnAttributeBase : Attribute
    {
        public DbColumnAttributeBase()
        {
            Size = -1;
        }

        /// <summary>
        /// 空值
        /// </summary>
        public object NullValue { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public DbColumnKind ColumnKind { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public DbColumnType ColumnType { get; set; }

        /// <summary>
        /// 索引类型
        /// </summary>
        public DbColumnIndexType IndexType { get; set; }

        /// <summary>
        /// 字段长度限制（-1为默认值，0为Max）
        /// </summary>
        public int Size { get; set; }

        internal static DbColumnAttributeBase Combine(DbColumnAttributeBase[] attrs)
        {
            if (attrs.Length == 0)
                return null;

            if (attrs.Length == 1)
                return attrs[0];

            DbColumnAttributeBase attr = attrs[0];

            for (int k = 1; k < attrs.Length; k++)
            {
                DbColumnAttributeBase attr0 = attrs[k];
                if (attr.DefaultValue == null)
                    attr.DefaultValue = attr0.DefaultValue;

                if (attr.NullValue == null)
                    attr.NullValue = attr0.NullValue;

                if (attr.GroupName == null)
                    attr.GroupName = attr0.GroupName;

                if (attr.FieldName == null)
                    attr.FieldName = attr0.FieldName;

                attr.ColumnKind |= attr0.ColumnKind;

                if (attr.IndexType < attr0.IndexType)
                    attr.IndexType = attr0.IndexType;

                if (attr.ColumnType == DbColumnType.Unknown)
                    attr.ColumnType = attr0.ColumnType;
            }

            return attr;
        }
    }

    /// <summary>
    /// 主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbPrimaryKeyAttribute : DbColumnAttributeBase
    {
        public DbPrimaryKeyAttribute()
        {
            ColumnKind = DbColumnKind.PrimaryKey;
            IndexType = DbColumnIndexType.Slave;
        }
    }

    /// <summary>
    /// 路由键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbRouteKeyAttribute : DbColumnAttributeBase
    {
        public DbRouteKeyAttribute()
        {
            ColumnKind = DbColumnKind.RouteKey;
            IndexType = DbColumnIndexType.Master;
        }
    }

    /// <summary>
    /// 字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbColumnAttribute : DbColumnAttributeBase
    {
        public DbColumnAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}
