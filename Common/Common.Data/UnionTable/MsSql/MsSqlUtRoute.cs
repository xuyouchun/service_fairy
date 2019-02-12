using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using Common.Package;

namespace Common.Data.UnionTable.MsSql
{
    class MsSqlUtRoute : IUtRoute
    {
        public MsSqlUtRoute(MsSqlUtProvider utProvider, AutoLoad<MtDatabase> mtDb)
        {
            _utProvider = utProvider;
            _mtDb = mtDb;
        }

        private readonly MsSqlUtProvider _utProvider;
        private readonly AutoLoad<MtDatabase> _mtDb;

        /// <summary>
        /// 根据路由键获取连接点
        /// </summary>
        /// <param name="table"></param>
        /// <param name="routeKey"></param>
        /// <param name="usageType"></param>
        /// <returns></returns>
        public IConnectionPoint GetConnectionPoint(MtTable table, object routeKey, DbConnectionUsageType usageType)
        {
            int hashcode = Math.Abs(routeKey.GetHashCode());
            if (table.Owner.RouteType == DbRouteType.Mod)
            {
                int tableCount = Math.Max(1, table.PartialTableCount);
                int index = (hashcode >> 8) % tableCount;

                return _utProvider.ConnectionPointManager.GetTableConnectionPoint(table, index, usageType);
            }

            throw new NotSupportedException(string.Format("不支持路由类型“{0}”", table.Owner.RouteType));
        }
    }
}
