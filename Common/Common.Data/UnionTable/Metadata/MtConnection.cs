using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Data.UnionTable.Metadata
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    [Serializable, DataContract]
    public class MtConnection : MtBase<MtDatabase>
    {
        public MtConnection(string name, string desc, string conType, string usage, string conStr)
            : base(name, desc)
        {
            ConType = conType;
            Usage = usage;
            ConStr = conStr;

            ConnectionPoints = new MtCollection<MtConnectionPoint, MtConnection>(this);
        }

        /// <summary>
        /// 连接类型
        /// </summary>
        [DataMember]
        public string ConType { get; private set; }

        /// <summary>
        /// 用途
        /// </summary>
        [DataMember]
        public string Usage { get; private set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [DataMember]
        public string ConStr { get; private set; }

        /// <summary>
        /// 连接点
        /// </summary>
        public MtCollection<MtConnectionPoint, MtConnection> ConnectionPoints { get; private set; }

        public override string ToString()
        {
            return ConStr;
        }
    }

    /// <summary>
    /// 数据库连接类型
    /// </summary>
    public static class DbConnectionTypes
    {
        /// <summary>
        /// Sql-Server连接
        /// </summary>
        public const string MsSql = "MsSql";
    }
}
