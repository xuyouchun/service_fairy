using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using System.Diagnostics.Contracts;
using Common.Data.UnionTable.MsSql;
using Common.Utility;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 连接点创建工厂
    /// </summary>
    public interface IConnectionPointFactory
    {
        /// <summary>
        /// 创建连接点
        /// </summary>
        /// <param name="mtConPoint"></param>
        /// <param name="patialTableIndex"></param>
        /// <returns></returns>
        IConnectionPoint CreateConnectionPoint(MtConnectionPoint mtConPoint, int patialTableIndex = -1);
    }

    /// <summary> 
    /// 连接点创建工厂
    /// </summary>
    public class ConnectionPointFactory : IConnectionPointFactory
    {
        public ConnectionPointFactory()
        {

        }

        public IConnectionPoint CreateConnectionPoint(MtConnectionPoint mtConPoint, int patialTableIndex)
        {
            Contract.Requires(mtConPoint != null);

            return mtConPoint.Tag.Get<int, IConnectionPoint>("ConnectionPoint").GetOrSet(patialTableIndex, (index) => {

                switch (mtConPoint.Connection.ConType)
                {
                    case DbConnectionTypes.MsSql:
                        return new MsSqlConnectionPoint(mtConPoint, index);
                }

                throw new NotSupportedException(string.Format("不支持的连接类型“{0}”", mtConPoint.Connection.ConType));
            });
        }
    }
}
