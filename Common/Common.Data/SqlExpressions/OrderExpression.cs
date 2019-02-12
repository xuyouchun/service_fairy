using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.Text.RegularExpressions;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 排序表达式
    /// </summary>
    public class OrderExpression : SqlExpressionBase, ISqlExpressionWithFieldName
    {
        internal OrderExpression(OrderExpressionItem[] items)
        {
            Contract.Requires(items != null);

            Items = items;
        }

        /// <summary>
        /// 排序项
        /// </summary>
        public OrderExpressionItem[] Items { get; private set; }

        protected override string OnToString(ISqlExpressionContext context)
        {
            return string.Join(" ", (object[])Items);
        }

        public string[] GetFieldNames()
        {
            return Items.ToArray(item => item.FieldName);
        }

        /// <summary>
        /// 将字符串转换为表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public new static OrderExpression Parse(string exp)
        {
            Contract.Requires(exp != null);

            List<OrderExpressionItem> items = new List<OrderExpressionItem>();
            foreach (string item in exp.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = Regex.Split(item.Trim(), @"\s+");
                if (parts.Length > 2)
                    throw new FormatException("排序表达式语法错误：" + item);

                OrderExpressionType orderType;
                if (parts.Length > 1)
                {
                    if (!Enum.TryParse<OrderExpressionType>(parts[1], true, out orderType))
                        throw new FormatException("排序表达式语法错误：" + item);
                }
                else
                {
                    orderType = OrderExpressionType.Asc;
                }

                items.Add(new OrderExpressionItem(parts[0], orderType));
            }

            return new OrderExpression(items.ToArray());
        }
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    public enum OrderExpressionType
    {
        /// <summary>
        /// 正序
        /// </summary>
        Asc,

        /// <summary>
        /// 逆序
        /// </summary>
        Desc
    }

    /// <summary>
    /// 排序项
    /// </summary>
    [Serializable, DataContract]
    public class OrderExpressionItem
    {
        public OrderExpressionItem(string fieldName, OrderExpressionType orderType)
        {
            Contract.Requires(fieldName != null);

            FieldName = fieldName;
            OrderType = orderType;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        [DataMember]
        public string FieldName { get; private set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        [DataMember]
        public OrderExpressionType OrderType { get; private set; }

        public override string ToString()
        {
            return FieldName + " " + OrderType;
        }
    }
}
