using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Reflection;
using Common.Utility;
using Common.Data.UnionTable.Metadata;
using System.Collections.ObjectModel;

namespace Common.Data.Entities
{
    /// <summary>
    /// 表实体基类
    /// </summary>
    public abstract class DbEntity
    {
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <returns></returns>
        public abstract object GetPrimaryKey();

        /// <summary>
        /// 设置主键
        /// </summary>
        /// <param name="value">主键值</param>
        public abstract void SetPrimaryKey(object value);

        /// <summary>
        /// 获取路由键
        /// </summary>
        /// <returns></returns>
        public abstract object GetRouteKey();

        /// <summary>
        /// 设置路由键
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetRouteKey(object value);

        /// <summary>
        /// 获取指定字段的值
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <param name="throwError">是否在字段不存在时抛出异常</param>
        /// <returns></returns>
        public abstract object GetValue(string name, bool throwError = true);

        /// <summary>
        /// 设置指定字段的值
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <param name="value">字段值</param>
        /// <param name="throwError">是否在不存在该字段时抛出异常</param>
        public abstract void SetValue(string name, object value, bool throwError = true);

        /// <summary>
        /// 从程序集中寻找数据库表实体类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] LoadEntityTypesFrom(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            return assembly.SearchTypes<DbEntityAttribute>(true);
        }

        /// <summary>
        /// 获取列元数据
        /// </summary>
        /// <returns></returns>
        public abstract ReadOnlyCollection<MtColumn> GetColumns();

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <returns></returns>
        public abstract DbEntityTableInfo GetTableInfo();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            ReadOnlyCollection<MtColumn> columns = GetColumns();
            for (int k = 0; k < columns.Count; k++)
            {
                MtColumn col = columns[k];
                if (k > 0)
                    sb.Append(", ");

                sb.Append(col.Name).Append(" = ").Append(GetValue(col.Name));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 视为空值的时间
        /// </summary>
        public const string Null_DateTime = "0001-01-01 00:00:00";

        /// <summary>
        /// 视为空值的布尔型
        /// </summary>
        public const bool Null_Boolean = false;

        /// <summary>
        /// 视为空值的Byte整型
        /// </summary>
        public const byte Null_UInt8 = 0;

        /// <summary>
        /// 视为空值的整型
        /// </summary>
        public const int Null_Int32 = 0;

        /// <summary>
        /// 视为空值的短整型
        /// </summary>
        public const short Null_Int16 = (short)0;

        /// <summary>
        /// 视为空值的长整型
        /// </summary>
        public const long Null_Int64 = 0L;

        /// <summary>
        /// 空的Guid
        /// </summary>
        public const string Null_Guid = "00000000-0000-0000-0000-000000000000";
    }


    /// <summary>
    /// 表实体基类
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TClass">所定义的类</typeparam>
    public abstract partial class DbEntity<TEntity> : DbEntity
        where TEntity : DbEntity<TEntity>, new()
    {
        /// <summary>
        /// 获取主键值
        /// </summary>
        /// <returns></returns>
        public override object GetPrimaryKey()
        {
            return _w.Value.PrimaryKeyColumn.GetValue(this);
        }

        /// <summary>
        /// 设置主键值
        /// </summary>
        /// <param name="value">主键值</param>
        public override void SetPrimaryKey(object value)
        {
            _w.Value.PrimaryKeyColumn.SetValue(this, value);
        }

        /// <summary>
        /// 获取路由键值
        /// </summary>
        /// <returns></returns>
        public override object GetRouteKey()
        {
            return _w.Value.RouteKeyColumn.GetValue(this);
        }

        /// <summary>
        /// 设置路由键值
        /// </summary>
        /// <param name="value">路由键值</param>
        public override void SetRouteKey(object value)
        {
            _w.Value.RouteKeyColumn.SetValue(this, value);
        }

        /// <summary>
        /// 获取指定字段的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetValue(string name, bool throwError = true)
        {
            Contract.Requires(name != null);

            return _MtUtility.GetFieldInfo(name, throwError).GetValue(this);
        }

        /// <summary>
        /// 设置指定字段的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="throwError"></param>
        public override void SetValue(string name, object value, bool throwError = true)
        {
            Contract.Requires(name != null);

            _ColumnInfo cInfo = _MtUtility.GetFieldInfo(name, throwError);
            if (cInfo != null)
                cInfo.SetValue(this, value);
        }

        /// <summary>
        /// 批量设置字段的值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="throwError"></param>
        public void SetValues(IDictionary<string, object> values, bool throwError = true)
        {
            Contract.Requires(values != null);

            foreach (KeyValuePair<string, object> item in values)
            {
                SetValue(item.Key, item.Value, throwError);
            }
        }
    }
}
