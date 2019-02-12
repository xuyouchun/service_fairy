using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using System.Data;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace Common.Data.UnionTable.MsSql
{
    /// <summary>
    /// MsSql的元数据管理器
    /// </summary>
    class MsSqlMetadataManager : IMetadataManager
    {
        public MsSqlMetadataManager(IConnectionPointFactory conPointFactory, IConnectionPoint metadataConPoint)
        {
            _conPointFactory = conPointFactory;
            ConnectionPoint = metadataConPoint;
        }

        private readonly IConnectionPointFactory _conPointFactory;

        /// <summary>
        /// 连接点
        /// </summary>
        public IConnectionPoint ConnectionPoint { get; private set; }

        #region Class LoadMetaData ...

        /// <summary>
        /// 加载元数据
        /// </summary>
        /// <returns></returns>
        public MtDatabase LoadMetaData()
        {
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                string sql = new[] { "Connection", "ConnectionPoint", "TableGroup", "Table", "Column" }
                    .Select(tableName => "Select * From [" + tableName + "];").JoinBy("\r\n");

                MtDatabase db = new MtDatabase("UnionTable");
                Dictionary<Guid, object> hs = new Dictionary<Guid, object>();

                using (IDataReader reader = dbQuery.ExecuteReader(sql))
                {
                    // 连接
                    _ReadConnections(hs, db, reader);

                    // 连接点
                    reader.NextResult();
                    _ReadConnectionPoints(hs, db, reader);

                    // 表组
                    reader.NextResult();
                    _ReadTableGroups(hs, db, reader);

                    // 表
                    reader.NextResult();
                    _ReadTables(hs, db, reader);

                    // 列
                    reader.NextResult();
                    _ReadColumns(hs, db, reader);
                }

                return db;
            }
        }

        // 连接
        private void _ReadConnections(Dictionary<Guid, object> hs, MtDatabase db, IDataReader reader)
        {
            while (reader.Read())
            {
                Guid id = reader["ID"].ToType<Guid>();
                string name = reader["Name"].ToType<string>();
                string usage = reader["Usage"].ToType<string>();
                string conType = reader["ConType"].ToType<string>();
                string conStr = reader["ConStr"].ToType<string>();

                MtConnection con = new MtConnection(name, "", conType, usage, conStr);
                db.Connections.Add(con);

                hs.Add(id, con);
            }
        }

        // 连接点
        private void _ReadConnectionPoints(Dictionary<Guid, object> hs, MtDatabase db, IDataReader reader)
        {
            while (reader.Read())
            {
                Guid id = reader["ID"].ToType<Guid>();
                string name = reader["Name"].ToType<string>();
                Guid connectionId = reader["ConnectionId"].ToType<Guid>();
                string usage = reader["Usage"].ToType<string>();

                object con;
                if (hs.TryGetValue(connectionId, out con) && con is MtConnection)
                {
                    MtConnectionPoint conPoint = new MtConnectionPoint(name, "", usage, (MtConnection)con);
                    hs.Add(id, conPoint);

                    ((MtConnection)con).ConnectionPoints.Add(conPoint);
                }
                else
                {
                    throw new DbMetadataException(string.Format("连接点“{0}”的物理连接丢失", name));
                }
            }
        }

        // 读取表组
        private void _ReadTableGroups(Dictionary<Guid, object> hs, MtDatabase db, IDataReader reader)
        {
            while (reader.Read())
            {
                Guid id = reader["ID"].ToType<Guid>();
                string name = reader["Name"].ToType<string>();
                string routeKey = reader["RouteKey"].ToType<string>();
                DbRouteType routeType = reader["RouteType"].ToType<DbRouteType>();
                string routeArgs = reader["RouteArgs"].ToType<string>();
                DbColumnType routeKeyColumnType = reader["RouteKeyColumnType"].ToType<DbColumnType>();
                MtTableGroup tg = new MtTableGroup(name, "", routeKey, routeType, routeArgs, routeKeyColumnType);

                db.TableGroups.Add(tg);
                hs.Add(id, tg);
            }
        }

        // 读取表
        private void _ReadTables(Dictionary<Guid, object> hs, MtDatabase db, IDataReader reader)
        {
            while (reader.Read())
            {
                Guid id = reader["ID"].ToType<Guid>();
                string name = reader["Name"].ToType<string>();
                string primaryKey = reader["PrimaryKey"].ToType<string>();
                string defaultGroup = reader["DefaultGroup"].ToType<string>();
                int partialTableCount = reader["PartialTableCount"].ToType<int>();
                DbColumnType primaryKeyType = reader["PrimaryKeyColumnType"].ToType<DbColumnType>();

                Guid tableGroupId = reader["TableGroupId"].ToType<Guid>();

                MtTable mt = new MtTable(name, "", defaultGroup, primaryKey, primaryKeyType, partialTableCount);
                hs.Add(id, mt);

                object tg;
                if (hs.TryGetValue(tableGroupId, out tg) && tg is MtTableGroup)
                    ((MtTableGroup)tg).Tables.Add(mt);
            }
        }

        // 读取列
        private void _ReadColumns(Dictionary<Guid, object> hs, MtDatabase db, IDataReader reader)
        {
            while (reader.Read())
            {
                Guid id = reader["ID"].ToType<Guid>();
                string name = reader["Name"].ToType<string>();
                Guid tableId = reader["TableId"].ToType<Guid>();
                DbColumnType columnType = reader["Type"].ToType<DbColumnType>();
                int size = reader["Size"].ToType<int>();
                DbColumnIndexType indexType = reader["IndexType"].ToType<DbColumnIndexType>();

                MtColumn col = new MtColumn(name, columnType, size, indexType);
                hs.Add(id, col);

                object mt;
                if (hs.TryGetValue(tableId, out mt) && mt is MtTable)
                    ((MtTable)mt).Columns.Add(col);
            }
        }

        #endregion

        /// <summary>
        /// 添加一个物理连接
        /// </summary>
        /// <param name="con"></param>
        /// <param name="throwError"></param>
        public bool AddConnection(MtConnection con, bool throwError = false)
        {
            Contract.Requires(con != null);

            _conPointFactory.CreateConnectionPoint(new MtConnectionPoint("", "", "", con)).Check();

            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                string sql =
@"If Not Exists ( Select * From [Connection] Where Name = @name ) Begin 
    Insert Into [Connection] ([ID], [Name], [Usage], [ConType], [ConStr]) Values ( @Id, @Name, @Usage, @ConType, @ConStr );
End Else Begin
    Update [Connection] Set [Usage] = @Usage, [ConType] = @ConType, [ConStr] = @ConStr Where Name = @Name;
End;";

                int affectCount = dbQuery.ExecuteNonQuery(sql, new DbQueryParam[] {
                    new DbQueryParam("@Id", Guid.NewGuid()),
                    new DbQueryParam("@Name", con.Name),
                    new DbQueryParam("@Usage", con.Usage),
                    new DbQueryParam("@ConType", con.ConType),
                    new DbQueryParam("@ConStr", con.ConStr),
                });

                return true;
            }
        }

        /// <summary>
        /// 删除一个物理连接
        /// </summary>
        /// <param name="name"></param>
        /// <param name="throwError"></param>
        public bool DropConnection(string name, bool throwError = false)
        {
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                string sql =
@"If Not Exists ( Select * From [Connection] con Join [ConnectionPoint] conPt On con.ID = conPt.ConnectionId Where con.Name = @Name ) Begin
    Delete From [ConnectionPoint] Where [Name] = @Name;
    Select 1;
Else
    Select 0;
End;";
                int result = (int)dbQuery.ExecuteScalar(sql, new DbQueryParam[] {
                    new DbQueryParam("@Name", name)
                });

                if (result == 1)
                    return true;

                if (throwError)
                    throw new DbMetadataException(string.Format("连接“{0}”尚在使用中", name));

                return false;
            }
        }

        /// <summary>
        /// 添加一个连接点
        /// </summary>
        /// <param name="conPoint"></param>
        /// <param name="throwError"></param>
        public bool AddConnectionPoint(MtConnectionPoint conPoint, bool throwError)
        {
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                string sql =
@"Declare @ConnectionId As UniqueIdentifier;

Begin Tran myTran;
Select @ConnectionId = ID From [Connection] Where Name = @ConnectionName;

If @ConnectionId Is Null Begin
    Select 1;
End Else Begin
    If Not Exists ( Select * From [ConnectionPoint] Where Name = @Name ) Begin
        Insert Into [ConnectionPoint] ( [ID], [Name], [ConnectionId], [Usage] ) Values ( @ID, @Name, @ConnectionId, @Usage );
        Select 0;
    End Else Begin
        Select 2;
    End;
End;

Commit Tran myTran;";

                int errorNo = (int)dbQuery.ExecuteScalar(sql, new DbQueryParam[] {
                    new DbQueryParam(@"ID", Guid.NewGuid()),
                    new DbQueryParam("@ConnectionName", conPoint.Connection.Name),
                    new DbQueryParam("@Name", conPoint.Name),
                    new DbQueryParam("@Usage", conPoint.Usage),
                });

                if (errorNo == 0)
                    return true;

                if (throwError)
                {
                    throw new DbMetadataException(errorNo == 1 ?
                        string.Format("连接“{0}”不存在", conPoint.Connection.Name) :
                        string.Format("连接点“{0}”已经存在", conPoint.Name));
                }

                return false;
            }
        }

        /// <summary>
        /// 添加一个表组
        /// </summary>
        /// <param name="group"></param>
        /// <param name="throwError"></param>
        public bool AddTableGroup(MtTableGroup group, bool throwError)
        {
            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                const string sql =
@"If Not Exists ( Select * From [TableGroup] Where [Name] = @Name ) Begin
    Insert Into [TableGroup] ([ID], [Name], [RouteKey], [RouteKeyColumnType], [RouteType], [RouteArgs]) Values 
        ( @ID, @Name, @RouteKey, @RouteKeyColumnType, @RouteType, @RouteArgs );
End Else Begin
    Update [TableGroup] Set [RouteKey] = @RouteKey, [RouteKeyColumnType] = @RouteKeyColumnType, [RouteType] = @RouteType,
        [RouteArgs] = @RouteArgs Where [Name] = @Name
End;";
                dbQuery.ExecuteScalar(sql, new DbQueryParam[] {
                    new DbQueryParam("@ID", Guid.NewGuid()),
                    new DbQueryParam("@Name", group.Name),
                    new DbQueryParam("@RouteKey", group.RouteKey),
                    new DbQueryParam("@RouteKeyColumnType", group.RouteKeyColumnType.ToString()),
                    new DbQueryParam("@RouteType", group.RouteType.ToString()),
                    new DbQueryParam("@RouteArgs", group.RouteArgs),
                });

                return true;
            }
        }

        /// <summary>
        /// 添加表
        /// </summary>
        /// <param name="fullTableName">表全名</param>
        /// <param name="columns">列</param>
        /// <param name="patialTableCount">分表数量</param>
        /// <param name="throwError"></param>
        public void AddTable(string tableGroupName, MtTable table, bool throwError)
        {
            Contract.Requires(tableGroupName!=null && table != null);

            using (IDbQuery dbQuery = ConnectionPoint.CreateDbQuery())
            {
                string sql = _CreateAddTableSql(table);
                dbQuery.ExecuteScalar(sql, new DbQueryParam[] {
                    new DbQueryParam("@TableGroupName", tableGroupName),
                    new DbQueryParam("@TableId", Guid.NewGuid()),
                    new DbQueryParam("@TableName", table.Name),
                    new DbQueryParam("@DefaultGroup", table.DefaultGroup),
                    new DbQueryParam("@PrimaryKey", table.PrimaryKey),
                    new DbQueryParam("@PrimaryKeyColumnType", table.PrimaryKeyColumnType.ToString()),
                    new DbQueryParam("@PartialTableCount", table.PartialTableCount),
                });
            }
        }

        private string _CreateAddTableSql(MtTable table)
        {
            StringBuilder sql = new StringBuilder(
    @"Declare @tableGroupId As UniqueIdentifier;
Select @tableGroupId = [ID] From [TableGroup] tg Where tg.Name = @TableGroupName;
If @tableGroupId Is Null Begin
    Select 1;
End Else Begin  -- 添加表
    If Not Exists ( Select * From [Table] Where Name = @TableName ) Begin
        Insert Into [Table] ([ID], [Name], [DefaultGroup], [PrimaryKey], [PrimaryKeyColumnType], [TableGroupId], [PartialTableCount]) Values (
            @TableId, @TableName, @DefaultGroup, @PrimaryKey, @PrimaryKeyColumnType, @tableGroupId, @PartialTableCount);
    End Else Begin
        Update [Table] Set [DefaultGroup] = @DefaultGroup, [PrimaryKey] = @PrimaryKey, [PrimaryKeyColumnType] = @PrimaryKeyColumnType,
            [TableGroupId] = @tableGroupId, [PartialTableCount] = @PartialTableCount Where [Name] = @TableName;
        Select @TableId = [ID] From [Table] Where [Name] = @TableName;
    End;

    With s As ( Select * From (Values 
");

            // 添加字段
            int k = 0;
            foreach (MtColumn col in table.Columns)
            {
                if (k++ > 0)
                    sql.Append(",\r\n");

                sql.AppendSqlFormat("( {0}, {1}, {2}, {3}, {4} )", Guid.NewGuid(), col.Name, col.Type.ToString(), col.Size, col.IndexType.ToString());
            }

            sql.Append(
@") As _MyTable([ID], [Name], [Type], [Size], [IndexType])),
    t As ( Select * From [Column] Where TableId = @TableId )
    Merge Into t Using s On s.[Name] = t.[Name]
    When Matched Then
        Update Set [Type] = s.[Type], [Size] = s.[Size], [IndexType] = s.[IndexType]
    When Not Matched By Target Then
        Insert ([ID], [Name], [Type], [Size], [IndexType], [TableId]) Values (s.[ID], s.[Name], s.[Type], s.[Size], s.[IndexType], @TableId)
    ;

End;");

            return sql.ToString();
        }
    }
}
