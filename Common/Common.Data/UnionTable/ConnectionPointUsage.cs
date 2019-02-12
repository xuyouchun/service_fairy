using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 表的用途
    /// </summary>
    public class ConnectionPointUsage
    {
        public ConnectionPointUsage(DbConnectionUsageType usageType, string table)
        {
            Contract.Requires(table != null);

            UsageType = usageType;
            TableName = table;
        }

        /// <summary>
        /// 用途的类型
        /// </summary>
        public DbConnectionUsageType UsageType { get; private set; }

        /// <summary>
        /// 表
        /// </summary>
        public string TableName { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", UsageType, TableName.JoinBy(","));
        }

        public override int GetHashCode()
        {
            return UsageType.GetHashCode() ^ (TableName == null ? 0 : TableName.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ConnectionPointUsage))
                return false;

            ConnectionPointUsage obj2 = (ConnectionPointUsage)obj;
            return UsageType == obj2.UsageType && TableName == obj2.TableName;
        }

        /// <summary>
        /// 尝试将字符串表述的形式转换为对象
        /// </summary>
        /// <param name="s"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out ConnectionPointUsage usage)
        {
            int index;
            if (string.IsNullOrEmpty(s) || (index = s.IndexOf(':')) < 0)
                goto _error;

            DbConnectionUsageType usageType;
            if (!Enum.TryParse<DbConnectionUsageType>(s.Substring(0, index), out usageType))
                goto _error;

            string table = s.Substring(index + 1);
            usage = new ConnectionPointUsage(usageType, table);
            return true;

        _error:
            usage = null;
            return false;
        }

        /// <summary>
        /// 将字符串转化为对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ConnectionPointUsage Parse(string s)
        {
            ConnectionPointUsage usage;
            if (!TryParse(s, out usage))
                throw new FormatException("格式错误");

            return usage;
        }
    }
}
