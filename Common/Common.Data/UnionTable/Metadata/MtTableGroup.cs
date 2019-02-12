using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Data.UnionTable.Metadata
{
    /// <summary>
    /// 表组
    /// </summary>
    public class MtTableGroup : MtBase<MtDatabase>
    {
        public MtTableGroup(string name, string desc, string routeKey, DbRouteType routeType,
            string routeArgs, DbColumnType routeKeyColumnType) : base(name, desc)
        {
            RouteKey = routeKey;
            RouteType = routeType;
            RouteArgs = routeArgs;
            RouteKeyColumnType = routeKeyColumnType;
            Tables = new MtCollection<MtTable, MtTableGroup>(this);
        }

        /// <summary>
        /// 路由键
        /// </summary>
        [DataMember]
        public string RouteKey { get; private set; }

        /// <summary>
        /// 路由类型
        /// </summary>
        [DataMember]
        public DbRouteType RouteType { get; private set; }

        /// <summary>
        /// 路由参数
        /// </summary>
        [DataMember]
        public string RouteArgs { get; private set; }

        /// <summary>
        /// 路由键列类型
        /// </summary>
        [DataMember]
        public DbColumnType RouteKeyColumnType { get; private set; }

        /// <summary>
        /// 表
        /// </summary>
        [DataMember]
        public MtCollection<MtTable, MtTableGroup> Tables { get; private set; }
    }
}
