using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Data.SqlExpressions;
using System.Diagnostics.Contracts;

namespace Common.Data
{
    /// <summary>
    /// 数据库搜索参数
    /// </summary>
    [Serializable, DataContract]
    public class DbSearchParam
    {
        public DbSearchParam(string where, string order, int start, int count)
        {
            Where = where;
            Order = order;
            Start = start;
            Count = count;
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        [DataMember]
        public string Where { get; private set; }

        /// <summary>
        /// 排序表达式
        /// </summary>
        [DataMember]
        public string Order { get; set; }

        /// <summary>
        /// 起始
        /// </summary>
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        [DataMember]
        public int Count { get; set; }

        public static readonly DbSearchParam Empty = new DbSearchParam("", "", 0, -1);

        /// <summary>
        /// 仅指定查询条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DbSearchParam FromWhere(string where)
        {
            return new DbSearchParam(where, "", 0, -1);
        }

        /// <summary>
        /// 选择指定数量的记录
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序表达式</param>
        /// <returns></returns>
        public static DbSearchParam Top(int count, string where = null, string order = null)
        {
            Contract.Requires(count >= 0);

            return new DbSearchParam(where, order, 0, count);
        }

        /// <summary>
        /// 选择第一条记录
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static DbSearchParam TopOne(string where = null, string order = null)
        {
            return Top(1, where, order);
        }

        /// <summary>
        /// 从原型创建
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static DbSearchParam FromPrototype(DbSearchParam prototype, int count)
        {
            if (prototype == null)
                return new DbSearchParam(null, null, 0, count);

            return new DbSearchParam(prototype.Where, prototype.Order, prototype.Start, count);
        }

        /// <summary>
        /// 从原型创建
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="where"></param>
        /// <param name="combine"></param>
        /// <returns></returns>
        public static DbSearchParam FromPrototype(DbSearchParam prototype, string where, bool combine = true)
        {
            prototype = prototype ?? Empty;

            if (!combine)
            {
                return new DbSearchParam(where, prototype.Order, prototype.Start, prototype.Count);
            }
            else
            {
                return new DbSearchParam(CombineWhere(prototype.Where, where), prototype.Order, prototype.Start, prototype.Count);
            }
        }

        /// <summary>
        /// 将多个过滤条件合并在一起
        /// </summary>
        /// <param name="wheres"></param>
        /// <returns></returns>
        public static string CombineWhere(params string[] wheres)
        {
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < wheres.Length; k++)
            {
                string where = wheres[k];
                if (!string.IsNullOrWhiteSpace(where))
                {
                    if (sb.Length > 0)
                        sb.Append(" And ");

                    sb.Append("(").Append(where).Append(")");
                }
            }

            return sb.ToString();
        }
    }
}
