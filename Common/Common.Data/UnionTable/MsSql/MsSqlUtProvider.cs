using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using Common.Package;
using Common.Utility;
using System.Data;
using System.Collections;

namespace Common.Data.UnionTable.MsSql
{
    /// <summary>
    /// sql-server的元数据驱动
    /// </summary>
    class MsSqlUtProvider : IUtProvider
    {
        public MsSqlUtProvider(IConnectionPointFactory conPointFactory, string conStr)
        {
            Contract.Requires(conPointFactory != null && conStr != null);

            ConStr = conStr;
            Cache = new Hashtable();
            _mtDb = new AutoLoad<MtDatabase>(_LoadMetaData, TimeSpan.FromSeconds(30));
            _metadataCon = new MtConnection("mssql_metadata", "", DbConnectionTypes.MsSql, DbConnectionUsageType.Metadata.ToString(), conStr);
            _metadataConPoint = new MtConnectionPoint("mssql_metadata_connection_point", "", "", _metadataCon);
            _sqlTableEditor = new MsSqlUtEditor(_metadataConPoint);

            _metadataMgr = new MetadataManagerProxy(this, new MsSqlMetadataManager(conPointFactory, new MsSqlConnectionPoint(_metadataConPoint)));
            _conPointMgr = new MsSqlConnectionPointManager(conPointFactory, _metadataConPoint, _metadataMgr, _mtDb);
        }

        private readonly MtConnection _metadataCon;
        private readonly MtConnectionPoint _metadataConPoint;
        private readonly IConnectionPointManager _conPointMgr;
        private readonly IMetadataManager _metadataMgr;
        private readonly AutoLoad<MtDatabase> _mtDb;

        /// <summary>
        /// 元数据库连接字符串
        /// </summary>
        public string ConStr { get; private set; }

        private readonly MsSqlUtEditor _sqlTableEditor;

        private MtDatabase _LoadMetaData()
        {
            return _metadataMgr.LoadMetaData();
        }

        /// <summary>
        /// 获取表的执行环境
        /// </summary>
        /// <returns></returns>
        public UtContext[] GetTableContexts()
        {
            return _GetTableContextsDict().Values.ToArray();
        }

        private IDictionary<string, UtContext> _GetTableContextsDict()
        {
            MtDatabase mtDb = _mtDb.Value;
            return mtDb.Tag.Get<string, UtContext>(null, () => _GetTableContexts(mtDb));
        }

        private IDictionary<string, UtContext> _GetTableContexts(MtDatabase mtDb)
        {
            return mtDb.TableGroups.SelectMany(tg => tg.Tables.Select(t => new UtContext(
                new TableInfo(tg.Name, t.Name, t.Columns.ToArray(c => c.ToColumnInfo()), t.PartialTableCount),
                new MsSqlUtRoute(this, _mtDb), new MsSqlUtSchema(this, t), t
            ))).ToIgnoreCaseDictionary(v => UtUtility.BuildFullColumnName(v.TableInfo.Group, v.TableInfo.Name));
        }

        /// <summary>
        /// 获取表的执行环境
        /// </summary>
        /// <param name="fullTableName"></param>
        /// <returns></returns>
        public UtContext GetTableContext(string fullTableName)
        {
            Contract.Requires(fullTableName != null);

            return _GetTableContextsDict().GetOrDefault(fullTableName);
        }

        /// <summary>
        /// 创建表连接
        /// </summary>
        /// <param name="fullTableName"></param>
        /// <returns></returns>
        public IUtConnection CreateUtConnection(string fullTableName)
        {
            UtContext utCtx = GetTableContext(fullTableName);
            if (utCtx == null)
                throw new DbException(string.Format("表“{0}”不存在", fullTableName));

            return new UtConnection(utCtx, this);
        }

        /// <summary>
        /// 连接点管理器
        /// </summary>
        public IConnectionPointManager ConnectionPointManager
        {
            get { return _conPointMgr; }
        }

        /// <summary>
        /// 元数据管理器
        /// </summary>
        public IMetadataManager MetadataManager
        {
            get { return _metadataMgr; }
        }

        #region Class MetadataManagerProxy ...

        class MetadataManagerProxy : IMetadataManager
        {
            public MetadataManagerProxy(MsSqlUtProvider owner, IMetadataManager mdMgr)
            {
                _owner = owner;
                _mdMgr = mdMgr;
            }

            private readonly IMetadataManager _mdMgr;
            private readonly MsSqlUtProvider _owner;

            private void _Refresh()
            {
                _owner._mtDb.ClearCache();
            }

            public MtDatabase LoadMetaData()
            {
                return _mdMgr.LoadMetaData();
            }

            public bool AddConnectionPoint(MtConnectionPoint conPoint, bool throwError = false)
            {
                if (_mdMgr.AddConnectionPoint(conPoint, throwError))
                {
                    _Refresh();
                    return true;
                }

                return false;
            }

            public bool AddConnection(MtConnection con, bool throwError = false)
            {
                if (_mdMgr.AddConnection(con, throwError))
                {
                    _Refresh();
                    return true;
                }

                return false;
            }

            public bool DropConnection(string name, bool throwError = false)
            {
                if (_mdMgr.DropConnection(name, throwError))
                {
                    _Refresh();
                    return true;
                }

                return false;
            }

            public bool AddTableGroup(MtTableGroup group, bool throwError = false)
            {
                if (_mdMgr.AddTableGroup(group, throwError))
                {
                    _Refresh();
                    return true;
                }

                return false;
            }

            public void AddTable(string tableGroupName, MtTable table, bool throwError = false)
            {
                _mdMgr.AddTable(tableGroupName, table, throwError);
                _Refresh();
            }
        }

        #endregion

        internal IDictionary Cache { get; private set; }
    }
}
