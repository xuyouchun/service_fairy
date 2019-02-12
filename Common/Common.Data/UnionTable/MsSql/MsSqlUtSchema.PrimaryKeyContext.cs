using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using Common.Utility;

namespace Common.Data.UnionTable.MsSql
{
    partial class MsSqlUtSchema
    {
        class PrimaryKeyContext
        {
            public PrimaryKeyContext(MsSqlUtProvider utProvider, MtTable mtTable)
            {
                _utProvider = utProvider;
                _mtTable = mtTable;
                _primaryKeyType = mtTable.PrimaryKeyColumnType.ToType();
            }

            private readonly MsSqlUtProvider _utProvider;
            private readonly MtTable _mtTable;
            private readonly Type _primaryKeyType;
            private long _start = 0;
            private int _count = 0;

            /// <summary>
            /// 生成新的主键
            /// </summary>
            /// <param name="count"></param>
            /// <returns></returns>
            public object[] GeneratePrimaryKeys(int count)
            {
                lock (this)
                {
                    object[] r = new object[count];
                    int k = 0, c = Math.Min(_count, count);
                    for (; k < c; k++)
                    {
                        r[k] = _start++;
                    }

                    _count -= c;

                    if (k < r.Length)
                    {
                        _LoadPrimaryKeys(Math.Max(r.Length - k, 64));

                        for (; k < r.Length; k++)
                        {
                            r[k] = Convert.ChangeType(_start++, _primaryKeyType);
                            _count--;
                        }
                    }

                    return r;
                }
            }

            private void _LoadPrimaryKeys(int count)
            {
                IConnectionPoint conPoint = _utProvider.ConnectionPointManager.GetMetadataConnectionPoint();
                using (IDbQuery dbQuery = conPoint.CreateDbQuery())
                {
                    const string sql =
    @"Declare @TableId As UniqueIdentifier;
Select @TableId = [ID] From [Table] Where [Name] = @TableName;
Begin Tran myTran;
If @TableId Is Null Begin
    Select Null;
End Else Begin
    Declare @CurValue As BigInt;
    Select @CurValue = [Value] From [PrimaryKey] With (RowLock) Where [TableId] = @TableId;
    If @CurValue Is Null Begin
        Insert Into [PrimaryKey] ( [TableId], [Value] ) Values ( @TableId, @Count + 1 );
        Select 1;
    End Else Begin
        Update [PrimaryKey] With (RowLock) Set [Value] = [Value] + @Count Where [TableId] = @TableId;
        Select @CurValue;
    End;
End;
Commit Tran myTran;
";

                    object start = dbQuery.ExecuteScalar(sql, new DbQueryParam[] {
                        new DbQueryParam("@TableName", _mtTable.Name),
                        new DbQueryParam("@Count", count),
                    });

                    if (start == null || start is DBNull)
                        throw new DbMetadataException(string.Format("生成主键时出现错误：表“{0}”不存在", _mtTable.Name));

                    _start = start.ToTypeWithError<long>();
                    _count = count;
                }
            }
        }
    }
}
