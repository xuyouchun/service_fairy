using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using Common.Package;

namespace Common.Data.UnionTable.MsSql
{
    /// <summary>
    /// MsSql连接点
    /// </summary>
    class MsSqlConnectionPoint : IConnectionPoint
    {
        public MsSqlConnectionPoint(MtConnectionPoint conPoint, int partialTableIndex = -1)
        {
            ConnectionPoint = conPoint;
            PartialTableIndex = partialTableIndex;
        }

        /// <summary>
        /// 连接点元数据
        /// </summary>
        public MtConnectionPoint ConnectionPoint { get; private set; }

        /// <summary>
        /// 分表索引号
        /// </summary>
        public int PartialTableIndex { get; private set; }

        /// <summary>
        /// 表结构编辑器
        /// </summary>
        public IUtEditor CreateUtEditor()
        {
            return new MsSqlUtEditor(ConnectionPoint);
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbQuery CreateDbQuery()
        {
            return new SqlDbQuery(ConnectionPoint.Connection.ConStr);
        }

        /// <summary>
        /// 检查连接的有效性
        /// </summary>
        public void Check()
        {
            using (IDbQuery dbQuery = CreateDbQuery())
            {
                string sql =
@"Select Top 0 Null From Sys.Databases;
If Object_Id('_TEMPTABLE_') Is Not Null Drop Table _TEMPTABLE_;
Create Table _TEMPTABLE_ (ID Int);
Insert Into _TEMPTABLE_ (ID) Values (1);
Update _TEMPTABLE_ Set ID = 0;
Select * From _TEMPTABLE_;
Delete From _TEMPTABLE_;
Drop Table _TEMPTABLE_;
";
                dbQuery.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 创建表连接
        /// </summary>
        /// <param name="utCtx"></param>
        /// <returns></returns>
        public IUtConnection CreateTableConnection(UtContext utCtx)
        {
            return new MsSqlUtConnection(utCtx, this, PartialTableIndex);
        }
    }
}
