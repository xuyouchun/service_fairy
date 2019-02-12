using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using Common.Package;
using System.Diagnostics.Contracts;
using Common.Collection;
using Common.Utility;

namespace Common.Data.UnionTable.MsSql
{
    /// <summary>
    /// MsSql连接点管理器
    /// </summary>
    class MsSqlConnectionPointManager : IConnectionPointManager
    {
        public MsSqlConnectionPointManager(IConnectionPointFactory conPointFactory, MtConnectionPoint metadataConPoint, IMetadataManager metadataMgr, AutoLoad<MtDatabase> mtDb)
        {
            _conPointFactory = conPointFactory;
            _mtDb = mtDb;
            _metadataConPoint = metadataConPoint;
            _metadataMgr = metadataMgr;
        }

        private readonly IConnectionPointFactory _conPointFactory;
        private readonly AutoLoad<MtDatabase> _mtDb;
        private readonly MtConnectionPoint _metadataConPoint;
        private readonly IMetadataManager _metadataMgr;

        /// <summary>
        /// 获取元数据表的连接点
        /// </summary>
        /// <returns></returns>
        public IConnectionPoint GetMetadataConnectionPoint()
        {
            return new MsSqlConnectionPoint(_metadataConPoint);
        }

        /// <summary>
        /// 获取指定表的所有连接点
        /// </summary>
        /// <param name="fullTableName">全表名</param>
        /// <param name="usageType">用途</param>
        /// <returns>连接点</returns>
        public IConnectionPoint[] GetAllTableConnectionPoints(string fullTableName, DbConnectionUsageType usageType)
        {
            MtDatabase mtDb = _mtDb.Value;

            string tableGroup, tableName;
            _SplitFullTableName(fullTableName, out tableGroup, out tableName);

            MtTableGroup mtg;
            MtTable mt;

            if ((mtg = mtDb.TableGroups[tableGroup]) == null || (mt = mtg.Tables[tableName]) == null)
                throw new DbException(string.Format("表“{0}”不存在", fullTableName));

            IConnectionPoint[] conPoints = new IConnectionPoint[mt.PartialTableCount];
            for (int k = 0; k < conPoints.Length; k++)
            {
                conPoints[k] = GetTableConnectionPoint(fullTableName, k, usageType);
            }

            return conPoints;
        }

        private static void _SplitFullTableName(string fullTableName, out string tableGroupName, out string tableName)
        {
            int k = fullTableName.IndexOf('.');
            if (k < 0)
                throw new DbMetadataException("获取表连接点时未指定表的全名");

            tableGroupName = fullTableName.Substring(0, k);
            tableName = fullTableName.Substring(k + 1);
        }

        /// <summary>
        /// 获取表的连接点
        /// </summary>
        /// <param name="fullTableName">全表名</param>
        /// <param name="patialTableIndex">分表索引</param>
        /// <param name="usageType">用途</param>
        /// <returns>连接点</returns>
        public IConnectionPoint GetTableConnectionPoint(string fullTableName, int patialTableIndex, DbConnectionUsageType usageType)
        {
            MtDatabase mtDb = _mtDb.Value;

            string tableGroup, tableName;
            _SplitFullTableName(fullTableName, out tableGroup, out tableName);
            string usageStr = UtUtility.CreateUsage(tableGroup, tableName, usageType, patialTableIndex);

            IDictionary<string, MtConnectionPoint> dict = mtDb.Tag.Get<string, MtConnectionPoint>("ConnectionPointCache", () => _LoadConnectionPointCache(mtDb));
            MtConnectionPoint conPoint;
            if (!dict.TryGetValue(usageStr.ToLower(), out conPoint))
            {
                // 创建表连接点并清除缓存
                MtConnection con = _FindDbConnection(mtDb, usageType, tableGroup);
                if (con == null)
                    throw new DbMetadataException(string.Format("未配置用途为“{0}”的连接", usageType + ":" + tableGroup));

                conPoint = new MtConnectionPoint(usageStr, "", usageStr, con);
                _AddConnectionPoint(conPoint);
                _mtDb.ClearCache();
            }

            return _conPointFactory.CreateConnectionPoint(conPoint, patialTableIndex);
        }

        // 寻找表连接
        private MtConnection _FindDbConnection(MtDatabase mtDb, DbConnectionUsageType usage, string tableGroup)
        {
            MtConnection[] cons = mtDb.Connections.Where(con => _IsUsage(con, usage, tableGroup)).ToArray();
            if (cons.Length == 0)
                return null;

            MtConnection minCon = cons[0];
            for (int k = 1; k < cons.Length; k++)
            {
                if (cons[k].ConnectionPoints.Count < minCon.ConnectionPoints.Count)
                    minCon = cons[k];
            }

            return minCon;
        }

        // 判断该连接是否用于指定的用途
        private bool _IsUsage(MtConnection con, DbConnectionUsageType usage, string tableGroup)
        {
            string conUsage = con.Usage;
            if (!conUsage.StartsWith(usage + ":", StringComparison.OrdinalIgnoreCase))
                return false;

#warning 此处算法需要优化一下 ...
            string[] tableGroups = conUsage.Substring(conUsage.IndexOf(':') + 1).Split(',').ToArray(s => s.Trim());
            return tableGroups.Contains(tableGroup, IgnoreCaseEqualityComparer.Instance);
        }

        private IDictionary<string, MtConnectionPoint> _LoadConnectionPointCache(MtDatabase mtDb)
        {
            return mtDb.Connections.SelectMany(con => con.ConnectionPoints).ToDictionary(p => p.Usage.ToLower());
        }

        // 添加一个连接点
        private void _AddConnectionPoint(MtConnectionPoint conPoint)
        {
            _metadataMgr.AddConnectionPoint(conPoint, true);
        }
    }
}
