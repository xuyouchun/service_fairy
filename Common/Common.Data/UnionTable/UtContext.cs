using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Data.UnionTable.Metadata;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 表的执行策略
    /// </summary>
    public class UtContext
    {
        public UtContext(TableInfo tableInfo, IUtRoute route, IUtSchema schema, MtTable table)
        {
            Contract.Requires(tableInfo != null && route != null && schema != null && table != null);

            TableInfo = tableInfo;
            Route = route;
            Schema = schema;
            MtTable = table;
        }

        /// <summary>
        /// 表的信息
        /// </summary>
        public TableInfo TableInfo { get; private set; }

        /// <summary>
        /// 数据路由器
        /// </summary>
        public IUtRoute Route { get; private set; }

        /// <summary>
        /// 用于读取一些重要的数据
        /// </summary>
        public IUtSchema Schema { get; private set; }

        /// <summary>
        /// 表的元数据
        /// </summary>
        public MtTable MtTable { get; private set; }
    }
}
