using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.UnionTable.Metadata
{
    /// <summary>
    /// 数据库元数据
    /// </summary>
    public class MtDatabase : MtBase
    {
        public MtDatabase(string name, string desc = "")
            : base(name, desc)
        {
            TableGroups = new MtCollection<MtTableGroup, MtDatabase>(this);
            Connections = new MtCollection<MtConnection, MtDatabase>(this);
        }

        /// <summary>
        /// 表组
        /// </summary>
        public MtCollection<MtTableGroup, MtDatabase> TableGroups { get; private set; }

        /// <summary>
        /// 连接
        /// </summary>
        public MtCollection<MtConnection, MtDatabase> Connections { get; private set; }
    }
}
