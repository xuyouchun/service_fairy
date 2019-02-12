using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Data;
using Common.Package;

namespace Common.Data
{
    public abstract class DbQuery : IDbQuery
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        protected abstract IDbConnection CreateConnection();

        protected virtual void AppendParameter(IDbCommand command, DbQueryParam parameter)
        {
            IDbDataParameter p = command.CreateParameter();

            if (parameter.DbType != DbType.Object)
                p.DbType = parameter.DbType;

            p.Direction = parameter.Direction;
            p.Value = parameter.Value;

            if (parameter.Size != 0)
                p.Size = parameter.Size;
        }

        /// <summary>
        /// 创建DataAdapter
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected abstract IDataAdapter CreateDataAdapter(IDbCommand command);

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        protected virtual IDbTransaction OnCreateTrans(IDbConnection con)
        {
            return con.BeginTransaction();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="level"></param>
        public ITranscation BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            _ValidateDispose();

            IDbConnection con = _CreateConnection(true);
            DbTranscation tran = new DbTranscation(this, con, OnCreateTrans(con));

            return tran;
        }

        private IDbConnection _CreateConnection(bool open = false)
        {
            IDbConnection con = CreateConnection();
            if (open)
                con.Open();

            return con;
        }

        private IDbConnection _GetConnection(ITranscation transcation, bool open = false)
        {
            if (transcation != null)
                return new DbConProxy(transcation);

            return _CreateConnection(open);
        }

        #region Class DbConProxy ...

        class DbConProxy : IDbConnection
        {
            public DbConProxy(ITranscation tran)
            {
                _tran = tran;
            }

            private readonly ITranscation _tran;

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                return _tran.Transcation;
            }

            public IDbTransaction BeginTransaction()
            {
                return _tran.Transcation;
            }

            public void ChangeDatabase(string databaseName)
            {
                _tran.Connection.ChangeDatabase(databaseName);
            }

            public void Close()
            {
                
            }

            public string ConnectionString
            {
                get
                {
                    return _tran.Connection.ConnectionString;
                }
                set
                {
                    _tran.Connection.ConnectionString = value;
                }
            }

            public int ConnectionTimeout
            {
                get { return _tran.Connection.ConnectionTimeout; }
            }

            public IDbCommand CreateCommand()
            {
                return _tran.Connection.CreateCommand();
            }

            public string Database
            {
                get { return _tran.Connection.Database; }
            }

            public void Open()
            {
                
            }

            public ConnectionState State
            {
                get { return _tran.Connection.State; }
            }

            public void Dispose()
            {
                
            }
        }

        #endregion

        private T _Execute<T>(string sql, DbQueryParam[] parameters, CommandType commandType, Func<IDbCommand, T> func, ITranscation transcation, bool closeConnection = true)
        {
            Contract.Requires(sql != null);
            _ValidateDispose();

            if (closeConnection)
            {
                using (IDbConnection con = _GetConnection(transcation, true))
                {
                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        _InitDbCommand(cmd, sql, parameters, commandType, transcation);
                        return func(cmd);
                    }
                }
            }
            else
            {
                IDbConnection con = _GetConnection(transcation, true);
                IDbCommand cmd = con.CreateCommand();
                _InitDbCommand(cmd, sql, parameters, commandType, transcation);
                return func(cmd);
            }
        }

        // 初始化DbCommand
        private void _InitDbCommand(IDbCommand cmd, string sql, DbQueryParam[] parameters, CommandType commandType, ITranscation transcation)
        {
            cmd.CommandText = sql;
            cmd.CommandType = commandType;
            if (parameters != null)
            {
                foreach (DbQueryParam parameter in parameters)
                {
                    AppendParameter(cmd, parameter);
                }
            }

            if (transcation != null)
                cmd.Transaction = transcation.Transcation;
        }

        /// <summary>
        /// 执行SQL并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, ITranscation transcation = null)
        {
            return _Execute<int>(sql, parameters, commandType, delegate(IDbCommand cmd) {
                return cmd.ExecuteNonQuery();
            }, transcation);
        }

        /// <summary>
        /// 执行SQL并返回DataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="behavior"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, DbQueryParam[] parameters = null,
            CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, ITranscation transcation = null)
        {
            return _Execute<IDataReader>(sql, parameters, commandType, delegate(IDbCommand cmd) {
                return cmd.ExecuteReader(transcation == null ? (behavior | CommandBehavior.CloseConnection) : (behavior & ~CommandBehavior.CloseConnection));
            }, transcation, false);
        }

        /// <summary>
        /// 执行SQL并返回第一行第一列的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, ITranscation transcation = null)
        {
            return _Execute<object>(sql, parameters, commandType, delegate(IDbCommand cmd) {
                return cmd.ExecuteScalar();
            }, transcation);
        }

        /// <summary>
        /// 执行SQL并返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, ITranscation transcation = null)
        {
            return _Execute<DataSet>(sql, parameters, commandType, delegate(IDbCommand cmd) {
                IDataAdapter dataAdapter = CreateDataAdapter(cmd);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                return ds;
            }, transcation);
        }

        /// <summary>
        /// 执行SQL并返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, ITranscation transcation = null)
        {
            DataSet ds = ExecuteDataSet(sql, parameters, commandType, transcation);
            return ds.Tables.Count == 0 ? null : ds.Tables[0];
        }

        private void _ValidateDispose()
        {
            if (_dispose)
                throw new ObjectDisposedException(this.GetType().ToString());
        }

        private volatile bool _dispose;

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (_dispose)
                return;

            _dispose = true;
        }

        ~DbQuery()
        {
            Dispose();
        }

        /// <summary>
        /// 创建DB访问器
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbQuery Create(string connectionString)
        {
            Contract.Requires(connectionString != null);

            return new SqlDbQuery(connectionString);
        }
    }
}
