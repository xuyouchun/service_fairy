using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 运营商前缀
    /// </summary>
    public class OperationPrefixes
    {
        public OperationPrefixes(int minLength, int maxLength, IList<OperationPrefix> list)
        {
            Contract.Requires(minLength >= 0 && maxLength >= 0 && list != null);

            MinLength = minLength;
            MaxLength = maxLength;
            List = list;
        }

        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength { get; private set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// 运营商前缀列表，按前缀顺序排序
        /// </summary>
        public IList<OperationPrefix> List { get; private set; }
    }

    /// <summary>
    /// 运营商前缀
    /// </summary>
    public class OperationPrefix
    {
        public OperationPrefix(string prefix, OperationInfo operationInfo)
        {
            Contract.Requires(prefix != null && operationInfo != null);

            Prefix = prefix;
            OperationInfo = operationInfo;
        }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// 运营商
        /// </summary>
        public OperationInfo OperationInfo { get; private set; }
    }
}
