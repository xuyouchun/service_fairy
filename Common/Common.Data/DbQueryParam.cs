using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics.Contracts;

namespace Common.Data
{
    /// <summary>
    /// 数据库查询参数
    /// </summary>
    public class DbQueryParam
    {
        public DbQueryParam(string name, object value, DbType dbType = DbType.Object, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            Contract.Requires(name != null);

            Name = name;
            Value = value ?? DBNull.Value;
            DbType = dbType;
            Direction = direction;
            Size = size;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// 类型
        /// </summary>
        public DbType DbType { get; private set; }

        /// <summary>
        /// 方向
        /// </summary>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public int Size { get; private set; }
    }
}
