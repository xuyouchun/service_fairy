using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics.Contracts;
using System.Data.SqlClient;

namespace Common.Data
{
    /// <summary>
    /// Sql-Server的查询器
    /// </summary>
    public class SqlDbQuery : DbQuery
    {
        public SqlDbQuery(string connectionString)
        {
            Contract.Requires(connectionString != null);

            _dbConCreator = () => new SqlConnection(connectionString);
        }

        public SqlDbQuery(SqlConnection connection)
        {
            Contract.Requires(connection != null);
            _dbConCreator = () => new SqlConnectionProxy(connection);
        }

        private readonly Func<IDbConnection> _dbConCreator;

        protected override IDbConnection CreateConnection()
        {
            return _dbConCreator();
        }

        protected override void AppendParameter(IDbCommand command, DbQueryParam parameter)
        {
            string name = parameter.Name;
            if(!name.StartsWith("@"))
                name = "@" +name;

            SqlParameter p;
            if (parameter.DbType == DbType.Object)
            {
                p = new SqlParameter(name, parameter.Value);
            }
            else
            {
                p = new SqlParameter(name, parameter.DbType);
                p.Value = parameter.Value;
            }

            if (parameter.Size != 0)
                p.Size = parameter.Size;

            ((SqlCommand)command).Parameters.Add(p);
        }

        protected override IDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }

        #region Class SqlConnectionProxy ...

        class SqlConnectionProxy : IDbConnection
        {
            public SqlConnectionProxy(SqlConnection con)
            {
                _con = con;
                if (con.State == ConnectionState.Closed)
                    con.Open();
            }

            private readonly SqlConnection _con;

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                return _con.BeginTransaction(il);
            }

            public IDbTransaction BeginTransaction()
            {
                return _con.BeginTransaction();
            }

            public void ChangeDatabase(string databaseName)
            {
                _con.ChangeDatabase(databaseName);
            }

            public void Close()
            {
                
            }

            public string ConnectionString
            {
                get
                {
                    return _con.ConnectionString;
                }
                set
                {
                    _con.ConnectionString = value;
                }
            }

            public int ConnectionTimeout
            {
                get { return _con.ConnectionTimeout; }
            }

            public IDbCommand CreateCommand()
            {
                return _con.CreateCommand();
            }

            public string Database
            {
                get { return _con.Database; }
            }

            public void Open()
            {
                _con.Open();
            }

            public ConnectionState State
            {
                get { return _con.State; }
            }

            public void Dispose()
            {
                
            }
        }

        #endregion

    }
}
