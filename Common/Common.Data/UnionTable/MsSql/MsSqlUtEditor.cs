using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using System.Diagnostics.Contracts;
using Common.Package;

namespace Common.Data.UnionTable.MsSql
{
    class MsSqlUtEditor : IUtEditor
    {
        public MsSqlUtEditor(MtConnectionPoint conPoint)
        {
            MsSqlUtility.ValidateConnectionType(conPoint);
            ConnectionPoint = conPoint;
        }

        /// <summary>
        /// 连接点
        /// </summary>
        public MtConnectionPoint ConnectionPoint { get; private set; }

        /// <summary>
        /// 元数据库连接字符串
        /// </summary>
        public string ConStr { get; private set; }

        /// <summary>
        /// 初始化元数据结构
        /// </summary>
        public bool InitMetaDataSchema()
        {
            using (SqlDbQuery dbQuery = new SqlDbQuery(ConnectionPoint.Connection.ConStr))
            {
                return MsSqlUtility.Revise(dbQuery, _mtReviseInfos);
            }
        }

        #region Metadata table revise infos ...

        private static readonly UtTableReviseInfo[] _mtReviseInfos = new UtTableReviseInfo[] {

            // 连接
            new UtTableReviseInfo { TableName = "Connection", Columns = new MtColumn[] {
                    new MtColumn("ID", DbColumnType.Guid, DbColumnIndexType.Master),
                    new MtColumn("Name", DbColumnType.String, 128, DbColumnIndexType.Slave),
                    new MtColumn("Usage", DbColumnType.String, 128),
                    new MtColumn("ConType", DbColumnType.String, 32),
                    new MtColumn("ConStr", DbColumnType.String, 1024),
                }
            },

            // 连接点
            new UtTableReviseInfo { TableName = "ConnectionPoint", Columns = new MtColumn[] {
                    new MtColumn("ID", DbColumnType.Guid, DbColumnIndexType.Master),
                    new MtColumn("Name", DbColumnType.String, 128, DbColumnIndexType.Slave),
                    new MtColumn("ConnectionId", DbColumnType.Guid),
                    new MtColumn("Usage", DbColumnType.String, 128),
                }
            },

            // 表组
            new UtTableReviseInfo { TableName = "TableGroup", Columns = new MtColumn[] {
                    new MtColumn("ID", DbColumnType.Guid, DbColumnIndexType.Master),
                    new MtColumn("Name", DbColumnType.String, 128, DbColumnIndexType.Slave),
                    new MtColumn("RouteType", DbColumnType.String, 32),
                    new MtColumn("RouteArgs", DbColumnType.String, 128),
                    new MtColumn("RouteKey", DbColumnType.String, 128),
                    new MtColumn("RouteKeyColumnType", DbColumnType.String, 32),
                }
            },

            // 表
            new UtTableReviseInfo { TableName = "Table", Columns = new MtColumn[] {
                    new MtColumn("ID", DbColumnType.Guid, DbColumnIndexType.Master),
                    new MtColumn("Name", DbColumnType.String, 128, DbColumnIndexType.Slave),
                    new MtColumn("DefaultGroup", DbColumnType.String, 128),
                    new MtColumn("PrimaryKey", DbColumnType.String, 128),
                    new MtColumn("PrimaryKeyColumnType", DbColumnType.String, 32),
                    new MtColumn("PartialTableCount", DbColumnType.Int32),
                    new MtColumn("TableGroupId", DbColumnType.Guid),
                }
            },

            // 列
            new UtTableReviseInfo { TableName = "Column", Columns = new MtColumn[] {
                    new MtColumn("ID", DbColumnType.Guid, DbColumnIndexType.Master),
                    new MtColumn("Name", DbColumnType.String, 128, DbColumnIndexType.Slave),
                    new MtColumn("TableId", DbColumnType.Guid),
                    new MtColumn("Type", DbColumnType.String, 32),
                    new MtColumn("Size", DbColumnType.Int32),
                    new MtColumn("IndexType", DbColumnType.String, 32),
                }
            },

            // 自增主键
            new UtTableReviseInfo { TableName = "PrimaryKey", Columns = new MtColumn[] {
                    new MtColumn("TableId", DbColumnType.Guid, DbColumnIndexType.Master),
                    new MtColumn("Value", DbColumnType.Int64),
                }
            },
        };

        #endregion

        /// <summary>
        /// 修正表结构
        /// </summary>
        /// <param name="tableReviseInfos"></param>
        public bool ReviseTableSchema(UtTableReviseInfo[] tableReviseInfos)
        {
            Contract.Requires(tableReviseInfos != null);

            using (SqlDbQuery dbQuery = new SqlDbQuery(ConnectionPoint.Connection.ConStr))
            {
                return MsSqlUtility.Revise(dbQuery, tableReviseInfos);
            }
        }
    }
}
