using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// UnionTable的调用设置
    /// </summary>
    [Serializable, DataContract]
    public class UtInvokeSettings
    {
        public UtInvokeSettings()
        {
            PartialTableMask = -1L;
        }

        /// <summary>
        /// 如果当前操作影响到所有的行，是否确认这种行为，该值用于防止漏写查询条件，而误删或误更新所有的记录
        /// </summary>
        [DataMember]
        public bool EnsureEffectAll { get; set; }

        /// <summary>
        /// 分表索引号（如果为-1则未指定分表索引）
        /// </summary>
        [DataMember]
        public long PartialTableMask { get; set; }

        /// <summary>
        /// 默认调用设置
        /// </summary>
        public static readonly UtInvokeSettings Default = new UtInvokeSettings();

        /// <summary>
        /// 通过指定分表索引创建
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static UtInvokeSettings FromPartialTable(int index)
        {
            Contract.Requires(index >= 0 && index < sizeof(long));

            return new UtInvokeSettings { PartialTableMask = (1L << index) };
        }

        /// <summary>
        /// 通过指定分表索引创建
        /// </summary>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public static UtInvokeSettings FromPartialTable(params int[] indexes)
        {
            long mask = 0L;
            for (int k = 0; k < indexes.Length; k++)
            {
                int i = indexes[k];
                Contract.Requires(i >= 0 && i < sizeof(long));
                mask |= (1L << i);
            }

            return new UtInvokeSettings { PartialTableMask = mask };
        }

        /// <summary>
        /// 确认更新全部的列
        /// </summary>
        /// <param name="ensureEffectAll"></param>
        /// <returns></returns>
        public static UtInvokeSettings FromEnsureEffectAll(bool ensureEffectAll = true)
        {
            return new UtInvokeSettings { EnsureEffectAll = ensureEffectAll };
        }
    }
}
