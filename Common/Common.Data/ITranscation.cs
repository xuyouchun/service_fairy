using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Common.Data
{
    /// <summary>
    /// 数据库的事务
    /// </summary>
    public interface ITranscation : IDisposable
    {
        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();

        /// <summary>
        /// 数据查询器
        /// </summary>
        IDbQuery DbQuery { get; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// 数据库事务
        /// </summary>
        IDbTransaction Transcation { get; }
    }
}
