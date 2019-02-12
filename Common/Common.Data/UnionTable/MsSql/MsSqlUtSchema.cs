using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Data.UnionTable.Metadata;
using Common.Utility;
using Common.Collection;

namespace Common.Data.UnionTable.MsSql
{
    partial class MsSqlUtSchema : IUtSchema
    {
        public MsSqlUtSchema(MsSqlUtProvider utProvider, MtTable mtTable)
        {
            _utProvider = utProvider;
            _mtTable = mtTable;
        }

        private readonly MsSqlUtProvider _utProvider;
        private readonly MtTable _mtTable;

        /// <summary>
        /// 生成新的主键
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public object[] GenerateNewKeys(int count)
        {
            Contract.Requires(count >= 0);

            if (count == 0)
                return Array<object>.Empty;

            DbColumnType ct = _mtTable.PrimaryKeyColumnType;
            if (ct == DbColumnType.Guid)
            {
                return CollectionUtility.GenerateArray(count, (k) => (object)Guid.NewGuid());
            }
            else if (_IsNumberType(ct))
            {
                PrimaryKeyContext primaryKeyContext =
                    _utProvider.Cache.Get<string, PrimaryKeyContext>(null, () => new IgnoreCaseDictionary<PrimaryKeyContext>())
                    .GetOrSet(_mtTable.Name, (key) => new PrimaryKeyContext(_utProvider, _mtTable));

                return primaryKeyContext.GeneratePrimaryKeys(count);
            }

            throw new NotSupportedException(string.Format("不支持主键类型“{0}”的生成", ct));
        }

        private static bool _IsNumberType(DbColumnType ct)
        {
            return ct == DbColumnType.Int16 || ct == DbColumnType.Int32 || ct == DbColumnType.Int64 || ct == DbColumnType.Int8;
        }
    }
}
