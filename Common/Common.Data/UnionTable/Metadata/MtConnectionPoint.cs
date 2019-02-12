using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Data.UnionTable.Metadata
{
    /// <summary>
    /// 表的连接点
    /// </summary>
    [Serializable, DataContract]
    public class MtConnectionPoint : MtBase<MtConnection>
    {
        public MtConnectionPoint(string name, string desc, string usage, MtConnection connection)
            : base(name, desc)
        {
            Usage = usage;
            Connection = connection;
        }

        /// <summary>
        /// 连接ID
        /// </summary>
        [DataMember]
        public MtConnection Connection { get; private set; }

        /// <summary>
        /// 用途
        /// </summary>
        [DataMember]
        public string Usage { get; private set; }
    }
}
