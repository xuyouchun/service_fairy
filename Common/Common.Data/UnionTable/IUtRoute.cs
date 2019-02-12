using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 数据路由器
    /// </summary>
    public interface IUtRoute
    {
        /// <summary>
        /// 根据路由键获取连接点
        /// </summary>
        /// <param name="table"></param>
        /// <param name="routeKey"></param>
        /// <param name="usageType"></param>
        /// <returns></returns>
        IConnectionPoint GetConnectionPoint(MtTable table, object routeKey, DbConnectionUsageType usageType);
    }
}
