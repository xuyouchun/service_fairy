using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics.Contracts;

namespace Common.Data
{
    /// <summary>
    /// DbQuery所用的事务
    /// </summary>
    public class DbTranscation : ITranscation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbQuery"></param>
        /// <param name="connection"></param>
        /// <param name="transcation"></param>
        public DbTranscation(IDbQuery dbQuery, IDbConnection connection, IDbTransaction transcation)
        {
            Contract.Requires(dbQuery != null && connection != null && transcation != null);

            DbQuery = dbQuery;
            Connection = connection;
            Transcation = transcation;
        }

        /// <summary>
        /// 数据查询器
        /// </summary>
        public IDbQuery DbQuery { get; private set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// 数据库事务
        /// </summary>
        public IDbTransaction Transcation { get; private set; }

        private volatile bool _done;
        private readonly object _thisLock = new object();

        private void _DoThreadSafe(Action action)
        {
            if (_done)
                return;

            lock (_thisLock)
            {
                if (!_done)
                {
                    GC.SuppressFinalize(this);

                    using (DbQuery)
                    using (Connection)
                    {
                        action();
                        _done = true;
                    }
                }
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void Commit()
        {
            _DoThreadSafe(Transcation.Commit);
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public virtual void Rollback()
        {
            _DoThreadSafe(() => {
                if (Transcation.Connection != null && Transcation.Connection.State == ConnectionState.Open)
                    Transcation.Rollback();
            });
        }

        public virtual void Dispose()
        {
            _DoThreadSafe(() => { });
        }

        ~DbTranscation()
        {
            Dispose();
        }
    }
}
