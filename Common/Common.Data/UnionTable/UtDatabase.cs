using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Collection;
using Common.Data.UnionTable.Metadata;
using Common.Package;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// UnionTable数据库
    /// </summary>
    public class UtDatabase : IUtConnectionProvider
    {
        internal UtDatabase(IUtProvider utProvider)
        {
            _utProvider = utProvider;
        }

        private readonly IUtProvider _utProvider;

        /// <summary>
        /// 初始化元数据表结构
        /// </summary>
        public void InitMetaDataSchame()
        {
            IConnectionPoint metadataConPoint = _utProvider.ConnectionPointManager.GetMetadataConnectionPoint();
            metadataConPoint.CreateUtEditor().InitMetaDataSchema();
        }

        /// <summary>
        /// 初始化元数据并修正表信息
        /// </summary>
        /// <param name="reviseInfos"></param>
        /// <param name="callback"></param>
        public void InitMetaData(UtTableGroupReviseInfo[] reviseInfos, UtTableReviseCallback callback = null)
        {
            Contract.Requires(reviseInfos != null);

            MtDatabase mtDb = _utProvider.MetadataManager.LoadMetaData();
            foreach (UtTableGroupReviseInfo groupReviseInfo in reviseInfos)
            {
                try
                {
                    _ReviseTableGroup(_utProvider.ConnectionPointManager, mtDb, groupReviseInfo, callback);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                    if (callback != null)
                        callback(groupReviseInfo.TableGroupName, null, ex);
                }
            }
        }

        private void _ReviseTableGroup(IConnectionPointManager conPointMgr, MtDatabase mtDb, UtTableGroupReviseInfo groupReviseInfo, UtTableReviseCallback callback)
        {
            var g = groupReviseInfo;
            if (string.IsNullOrEmpty(g.TableGroupName))
                throw new DbException("未指定表组名");

            // 添加表组的元数据
            _utProvider.MetadataManager.AddTableGroup(new MtTableGroup(g.TableGroupName, "",
                g.RouteKeyColumnInfo.Name, g.RouteType, g.RouteArgs, g.RouteKeyColumnInfo.Type));

            // 修正表组中的表结构
            foreach (UtTableReviseInfo tableReviseInfo in groupReviseInfo.ReviseInfos)
            {
                try
                {
                    _ReviseTable(conPointMgr, mtDb, groupReviseInfo, tableReviseInfo);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                    if (callback != null)
                        callback(groupReviseInfo.TableGroupName, tableReviseInfo.TableName, ex);
                }
            }
        }

        // 修正表的信息及元数据
        private void _ReviseTable(IConnectionPointManager conPointMgr,
            MtDatabase mtDb, UtTableGroupReviseInfo groupReviseInfo, UtTableReviseInfo tableReviseInfo)
        {
            string tableName = tableReviseInfo.TableName;
            if (string.IsNullOrEmpty(tableName))
                throw new DbException("未指定表名");

            int tableCount = _FindPartialTableCount(mtDb, groupReviseInfo, tableReviseInfo);
            string fullTableName = groupReviseInfo.TableGroupName + "." + tableName;

            for (int tableIndex = 0; tableIndex < tableCount; tableIndex++)
            {
                IConnectionPoint conPoint = conPointMgr.GetTableConnectionPoint(fullTableName, tableIndex, DbConnectionUsageType.Data);

                if (conPoint == null)
                    throw new DbMetadataException(string.Format("无法创建表“{0}”的第{1}个分表的连接点", fullTableName, tableIndex));

                UtTableReviseInfo[] riInfos = _SplitTableReviseInfo(groupReviseInfo, tableReviseInfo);
                riInfos.ForEach(ri => ri.TableName = groupReviseInfo.TableGroupName + "." + ri.TableName + "." + tableIndex.ToString().PadLeft(4, '0'));
                conPoint.CreateUtEditor().ReviseTableSchema(riInfos);
            }

            MtTable mt = new MtTable(tableName, "", tableReviseInfo.DefaultGroup, tableReviseInfo.PrimaryKeyColumnInfo.Name,
                tableReviseInfo.PrimaryKeyColumnInfo.Type, tableCount);

            mt.Columns.AddRange(tableReviseInfo.Columns);
            _utProvider.MetadataManager.AddTable(groupReviseInfo.TableGroupName, mt);
        }

        // 将修正信息按组划分开
        private UtTableReviseInfo[] _SplitTableReviseInfo(UtTableGroupReviseInfo groupReviseInfo, UtTableReviseInfo tableReviseInfo)
        {
            IgnoreCaseDictionary<List<MtColumn>> dict = new IgnoreCaseDictionary<List<MtColumn>>();
            List<MtColumn> specialColumns = new List<MtColumn>();
            foreach (MtColumn mtCol in tableReviseInfo.Columns)
            {
                string group, column;
                UtUtility.SplitFullColumnName(mtCol.Name, out group, out column);
                if (string.IsNullOrEmpty(group))
                    specialColumns.Add(mtCol);
                else
                    dict.GetOrSet(group).Add(new MtColumn(column, mtCol.Type, mtCol.Size, mtCol.IndexType));
            }

            return dict.ToArray(item => new UtTableReviseInfo() {
                TableName = tableReviseInfo.TableName + "." + item.Key, InitTableCount = tableReviseInfo.InitTableCount,
                Columns = item.Value.Concat(specialColumns).ToArray(), PrimaryKeyColumnInfo = tableReviseInfo.PrimaryKeyColumnInfo
            });
        }

        private int _FindPartialTableCount(MtDatabase mtDb, UtTableGroupReviseInfo groupReviseInfo, UtTableReviseInfo tableReviseInfo)
        {
            MtTableGroup mtGroup = mtDb.TableGroups[groupReviseInfo.TableGroupName];
            if (mtGroup == null)
                goto _end;

            MtTable mtTable = mtGroup.Tables[tableReviseInfo.TableName];
            if (mtTable == null)
                goto _end;

            return Math.Max(mtTable.PartialTableCount, 1);

            _end:
            int tableCount = tableReviseInfo.InitTableCount;
            if (tableCount == 0)
                return 1;

            if (tableCount < 0)
                throw new DbException("表的数量必须大于0");

            if (!_IsMultipleNumber(tableCount))
                throw new DbException("表的数量必须为1或其成倍增长的数");

            if (tableCount > 256)
                throw new DbException("表的数量不允许超过256");

            return tableCount;
        }

        // 判断是否为1或其成倍增长的数
        private static bool _IsMultipleNumber(int number)
        {
            int num = 1;
            do
            {
                if (num == number)
                    return true;

            } while ((num <<= 1) != 0);

            return false;
        }

        /// <summary>
        /// 获取所有的连接
        /// </summary>
        /// <returns></returns>
        public MtConnection[] GetConnections()
        {
            return _utProvider.MetadataManager.LoadMetaData().Connections.ToArray();
        }

        /// <summary>
        /// 添加一个物理连接
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conType"></param>
        /// <param name="conStr"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
        public MtConnection AddConnection(string name, string conType, string usage, string conStr)
        {
            Contract.Requires(name != null && conType != null && usage != null && conStr != null);

            MtConnection con = new MtConnection(name, "", conType, usage, conStr);
            _utProvider.MetadataManager.AddConnection(con);
            return con;
        }

        /// <summary>
        /// 删除一个物理连接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void DropConnection(string name)
        {
            Contract.Requires(name != null);

            _utProvider.MetadataManager.DropConnection(name);
        }

        /// <summary>
        /// 获取所有表的信息
        /// </summary>
        /// <returns></returns>
        public TableInfo[] GetTableInfos()
        {
            return _utProvider.GetTableContexts().ToArray(md => md.TableInfo);
        }

        /// <summary>
        /// 获取指定表名的信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public TableInfo GetTableInfo(string tableName)
        {
            UtContext md = _utProvider.GetTableContext(tableName);
            return md == null ? null : md.TableInfo;
        }

        /// <summary>
        /// 创建指定表的连接
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IUtConnection CreateUtConnection(string tableName)
        {
            Contract.Requires(tableName != null);

            return _utProvider.CreateUtConnection(tableName);
        }

        /// <summary>
        /// 创建分布式数据库的连接
        /// </summary>
        /// <param name="metadataConStr">元数据的数据库连接字符串</param>
        /// <param name="assemblies">数据库实体类所涉及的程序集</param>
        /// <param name="conType">数据库类型</param>
        /// <returns></returns>
        public static UtDatabase Create(string metadataConStr, Assembly[] assemblies, string conType = "")
        {
            Contract.Requires(metadataConStr != null);

            UtService utSrv = new UtService();
            UtDatabase db = utSrv.Connect(metadataConStr, conType);

            if (!assemblies.IsNullOrEmpty())
            {
                UtTableGroupReviseInfo[] reviseInfos = UtTableGroupReviseInfo.LoadFromAssembly(assemblies);
                db.InitMetaData(reviseInfos);
            }

            return db;
        }

        /// <summary>
        /// 创建分布式数据库的连接
        /// </summary>
        /// <param name="metadataConStr">元数据的数据库连接字符串</param>
        /// <param name="conType">数据库类型</param>
        /// <returns></returns>
        public static UtDatabase Create(string metadataConStr, string conType = "")
        {
            return Create(metadataConStr, (Assembly[])null, conType);
        }

        /// <summary>
        /// 创建分布式数据库的连接
        /// </summary>
        /// <param name="metadataConStr">元数据的数据库连接字符串</param>
        /// <param name="assembly">数据库实体类所涉及的程序集</param>
        /// <param name="conType">数据库类型</param>
        /// <returns></returns>
        public static UtDatabase Create(string metadataConStr, Assembly assembly, string conType = "")
        {
            return Create(metadataConStr, new[] { assembly }, conType);
        }

        /// <summary>
        /// 创建分布式数据库的连接
        /// </summary>
        /// <param name="metadataConStr">元数据的数据库连接字符串</param>
        /// <param name="dbEntityType">数据库实体</param>
        /// <param name="conType">数据库类型</param>
        /// <returns></returns>
        public static UtDatabase Create(string metadataConStr, Type dbEntityType, string conType = "")
        {
            Contract.Requires(dbEntityType != null);

            return Create(metadataConStr, dbEntityType.Assembly, conType);
        }
    }
}
